using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Paroxe.PdfRenderer.Internal;

namespace Paroxe.PdfRenderer
{
#if !UNITY_WEBGL
    /// <summary>
    /// Represent a search session within a specific page. To search in entire document use PDFTextPage.Search
    /// </summary>
    public sealed class PDFSearchHandle : IDisposable, ICoordinatedNativeDisposable
    {
        public enum MatchOption
        {
	        NONE = 0x00000000,
	        MATCH_CASE = 0x00000001,
	        MATCH_WHOLE_WORD = 0x00000002,
	        MATCH_CASE_AND_WHOLE_WORD = 0x00000003
        }

        private IntPtr m_NativePointer;
        private PDFTextPage m_TextPage;

        public PDFSearchHandle(PDFTextPage textPage, byte[] findWhatUnicode, int startIndex, MatchOption flags = MatchOption.NONE)
        {
            if (textPage == null)
                throw new ArgumentNullException("textPage");
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex");

            m_TextPage = textPage;

            PDFLibrary.Instance.DisposeCoordinator.EnsureNativeLibraryIsInitialized();

            IntPtr unmanagedPointer = Marshal.AllocHGlobal(findWhatUnicode.Length);
            Marshal.Copy(findWhatUnicode, 0, unmanagedPointer, findWhatUnicode.Length);

            lock (PDFLibrary.nativeLock)
            {
	            m_NativePointer = NativeMethods.FPDFText_FindStart(textPage.NativePointer, unmanagedPointer, (uint)flags, startIndex);

	            Marshal.FreeHGlobal(unmanagedPointer);

                if (m_NativePointer != IntPtr.Zero)
					PDFLibrary.Instance.DisposeCoordinator.AddReference(this);
            }
        }

        ~PDFSearchHandle()
        {
	        Close();
        }

        public void Dispose()
        {
	        Close();

            GC.SuppressFinalize(this);
        }

        private void Close()
        {
	        if (m_NativePointer == IntPtr.Zero)
		        return;

	        PDFLibrary.Instance.DisposeCoordinator.RemoveReference(this);

	        m_NativePointer = IntPtr.Zero;
        }

        public IntPtr NativePointer
        {
            get { return m_NativePointer; }
        }

        /// <summary>
        /// Return an array containing all the searchResults. If there is no result, this function return null.
        /// </summary>
        /// <returns></returns>
        public IList<PDFSearchResult> GetResults()
        {
            List<PDFSearchResult> results = new List<PDFSearchResult>();

            foreach (PDFSearchResult result in EnumerateSearchResults())
            {
                results.Add(result);
            }

            return results;
        }

        /// <summary>
        /// Enumerate search results.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PDFSearchResult> EnumerateSearchResults()
        {
            if (m_NativePointer != IntPtr.Zero)
            {
                while (NativeMethods.FPDFText_FindNext(m_NativePointer))
                    yield return new PDFSearchResult(
                        m_TextPage.PageIndex, NativeMethods.FPDFText_GetSchResultIndex(m_NativePointer), NativeMethods.FPDFText_GetSchCount(m_NativePointer));
            }
        }

        /// <summary>
        /// Get the next search result. If there is no other result, the function returns an invalid searchResult (validate it with PDFSearchResult.IsValid)
        /// </summary>
        /// <returns></returns>
        public PDFSearchResult FindNext()
        {
            if (NativeMethods.FPDFText_FindNext(m_NativePointer))
                return new PDFSearchResult(
                    m_TextPage.PageIndex, NativeMethods.FPDFText_GetSchResultIndex(m_NativePointer), NativeMethods.FPDFText_GetSchCount(m_NativePointer));
            return new PDFSearchResult(-1, -1, -1);
        }

        /// <summary>
        /// Get the previous search result. If there is no other result, the function returns an invalid searchResult (validate it with PDFSearchResult.IsValid)
        /// </summary>
        /// <returns></returns>
        public PDFSearchResult FindPrevious()
        {
            if (NativeMethods.FPDFText_FindPrev(m_NativePointer))
                return new PDFSearchResult(
                    m_TextPage.PageIndex, NativeMethods.FPDFText_GetSchResultIndex(m_NativePointer), NativeMethods.FPDFText_GetSchCount(m_NativePointer));
            return new PDFSearchResult(-1, -1, -1);
        }

        IntPtr ICoordinatedNativeDisposable.NativePointer
        {
	        get { return m_NativePointer; }
        }

        ICoordinatedNativeDisposable ICoordinatedNativeDisposable.NativeParent
        {
	        get { return m_TextPage; }
        }

        Action<IntPtr> ICoordinatedNativeDisposable.GetDisposeMethod()
        {
	        return NativeMethods.FPDFText_FindClose;
        }
    }

#endif
}