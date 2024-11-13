using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Paroxe.PdfRenderer.Internal.Viewer
{
	public class PDFBookmarksViewer : UIBehaviour
    {
        [SerializeField]
        private RectTransform m_BooksmarksContainer;
        [SerializeField]
        private PDFBookmarkListItem m_ItemPrefab;
        [SerializeField]
        private Image m_LastHighlightedImage;

#if !UNITY_WEBGL
        private CanvasGroup m_ContainerCanvasGroup;
        private PDFViewerLeftPanelScrollbar m_Scrollbar;
#pragma warning disable 414 // Remove unread private members
        private bool m_Initialized = false;
#pragma warning restore 414 // Remove unread private members
        private RectTransform m_LeftPanel;
        private bool m_Loaded = false;
        private PDFDocument m_Document;
        private PDFViewer m_Viewer;
        private RectTransform m_RectTransform;
        private List<RectTransform> m_TopLevelItems;
        private PDFBookmark m_RootBookmark;
        private bool m_IsEnableCalled;
        private List<PDFBookmarkListItem> m_Items = new List<PDFBookmarkListItem>();
#endif

        public Image LastHighlightedImage
	    {
		    get { return m_LastHighlightedImage; }
		    set { m_LastHighlightedImage = value; }
	    }

#if !UNITY_WEBGL
        public PDFBookmark RootBookmark
	    {
		    get { return m_RootBookmark; }
	    }

	    public PDFViewer Viewer
	    {
		    get { return m_Viewer; }
	    }
#endif

#if !UNITY_WEBGL
        public void RegisterItem(PDFBookmarkListItem item)
	    {
		    m_Items.Add(item);
	    }

        private void UpdateItemsPlacement()
        {
	        foreach (PDFBookmarkListItem item in m_Items)
	        {
		        if (item == null)
			        continue;

		        item.UpdateItem();
	        }
        }
#endif

        public void DoUpdate()
        {
#if !UNITY_WEBGL
	        if (m_RectTransform != null && m_LeftPanel != null &&
                Math.Abs(m_RectTransform.sizeDelta.x - (m_LeftPanel.sizeDelta.x - 24.0f)) > 0.01f)
            {
                m_RectTransform.sizeDelta = new Vector2(m_LeftPanel.sizeDelta.x - 24.0f, m_RectTransform.sizeDelta.y);
            }
#endif
        }

        private void Cleanup()
        {
#if !UNITY_WEBGL
            if (m_Loaded)
            {
                m_Loaded = false;
                m_Initialized = false;
                m_TopLevelItems = null;
                m_Document = null;
                m_RootBookmark = null;

                bool isNotFirst = false;
                foreach (Transform child in m_BooksmarksContainer.transform)
                {
                    if (isNotFirst)
                        Destroy(child.gameObject);
                    else
                        isNotFirst = true;
                }

                m_ItemPrefab.gameObject.SetActive(false);
                SetAlpha(0.0f);

                m_Items.Clear();
            }
#endif
        }

        public void OnDocumentLoaded(PDFDocument document)
        {
#if !UNITY_WEBGL
	        if (m_Loaded) 
		        return;

	        m_Loaded = true;
            m_Document = document;

            m_TopLevelItems = new List<RectTransform>();

            m_RectTransform = (RectTransform)transform;
            m_LeftPanel = (RectTransform)transform.parent;

            m_RootBookmark = m_Document.GetRootBookmark();

            if (m_RootBookmark == null || m_RootBookmark.ChildCount == 0) 
	            return;

            gameObject.SetActive(true);

            m_ItemPrefab.gameObject.SetActive(true);

            foreach (PDFBookmark child in m_RootBookmark.EnumerateChildrenBookmarks())
            {
	            PDFBookmarkListItem item = Instantiate(m_ItemPrefab.gameObject).GetComponent<PDFBookmarkListItem>();
	            RectTransform itemTransform = (RectTransform)item.transform;

	            itemTransform.SetParent(m_BooksmarksContainer, false);
	            itemTransform.localScale = Vector3.one;
	            itemTransform.anchorMin = new Vector2(0.0f, 1.0f);
	            itemTransform.anchorMax = new Vector2(0.0f, 1.0f);
	            itemTransform.offsetMin = Vector2.zero;
	            itemTransform.offsetMax = Vector2.zero;

	            m_TopLevelItems.Add(itemTransform);

	            item.Initialize(this, child, 0, false);
            }

            m_ItemPrefab.gameObject.SetActive(false);

            UpdateItemsPlacement();

            SetAlpha(0.0f);

            Viewer.StartCoroutine(DelayedShow(m_Document));
#endif
        }

#if !UNITY_WEBGL
        IEnumerator DelayedShow(PDFDocument document)
        {
	        for (int i = 0; i < 3; ++i)
	        {
                if (!m_Loaded || m_Document != document)
                    yield break;

                yield return null;
            }

	        if (m_Loaded && m_Document == document)
	        {
		        SetAlpha(1.0f);
            }
        }
#endif

        public void OnDocumentUnloaded()
        {
#if !UNITY_WEBGL
            Cleanup();
#endif
        }

#if !UNITY_WEBGL
        protected override void OnEnable()
        {
            base.OnEnable();

            DoOnEnable();
        }
#endif

#if !UNITY_WEBGL
        protected override void OnDisable()
	    {
            base.OnDisable();

            m_IsEnableCalled = false;
        }
#endif

        public void DoOnEnable()
        {
#if !UNITY_WEBGL
	        if (m_IsEnableCalled)
		        return;
	        m_IsEnableCalled = true;

	        if (m_RectTransform == null)
		        m_RectTransform = (RectTransform)transform;

            if (m_Viewer == null)
                m_Viewer = GetComponentInParent<PDFViewer>();
            
            if (m_ContainerCanvasGroup == null)
            {
	            m_ContainerCanvasGroup = m_BooksmarksContainer.GetComponent<CanvasGroup>();

	            if (m_ContainerCanvasGroup == null)
	            {
		            m_ContainerCanvasGroup = m_BooksmarksContainer.gameObject.AddComponent<CanvasGroup>();
	            }
            }

            if (m_Scrollbar == null)
            {
	            ScrollRect scrollRect = GetComponent<ScrollRect>();

	            if (scrollRect != null)
	            {
		            Scrollbar scrollbar = scrollRect.verticalScrollbar;

		            if (scrollbar != null)
		            {
			            m_Scrollbar = scrollbar.GetComponent<PDFViewerLeftPanelScrollbar>();
		            }
	            }
            }

            m_ItemPrefab.gameObject.SetActive(false);

            if (!m_Loaded && m_Viewer.Document != null && m_Viewer.Document.IsValid)
            {
	            OnDocumentLoaded(m_Viewer.Document);
            }
            else
            {
	            UpdateItemsPlacement();
            }
#endif
        }

#if !UNITY_WEBGL
        private void SetAlpha(float alpha)
        {
	        m_ContainerCanvasGroup.alpha = alpha;

            if (m_Scrollbar != null)
	            m_Scrollbar.Alpha = alpha;
        }
#endif
    }
}