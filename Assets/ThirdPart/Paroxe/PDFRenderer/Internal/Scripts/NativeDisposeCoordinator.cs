using System;
using System.Collections.Generic;
using System.Linq;

namespace Paroxe.PdfRenderer.Internal
{
	public interface ICoordinatedNativeDisposable
    {
        IntPtr NativePointer { get; }
        ICoordinatedNativeDisposable NativeParent { get; }

        Action<IntPtr> GetDisposeMethod();
    }

#if !UNITY_WEBGL || UNITY_EDITOR
    public struct NativePointerrWithType
    {
        public Type ManagedType;
        public IntPtr NativePointerr;

        public NativePointerrWithType(Type managedType, IntPtr nativePointerr)
        {
            ManagedType = managedType;
            NativePointerr = nativePointerr;
        }
    }

    public class NativeDisposeCoordinator
    {
        private readonly NativeDependencyTree m_DependencyRoot;
        private readonly Dictionary<NativePointerrWithType, NativeDependencyTree> m_DependencyMap;
        private readonly Dictionary<Type, Action<IntPtr>> m_DisposeMethods = new Dictionary<Type, Action<IntPtr>>();
        private bool m_IsLibraryInitialized;

        private List<NativeDependencyTree> m_PendingDisposes = new List<NativeDependencyTree>(128);
        private static Predicate<NativeDependencyTree> m_RemoveDisposedNodesPredicate = t => t.IsDisposed;

        public bool IsLibraryInitialized
        {
            get { return m_IsLibraryInitialized; }
        }

        public NativeDisposeCoordinator()
        {
            m_DependencyRoot = new NativeDependencyTree(new NativePointerrWithType(null, IntPtr.Zero), null);
            m_DependencyMap = new Dictionary<NativePointerrWithType, NativeDependencyTree>();
        }

        public void EnsureNativeLibraryIsInitialized()
        {
            lock (PDFLibrary.nativeLock)
            {
                if (!m_IsLibraryInitialized)
                {
                    NativeMethods.FPDF_InitLibrary();

                    m_IsLibraryInitialized = true;
                }
            }
        }

        public void AddReference(ICoordinatedNativeDisposable nativeObject)
        {
            lock (PDFLibrary.nativeLock)
            {
                EnsureNativeLibraryIsInitialized();

                NativeDependencyTree node;
                NativePointerrWithType handlerWithType = new NativePointerrWithType(nativeObject.GetType(), nativeObject.NativePointer);

                if (!m_DependencyMap.TryGetValue(handlerWithType, out node))
                {
                    if (nativeObject.NativeParent == null)
                    {
                        node = m_DependencyRoot.AddChild(handlerWithType);
                    }
                    else
                    {
                        NativeDependencyTree parentNode = null;

                        if (m_DependencyMap.TryGetValue(new NativePointerrWithType(nativeObject.NativeParent.GetType(), nativeObject.NativeParent.NativePointer), out parentNode))
                        {
                            node = parentNode.AddChild(handlerWithType);
                        }
                        else
                            throw new InvalidOperationException("Supplied parent NativePointerr is not found within the dependencies tree.");
                    }
                    
                    m_DependencyMap.Add(handlerWithType, node);
                }

                ++node.ReferenceCount;
            }
        }

        public void RemoveReference(ICoordinatedNativeDisposable nativeObject)
        {
            if (!m_DisposeMethods.ContainsKey(nativeObject.GetType()))
                m_DisposeMethods.Add(nativeObject.GetType(), nativeObject.GetDisposeMethod());

            lock (PDFLibrary.nativeLock)
            {
                NativeDependencyTree node = null;

                NativePointerrWithType handlerWithType = new NativePointerrWithType(nativeObject.GetType(), nativeObject.NativePointer);

                if (m_DependencyMap.TryGetValue(handlerWithType, out node))
                {
                    --node.ReferenceCount;

                    if (node.ReferenceCount == 0)
                    {
                        if (node.ChildCount == 0)
                        {
                            DisposeNativeObject(node);

                            node.Parent.RemoveChild(node);

                            m_DependencyMap.Remove(node.NativePointerrWithType);
                        }
                        else
                        {
                            m_PendingDisposes.Add(node);
                        }
                    }
                }

                bool disposing = false;
                do
                {
                    disposing = ProcessPendingDisposes();
                }
                while (disposing);

                if (m_IsLibraryInitialized && m_DependencyMap.Count == 0)
                {
                    NativeMethods.FPDF_DestroyLibrary();

                    m_IsLibraryInitialized = false;
                }
            }
        }

        private void DisposeNativeObject(NativeDependencyTree handler)
        {
            m_DisposeMethods[handler.NativePointerrWithType.ManagedType](handler.NativePointerrWithType.NativePointerr);

            handler.IsDisposed = true;
        }

        private bool ProcessPendingDisposes()
        {
            if (!m_PendingDisposes.Any())
                return false;

            bool disposing = false;

            foreach (NativeDependencyTree node in m_PendingDisposes)
            {
                if (node.ChildCount == 0)
                {
                    disposing = true;

                    node.Parent.RemoveChild(node);
                    m_DependencyMap.Remove(node.NativePointerrWithType);

                    DisposeNativeObject(node);
                }
            }

            if (disposing)
                m_PendingDisposes.RemoveAll(m_RemoveDisposedNodesPredicate);

            return disposing;
        }
    }

    public class NativeDependencyTree
    {
        private readonly NativePointerrWithType m_NativePointerWithType;
        private readonly LinkedList<NativeDependencyTree> m_Children;
        private readonly NativeDependencyTree m_Parent;

        private int m_ReferenceCount;
        private bool m_IsDisposed;

        public NativePointerrWithType NativePointerrWithType
        {
            get { return m_NativePointerWithType; }
        }

        public int ReferenceCount
        {
            get { return m_ReferenceCount; }
            set { m_ReferenceCount = value; }
        }

        public bool IsDisposed
        {
            get { return m_IsDisposed; }
            set { m_IsDisposed = value; }
        }

        public int ChildCount
        {
            get { return m_Children.Count; }
        }

        public NativeDependencyTree Parent
        {
            get { return m_Parent; }
        }

        public LinkedList<NativeDependencyTree> Children
        {
            get { return m_Children; }
        }

        public NativeDependencyTree(NativePointerrWithType nativePointerrWithType, NativeDependencyTree parentNode)
        {
            m_NativePointerWithType = nativePointerrWithType;
            m_Parent = parentNode;
            m_Children = new LinkedList<NativeDependencyTree>();
        }

        public NativeDependencyTree AddChild(NativePointerrWithType nativePointerrWithType)
        {
            NativeDependencyTree node = new NativeDependencyTree(nativePointerrWithType, this);

            m_Children.AddFirst(node);

            return node;
        }

        public void RemoveChild(NativeDependencyTree node)
        {
            m_Children.Remove(node);
        }
    }
#endif
}

