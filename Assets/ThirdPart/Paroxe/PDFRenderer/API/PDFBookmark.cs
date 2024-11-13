using System;
using System.Collections.Generic;
using Paroxe.PdfRenderer.Internal;

namespace Paroxe.PdfRenderer
{
#if !UNITY_WEBGL
    /// <summary>
    /// Represents the bookmark into a PDF document.
    /// </summary>
    public sealed class PDFBookmark
    {
	    private List<PDFBookmark> m_Bookmarks = new List<PDFBookmark>();
        private PDFDocument m_Document;
        private IntPtr m_NativePointer;
        private PDFBookmark m_ParentBookmark;
        private string m_Title;
        private PDFAction m_Action;
        private PDFDest m_Dest;
        private PDFBookmark m_FirstChild;
        private PDFBookmark m_NextSibling;
        private bool m_ActionCached;
        private bool m_DestCached;
        private bool m_FirstChildCached;
        private bool m_NextSiblingCached;
        
        public PDFBookmark(PDFDocument document, PDFBookmark parentBookmark, IntPtr nativePointer)
        {
            if (document == null)
                throw new ArgumentNullException("document");

            m_ParentBookmark = parentBookmark;
            m_NativePointer = nativePointer;
            m_Document = document;

            if (m_NativePointer == IntPtr.Zero)
	            m_Title = "ROOT";

            PDFBookmark firstChild = GetFirstChild();

            if (firstChild != null)
            {
                PDFBookmark previousSibling = firstChild;

                while (previousSibling != null)
                {
                    m_Bookmarks.Add(previousSibling);

                    previousSibling = previousSibling.GetNextSibling();
                }
            }
        }

        public PDFDocument Document
        {
            get { return m_Document; }
        }

        public int ChildCount
        {
            get { return m_Bookmarks.Count; }
        }

        public bool IsTopLevelBookmark
        {
            get { return (m_ParentBookmark == null || m_ParentBookmark.NativePointer == IntPtr.Zero); }
        }

        public PDFBookmark Parent
        {
            get { return m_ParentBookmark; }
        }

        public IntPtr NativePointer
        {
            get { return m_NativePointer; }
        }

        public IList<PDFBookmark> GetChildrenBookmarks()
        {
            return m_Bookmarks;
        }

        public IEnumerable<PDFBookmark> EnumerateChildrenBookmarks()
        {
            foreach (PDFBookmark child in m_Bookmarks)
            {
                yield return child;
            }
        }

        public void ExecuteBookmarkAction(IPDFDevice device)
        {
            if (device == null)
                throw new ArgumentNullException("device");

            PDFActionHandlerHelper.ExecuteBookmarkAction(this, device);
        }

        public PDFBookmark GetChild(int index)
        {
            if (index < 0 || index >= ChildCount)
                throw new ArgumentOutOfRangeException("index");

            return m_Bookmarks[index];
        }

        public string GetTitle()
        {
            if (string.IsNullOrEmpty(m_Title))
            {
                byte[] buffer = new byte[4096];

                int length = (int) NativeMethods.FPDFBookmark_GetTitle(m_NativePointer, buffer, (uint)buffer.Length);
                if (length > 0)
                    m_Title = PDFLibrary.Encoding.GetString(buffer);
            }
            return m_Title;
        }

        public PDFAction GetAction()
        {
	        if (m_ActionCached)
		        return m_Action;

            IntPtr actionPtr = NativeMethods.FPDFBookmark_GetAction(m_NativePointer);
            if (actionPtr != IntPtr.Zero)
	            m_Action = new PDFAction(this, actionPtr);

            m_ActionCached = true;

            return m_Action;
        }

        public PDFDest GetDest()
        {
	        if (m_DestCached)
		        return m_Dest;

            IntPtr destPtr = NativeMethods.FPDFBookmark_GetDest(m_Document.NativePointer, m_NativePointer);
            if (destPtr != IntPtr.Zero)
	            m_Dest = new PDFDest(this, destPtr);

            m_DestCached = true;

            return m_Dest;
        }

        public PDFBookmark GetFirstChild()
        {
	        if (m_FirstChildCached)
		        return m_FirstChild;

            IntPtr childPtr = NativeMethods.FPDFBookmark_GetFirstChild(m_Document.NativePointer, m_NativePointer);
            if (childPtr != IntPtr.Zero)
	            m_FirstChild = new PDFBookmark(m_Document, this, childPtr);

            m_FirstChildCached = true;

            return m_FirstChild;
        }

        public PDFBookmark GetNextSibling()
        {
	        if (m_NextSiblingCached)
		        return m_NextSibling;

            IntPtr nextPtr = NativeMethods.FPDFBookmark_GetNextSibling(m_Document.NativePointer, m_NativePointer);
            if (nextPtr != IntPtr.Zero)
	            m_NextSibling = new PDFBookmark(m_Document, m_ParentBookmark, nextPtr);

            m_NextSiblingCached = true;

            return m_NextSibling;
        }
    }
#endif
}