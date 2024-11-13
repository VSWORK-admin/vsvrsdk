using Paroxe.PdfRenderer.Internal;
using System.Text;

namespace Paroxe.PdfRenderer
{
#if !UNITY_WEBGL || UNITY_EDITOR
	public sealed class PDFLibrary
	{
		public enum ErrorCode
		{
			ErrSuccess = 0,     // No error.
			ErrUnknown = 1,     // Unknown error.
			ErrFile = 2,        // File not found or could not be opened.
			ErrFormat = 3,      // File not in PDF format or corrupted.
			ErrPassword = 4,    // Password required or incorrect password.
			ErrSecurity = 5,    // Unsupported security scheme.
			ErrPage = 6         // Page not found or content error.
		}

        public static readonly Encoding Encoding = new UnicodeEncoding(false, false, false);

		public static readonly object nativeLock = typeof(NativeMethods);

		private static PDFLibrary m_Instance;

#if (UNITY_IOS || UNITY_WEBGL) && !UNITY_EDITOR
        public const string PLUGIN_ASSEMBLY = "__Internal";
#else
		public const string PLUGIN_ASSEMBLY = "pdfrenderer";
#endif
		private readonly NativeDisposeCoordinator m_DisposeCoordinator;

		public bool IsInitialized
		{
			get { return m_DisposeCoordinator.IsLibraryInitialized; }
		}

		public void EnsureInitialized()
		{
			Instance.DisposeCoordinator.EnsureNativeLibraryIsInitialized();
		}

		private PDFLibrary()
		{
			m_DisposeCoordinator = new NativeDisposeCoordinator();
		}

		public static ErrorCode GetLastError()
		{
			Instance.DisposeCoordinator.EnsureNativeLibraryIsInitialized();

			return (ErrorCode)NativeMethods.FPDF_GetLastError();
		}

		public static PDFLibrary Instance
		{
			get { return m_Instance ?? (m_Instance = new PDFLibrary()); }
		}

		public NativeDisposeCoordinator DisposeCoordinator
		{
			get { return m_DisposeCoordinator; }
		}
	}
#else
    public sealed class PDFLibrary
    {
		public enum ErrorCode
		{
			ErrSuccess = 0,     // No error.
			ErrUnknown = 1,     // Unknown error.
			ErrFile = 2,        // File not found or could not be opened.
			ErrFormat = 3,      // File not in PDF format or corrupted.
			ErrPassword = 4,    // Password required or incorrect password.
			ErrSecurity = 5,    // Unsupported security scheme.
			ErrPage = 6         // Page not found or content error.
		}

		public static readonly Encoding Encoding = new UnicodeEncoding(false, false, false);

		public static readonly object nativeLock = typeof(NativeMethods);

		public const string PLUGIN_ASSEMBLY = "__Internal";

        private static PDFLibrary m_Instance;
        private static bool m_IsInitialized;
		private static bool m_AlreadyInitialized;

        private PDFLibrary()
        {
			if (!m_AlreadyInitialized)
            {
                NativeMethods.PDFJS_InitLibrary();

                m_AlreadyInitialized = true;
            }
		}

		public static ErrorCode GetLastError()
        {
			return ErrorCode.ErrSuccess;
        }

        public static PDFLibrary Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new PDFLibrary();

                return m_Instance;
            }
        }

        public void EnsureInitialized() { }

        public bool IsInitialized
        {
            get { return m_IsInitialized; }
            set { m_IsInitialized = value; }
        }
    }
#endif
}