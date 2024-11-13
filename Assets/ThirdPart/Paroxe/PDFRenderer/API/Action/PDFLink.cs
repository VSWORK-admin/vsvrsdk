using System;
using Paroxe.PdfRenderer.Internal;

namespace Paroxe.PdfRenderer
{
#if !UNITY_WEBGL
    /// <summary>
    /// Represents the annotation link in a PDF page.
    /// </summary>
    public sealed class PDFLink
    {
	    private IntPtr m_NativePointer;
        private PDFPage m_Page;
        private PDFDest m_Dest;
        private PDFAction m_Action;
        private bool m_DestCached;
        private bool m_ActionCached;

        public PDFLink(PDFPage page, IntPtr nativePointer)
        {
            if (page == null)
                throw new ArgumentNullException("page");
            if (nativePointer == IntPtr.Zero)
                throw new ArgumentNullException("nativePointer");

            m_Page = page;
            m_NativePointer = nativePointer;
        }

        public PDFPage Page
        {
            get { return m_Page; }
        }

        public IntPtr NativePointer
        {
            get { return m_NativePointer; }
        }

        /// <summary>
        /// Gets the named destination assigned to a link. Return null if there is no destination associated with the link,
        /// in this case the application should try GetAction() instead.
        /// </summary>
        /// <returns></returns>
        public PDFDest GetDest()
        {
	        if (m_DestCached)
		        return m_Dest;

            IntPtr destPtr = NativeMethods.FPDFLink_GetDest(m_Page.Document.NativePointer, m_NativePointer);
            if (destPtr != IntPtr.Zero)
	            m_Dest = new PDFDest(this, destPtr);

            m_DestCached = true;

            return m_Dest;
        }

        /// <summary>
        /// Gets the PDF action assigned to the link.
        /// </summary>
        /// <returns></returns>
        public PDFAction GetAction()
        {
	        if (m_ActionCached)
		        return m_Action;

            IntPtr actionPtr = NativeMethods.FPDFLink_GetAction(m_NativePointer);
            if (actionPtr != IntPtr.Zero)
	            m_Action =  new PDFAction(this, actionPtr);

            m_ActionCached = true;

            return m_Action;
        }

        public bool HasUrl
        {
	        get
	        {
		        PDFAction action = GetAction();
		        if (action == null)
			        return false;

		        return action.GetActionType() == PDFAction.ActionType.Uri;
	        }
        }

        public string Url
        {
	        get
	        {
		        if (!HasUrl)
			        return null;

                PDFAction action = GetAction();

                return action.GetURIPath();
	        }
        }
    }
#endif
}