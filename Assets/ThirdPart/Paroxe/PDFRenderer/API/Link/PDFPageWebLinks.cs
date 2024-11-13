using Paroxe.PdfRenderer.Internal;
using System;
using System.Text;
using UnityEngine;

namespace Paroxe.PdfRenderer
{
#if !UNITY_WEBGL
	public class PDFPageWebLinks : IDisposable, ICoordinatedNativeDisposable
	{
		private PDFTextPage m_TextPage;
		private IntPtr m_NativePointer;
		private int? m_LinksCount;
		private string m_Url;

		public IntPtr NativePointer
		{
			get { return m_NativePointer; }
		}

		public PDFPageWebLinks(PDFTextPage textPage)
		{
			if (textPage == null)
				throw new ArgumentNullException("textPage");

			m_TextPage = textPage;

			PDFLibrary.Instance.DisposeCoordinator.EnsureNativeLibraryIsInitialized();

			lock (PDFLibrary.nativeLock)
			{
				m_NativePointer = NativeMethods.FPDFLink_LoadWebLinks(((ICoordinatedNativeDisposable)m_TextPage).NativePointer);

				if (m_NativePointer != IntPtr.Zero)
					PDFLibrary.Instance.DisposeCoordinator.AddReference(this);
			}
		}

		public int LinksCount
		{
			get
			{
				if (m_LinksCount.HasValue)
					return m_LinksCount.Value;

				m_LinksCount = NativeMethods.FPDFLink_CountWebLinks(m_NativePointer);

				return m_LinksCount.Value;
			}
		}

		public int CountLinkRects(int linkIndex)
		{
			return NativeMethods.FPDFLink_CountRects(m_NativePointer, linkIndex);
		}

		public Rect GetLinkRect(int linkIndex, int rectIndex)
		{
			double left;
			double right;
			double bottom;
			double top;

			if (NativeMethods.FPDFLink_GetRect(m_NativePointer, linkIndex, rectIndex, out left, out top, out right, out bottom))
			{
				return new Rect((float)left, (float)top, Mathf.Abs((float)right - (float)left), Mathf.Abs((float)bottom - (float)top));
			}
			else
			{
				throw new IndexOutOfRangeException();
			}
		}

		public bool GetLinkTextRange(int linkIndex, out int startCharIndex, out int charCount)
		{
			return NativeMethods.FPDFLink_GetTextRange(m_NativePointer, linkIndex, out startCharIndex, out charCount);
		}

		public string GetLinkUrl(int linkIndex)
		{
			if (m_Url == null)
			{
				int length = NativeMethods.FPDFLink_GetURL(m_NativePointer, linkIndex, null, 0) * 2;

				if (length > 0)
				{
					byte[] buffer = new byte[length];

					NativeMethods.FPDFLink_GetURL(m_NativePointer, linkIndex, buffer, (uint)buffer.Length);

					m_Url = Encoding.Unicode.GetString(buffer);
				}

				if (m_Url == null)
					m_Url = string.Empty;
			}

			return m_Url;
		}

		~PDFPageWebLinks()
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

		ICoordinatedNativeDisposable ICoordinatedNativeDisposable.NativeParent
		{
			get { return m_TextPage; }
		}

		Action<IntPtr> ICoordinatedNativeDisposable.GetDisposeMethod()
		{
			return NativeMethods.FPDFLink_CloseWebLinks;
		}
	}
#endif
}

