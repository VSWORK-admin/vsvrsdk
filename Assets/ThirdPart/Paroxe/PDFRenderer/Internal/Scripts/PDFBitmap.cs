using System;

namespace Paroxe.PdfRenderer.Internal
{
#if !UNITY_WEBGL || UNITY_EDITOR
    public sealed class PDFBitmap : IDisposable
    {
	    public enum BitmapFormat
	    {
			Unknown = 0,
			Gray = 1,   // Gray scale bitmap, one byte per pixel.
			BGR = 2,    // 3 bytes per pixel, byte order: blue, green, red.
            BGRx = 3,   // 4 bytes per pixel, byte order: blue, green, red, unused.
            BGRA = 4    // 4 bytes per pixel, byte order: blue, green, red, alpha.
        }

	    private IntPtr m_NativePointer;
        private readonly int m_Width;
        private readonly int m_Height;
        private readonly bool m_UseAlphaChannel;

        public PDFBitmap(int width, int height, bool useAlphaChannel)
        {
	        m_Width = width;
            m_Height = height;
            m_UseAlphaChannel = useAlphaChannel;

            m_NativePointer = NativeMethods.FPDFBitmap_Create(m_Width, m_Height, useAlphaChannel);
        }

        ~PDFBitmap()
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

	        NativeMethods.FPDFBitmap_Destroy(m_NativePointer);

	        m_NativePointer = IntPtr.Zero;
        }

        public int Width
        {
            get { return m_Width; }
        }

        public int Height
        {
            get { return m_Height; }
        }

        public bool UseAlphaChannel
        {
            get { return m_UseAlphaChannel; }
        }

        public BitmapFormat Format
        {
	        get { return NativeMethods.FPDFBitmap_GetFormat(m_NativePointer); }
        }

        public IntPtr NativePointer
        {
            get { return m_NativePointer; }
        }

        public bool HasSameSize(PDFBitmap other)
        {
            return (m_Width == other.m_Width && m_Height == other.m_Height);
        }

        public bool HasSameSize(int width, int height)
        {
            return (m_Width == width && m_Height == height);
        }

        public void FillRect(int left, int top, int width, int height, int color)
        {
	        NativeMethods.FPDFBitmap_FillRect(m_NativePointer, left, top, width, height, color);
        }

        public IntPtr GetBuffer()
        {
            return NativeMethods.FPDFBitmap_GetBuffer(m_NativePointer);
        }

        public int GetStride()
        {
            return NativeMethods.FPDFBitmap_GetStride(m_NativePointer);
        }
    }
#endif
}