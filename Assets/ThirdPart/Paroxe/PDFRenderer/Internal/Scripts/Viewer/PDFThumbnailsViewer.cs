using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Paroxe.PdfRenderer.Internal.Viewer
{
	public class PDFThumbnailsViewer : UIBehaviour
    {
        [SerializeField]
        private PDFThumbnailItem m_ThumbnailItemPrefab;
        [SerializeField]
        private RectTransform m_ThumbnailsContainer;

        private PDFPageRange m_CurrentPageRange;
        private PDFThumbnailItem m_HighlightedItem;
        private RectTransform m_LeftPanel;
        private PDFPageTextureHolder[] m_PageTextureHolders;
        private PDFDocument m_Document;
        private PDFViewer m_Viewer;
        private RectTransform m_RectTransform;
        private List<PDFThumbnailItem> m_Thumbnails;
        private PDFViewerLeftPanel m_ViewerLeftPanel;
        private bool m_IsLoaded;
        private int m_UpdateFramesDelay;
        private HashSet<int> m_PageToResize = new HashSet<int>();
        private List<int> m_ResizedPages = new List<int>();

        private RectTransform RectTransform { get { return (RectTransform) transform; } }

        private void Cleanup()
        {
            m_IsLoaded = false;
            m_Document = null;
            m_HighlightedItem = null;

            if (m_PageTextureHolders != null)
            {
                foreach (PDFPageTextureHolder holder in m_PageTextureHolders)
                {
                    if (holder.Texture != null)
                    {
                        Texture2D tex = holder.Texture;
                        holder.Texture = null;

                        Destroy(tex);
                    }
                }
            }

            m_PageTextureHolders = null;

            bool isNotFirst = false;
            foreach (Transform child in m_ThumbnailItemPrefab.transform.parent)
            {
                if (isNotFirst)
                    Destroy(child.gameObject);
                else
                    isNotFirst = true;
            }

            m_ThumbnailItemPrefab.gameObject.SetActive(false);
        }

        private static bool Intersect(Rect box0, Rect box1)
        {
            if (box0.xMax < box1.xMin || box0.xMin > box1.xMax) return false;
            if (box0.yMax < box1.yMin || box0.yMin > box1.yMax) return false;
            return true;
        }

        private PDFPageRange GetVisiblePageRange()
        {
            PDFPageRange pageRange = new PDFPageRange();

            Rect viewportRect = new Rect(Vector2.zero, RectTransform.rect.size);
            viewportRect.center = new Vector2(0.0f, RectTransform.rect.size.y * 0.5f);

            int c = m_ThumbnailsContainer.childCount - 1;
            for (int i = 0; i < c; ++i)
            {
                RectTransform rt = (RectTransform)m_ThumbnailsContainer.GetChild(i + 1);

                Rect pageRect = new Rect(Vector2.zero, rt.rect.size);
                pageRect.center = -m_ThumbnailsContainer.anchoredPosition - rt.anchoredPosition + Vector2.up * rt.rect.size.y * 0.5f;
                
                if (Intersect(pageRect, viewportRect))
                {
                    if (pageRange.m_From == -1)
                    {
                        pageRange.m_From = i;
                    }
                    else
                    {
                        pageRange.m_To = i + 1;
                    }
                }
                else if (pageRange.m_From != -1)
                {
                    break;
                }
            }

            if (pageRange.m_From != -1 && pageRange.m_To == -1)
            {
                pageRange.m_To = pageRange.m_From + 1;
            }

            return pageRange;
        }

        public void OnDocumentUnloaded()
        {
            Cleanup();
        }

        public void OnDocumentLoaded(PDFDocument document)
        {
	        if (m_IsLoaded || !gameObject.activeInHierarchy) 
		        return;

	        m_Document = document;

	        int c = m_Document.GetPageCount();

	        m_PageTextureHolders = new PDFPageTextureHolder[c];
	        m_Thumbnails = new List<PDFThumbnailItem>();

	        int currentPage = m_Viewer.CurrentPageIndex;

	        m_ThumbnailItemPrefab.gameObject.SetActive(false);

	        for (int i = 0; i < c; ++i)
	        {
		        PDFThumbnailItem item = Instantiate(m_ThumbnailItemPrefab.gameObject).GetComponent<PDFThumbnailItem>();
		        item.transform.SetParent(m_ThumbnailItemPrefab.transform.parent, false);
		        item.gameObject.SetActive(true);

		        item.Highlighted.gameObject.SetActive(false);
		        item.PageIndexLabel.text = (i + 1).ToString();

		        m_Thumbnails.Add(item);

		        PDFPageTextureHolder textureHolder = new PDFPageTextureHolder
		        {
			        PageIndex = i, 
			        Page = item.PageThumbnailRawImage.gameObject, 
			        Viewer = m_Viewer
		        };

		        m_PageTextureHolders[i] = textureHolder;
	        }

	        if (currentPage >= 0 && currentPage < m_Viewer.Document.GetPageCount())
	        {
		        m_HighlightedItem = m_Thumbnails[currentPage];
		        m_HighlightedItem.Highlighted.gameObject.SetActive(true);
	        }

	        m_CurrentPageRange = new PDFPageRange();

	        m_UpdateFramesDelay = 2;
	        m_IsLoaded = true;
        }

        public void OnCurrentPageChanged(int newPageIndex)
        {
            if (!m_IsLoaded)
                return;

            if (m_HighlightedItem != null)
            {
                m_HighlightedItem.Highlighted.gameObject.SetActive(false);
            }

            if (newPageIndex >= 0)
            {
                m_HighlightedItem = m_Thumbnails[newPageIndex];
                m_HighlightedItem.Highlighted.gameObject.SetActive(true);
            }

            UpdateHighlightedItem();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if (m_IsLoaded)
            {
                Cleanup();
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            DoOnEnable();
        }

        public void DoOnEnable()
        {
            if (m_Viewer == null)
                m_Viewer = GetComponentInParent<PDFViewer>();
            if (m_RectTransform == null)
                m_RectTransform = (RectTransform)transform;
            if (m_LeftPanel == null)
                m_LeftPanel = (RectTransform)transform.parent;
            if (m_ViewerLeftPanel == null)
                m_ViewerLeftPanel = m_LeftPanel.GetComponent<PDFViewerLeftPanel>();
            if (m_CurrentPageRange == null)
                m_CurrentPageRange = new PDFPageRange();

            if (!m_IsLoaded
                && m_Viewer.Document != null
                && m_Viewer.Document.IsValid)
            {
                OnDocumentLoaded(m_Viewer.Document);
            }

            m_ThumbnailItemPrefab.gameObject.SetActive(false);
        }

        public void DoUpdate()
        {
            if (Math.Abs(m_RectTransform.sizeDelta.x - (m_LeftPanel.sizeDelta.x - 24.0f)) > 0.01f)
                m_RectTransform.sizeDelta = new Vector2(m_LeftPanel.sizeDelta.x - 24.0f, m_RectTransform.sizeDelta.y);

            if (!m_IsLoaded || !m_ViewerLeftPanel.IsOpened)
            {
                if (!m_ViewerLeftPanel.IsOpened)
                    m_UpdateFramesDelay = 2;

                return;
            }

            if (m_UpdateFramesDelay > 0)
            {
                --m_UpdateFramesDelay;

                return;
            }

            PDFPageRange pageRange = GetVisiblePageRange();

            if (pageRange != m_CurrentPageRange)
            {
	            int[] pagesToLoad = PDFPageRange.GetPagesToload(m_CurrentPageRange, pageRange);

#if UNITY_WEBGL && !UNITY_EDITOR
	            foreach (int pageIndex in pagesToLoad)
		            m_PageTextureHolders[pageIndex].Texture = null;
#endif

                PDFPageRange.UpdatePageAgainstRanges(m_CurrentPageRange, pageRange, m_Document, m_PageTextureHolders, null, 0.25f, null, m_Viewer.GetCachedNormalPageSizes());

                foreach (int pageIndex in pagesToLoad)
	                m_PageToResize.Add(pageIndex);

                foreach (int pageIndex in PDFPageRange.GetPagesToUnload(m_CurrentPageRange, pageRange))
	                m_PageToResize.Remove(pageIndex);

                m_CurrentPageRange = pageRange;
            }

            foreach (int pageIndex in m_PageToResize)
            {
	            Texture2D tex = m_PageTextureHolders[pageIndex].Texture;

	            if (tex != null)
	            {
		            m_Thumbnails[pageIndex].AspectRatioFitter.aspectRatio = tex.width / (float)tex.height;

		            m_ResizedPages.Add(pageIndex);
                }
            }

            foreach (int resizedPage in m_ResizedPages)
            {
	            m_PageToResize.Remove(resizedPage);
            }

            m_ResizedPages.Clear();
        }

        private void UpdateHighlightedItem()
        {
            if (m_HighlightedItem != null)
            {
                m_HighlightedItem.Highlighted.color = new Color(152.0f / 255.0f, 192.0f / 255.0f, 217.0f / 255.0f, 1.0f);
            }
            else if (m_HighlightedItem != null)
            {
                m_HighlightedItem.Highlighted.color = new Color(200.0f / 255.0f, 200.0f / 255.0f, 200.0f / 255.0f, 1.0f);
            }
        }
    }
}