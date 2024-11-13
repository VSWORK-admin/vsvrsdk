using System;
using Paroxe.PdfRenderer.Internal;

namespace Paroxe.PdfRenderer
{
#if !UNITY_WEBGL
    /// <summary>
    /// Represents a destination into a PDF document.
    /// </summary>
    public sealed class PDFDest
    {
	    private IntPtr m_NativePointer;
        private object m_Source;
        private PDFDocument m_Document;
        private int? m_PageIndex;

        public PDFDest(PDFAction action, IntPtr nativePointer)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            if (nativePointer == IntPtr.Zero)
                throw new ArgumentNullException("nativePointer");

            m_Source = action;
            m_Document = action.Document;
            m_NativePointer = nativePointer;
        }

        public PDFDest(PDFLink link, IntPtr nativePointer)
        {
            if (link == null)
                throw new ArgumentNullException("link");
            if (nativePointer == IntPtr.Zero)
                throw new ArgumentNullException("nativePointer");

            m_Source = link;
            m_Document = link.Page.Document;
            m_NativePointer = nativePointer;
        }

        public PDFDest(PDFBookmark bookmark, IntPtr nativePointer)
        {
            if (bookmark == null)
                throw new ArgumentNullException("bookmark");
            if (nativePointer == IntPtr.Zero)
                throw new ArgumentNullException("nativePointer");

            m_Source = bookmark;
            m_Document = bookmark.Document;
            m_NativePointer = nativePointer;
        }

        public PDFDocument Document
        {
            get { return m_Document; }
        }

        public object Source
        {
            get { return m_Source; }
        }

        public IntPtr NativePointer
        {
            get { return m_NativePointer; }
        }

        public int PageIndex
        {
            get
            {
                if (!m_PageIndex.HasValue)
                    m_PageIndex = (int)NativeMethods.FPDFDest_GetDestPageIndex(m_Document.NativePointer, m_NativePointer);
                return m_PageIndex.Value;
            }
        }
    }
#endif
}