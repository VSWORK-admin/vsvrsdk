﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Paroxe.PdfRenderer.Internal.Viewer
{
    public class PDFSearchPanel : UIBehaviour
    {
        [SerializeField]
        private RectTransform m_ContentPanel;
	    [SerializeField]
	    private InputField m_InputField;
        [SerializeField]
        private Image m_MatchCaseCheckBox;
        [SerializeField]
        private Image m_MatchWholeWordCheckBox;
        [SerializeField]
        private Text m_TotalResultText;
        [SerializeField]
        private Image m_ValidatorImage;

#if !UNITY_WEBGL
        private float m_AnimationDuration = 0.40f;
        private float m_AnimationPosition = 1.0f;
        private int m_CurrentSearchIndex = 0;
        private PDFSearchHandle.MatchOption m_Flags = PDFSearchHandle.MatchOption.NONE;
        private bool m_Opened;
        private PDFViewer m_Viewer;
        private string m_PreviousSearch;
        private PDFProgressiveSearch m_ProgressiveSearch;
        private bool m_SearchFinished;
        private int m_Total;
        private bool m_Registered;

        public void Close()
        {
            if (m_Opened)
            {
                m_Opened = false;

                m_AnimationPosition = 0.00f;

                if (m_ProgressiveSearch != null)
                    m_ProgressiveSearch.Abort();
                m_Viewer.SetSearchResults(null);

                try
                {
                    if (EventSystem.current.currentSelectedGameObject == m_InputField.gameObject)
                        EventSystem.current.SetSelectedGameObject(null, null);
                }
                catch { }
            }
        }

        public void OnCloseButton()
        {
            Close();
        }

        public void OnInputFieldEndEdit()
        {
            if (m_ProgressiveSearch != null)
            {
                if (m_InputField.text.Trim() != m_PreviousSearch)
                {
                    m_SearchFinished = false;

                    m_PreviousSearch = m_InputField.text.Trim();
                    m_ProgressiveSearch.StartSearch(m_InputField.text.Trim(), m_Flags);
                }
            }
        }

        public void OnMatchCaseClicked()
        {
            m_MatchCaseCheckBox.enabled = !m_MatchCaseCheckBox.enabled;

            if (m_MatchCaseCheckBox.enabled && m_MatchWholeWordCheckBox.enabled)
                m_Flags = PDFSearchHandle.MatchOption.MATCH_CASE_AND_WHOLE_WORD;
            else if (m_MatchCaseCheckBox.enabled && !m_MatchWholeWordCheckBox.enabled)
                m_Flags = PDFSearchHandle.MatchOption.MATCH_CASE;
            else if (!m_MatchCaseCheckBox.enabled && m_MatchWholeWordCheckBox.enabled)
                m_Flags = PDFSearchHandle.MatchOption.MATCH_WHOLE_WORD;
            else
                m_Flags = PDFSearchHandle.MatchOption.NONE;

            if (m_ProgressiveSearch != null)
            {
                m_SearchFinished = false;
                m_ProgressiveSearch.StartSearch(m_InputField.text.Trim(), m_Flags);
            }
        }

        public void OnMatchWholeWordCliked()
        {
            m_MatchWholeWordCheckBox.enabled = !m_MatchWholeWordCheckBox.enabled;

            if (m_MatchCaseCheckBox.enabled && m_MatchWholeWordCheckBox.enabled)
                m_Flags = PDFSearchHandle.MatchOption.MATCH_CASE_AND_WHOLE_WORD;
            else if (m_MatchCaseCheckBox.enabled && !m_MatchWholeWordCheckBox.enabled)
                m_Flags = PDFSearchHandle.MatchOption.MATCH_CASE;
            else if (!m_MatchCaseCheckBox.enabled && m_MatchWholeWordCheckBox.enabled)
                m_Flags = PDFSearchHandle.MatchOption.MATCH_WHOLE_WORD;
            else
                m_Flags = PDFSearchHandle.MatchOption.NONE;

            if (m_ProgressiveSearch != null)
            {
                m_SearchFinished = false;
                m_ProgressiveSearch.StartSearch(m_InputField.text.Trim(), m_Flags);
            }
        }

        public void OnNextButton()
        {
            m_Viewer.GoToNextSearchResult();
        }

        public void OnPreviousButton()
        {
            m_Viewer.GoToPreviousSearchResult();
        }

        public void Open()
        {
            if (!m_Opened && m_Viewer.IsLoaded)
            {
                m_Opened = true;

                m_AnimationPosition = 0.00f;

                m_PreviousSearch = "";
            }
        }

        public void Toggle()
        {
            if (m_Opened)
                Close();
            else
                Open();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            DestroyProgressiveSearch();

            m_Viewer.OnDocumentLoaded -= OnDocumentLoaded;
            m_Viewer.OnDocumentUnloaded -= OnDocumentUnloaded;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (m_Viewer == null)
                m_Viewer = GetComponentInParent<PDFViewer>();

            m_Viewer.OnDocumentLoaded += OnDocumentLoaded;
            m_Viewer.OnDocumentUnloaded += OnDocumentUnloaded;
        }

        private void OnDocumentLoaded(PDFViewer sender, PDFDocument document)
        {
            if (m_ProgressiveSearch == null)
                m_ProgressiveSearch = PDFProgressiveSearch.CreateSearch(m_Viewer.Document, sender.SearchTimeBudgetPerFrame);

            m_ProgressiveSearch.OnSearchFinished += OnSearchFinished;
            m_ProgressiveSearch.OnProgressChanged += OnProgressChanged;

            m_Registered = true;
        }

        private void OnDocumentUnloaded(PDFViewer sender, PDFDocument document)
        {
            DestroyProgressiveSearch();
        }

        private void DestroyProgressiveSearch()
        {
            if (m_ProgressiveSearch != null)
            {
                if (m_Registered)
                {
                    m_ProgressiveSearch.OnSearchFinished -= OnSearchFinished;
                    m_ProgressiveSearch.OnProgressChanged -= OnProgressChanged;

                    m_Registered = false;
                }

                Destroy(m_ProgressiveSearch.gameObject);
                m_ProgressiveSearch = null;
            }
        }

        private void OnProgressChanged(PDFProgressiveSearch sender, int total)
        {
            m_Total = total;

            UpdateSearchTotal();
        }

        private void OnSearchFinished(PDFProgressiveSearch sender, IList<PDFSearchResult>[] searchResults)
        {
            m_SearchFinished = true;

            m_Viewer.SetSearchResults(searchResults);
        }

        void Update()
        {
            if (!m_Viewer.IsLoaded)
            {
                Close();
            }

            if (m_Opened)
            {
                if (m_CurrentSearchIndex != m_Viewer.CurrentSearchResultIndex)
                {
                    m_CurrentSearchIndex = m_Viewer.CurrentSearchResultIndex;

                    UpdateSearchTotal();
                }
            }

            if (m_Opened && m_AnimationPosition < 1.0f)
            {
                m_AnimationPosition = Mathf.Clamp01(m_AnimationPosition + Time.deltaTime / m_AnimationDuration);

                m_ContentPanel.anchoredPosition = Vector2.Lerp(
                    new Vector2(m_ContentPanel.anchoredPosition.x, 135.0f),
                    new Vector2(m_ContentPanel.anchoredPosition.x, 0.0f),
                    PDFInternalUtils.CubicEaseIn(m_AnimationPosition, 0.0f, 1.0f, 1.0f));

                if (m_AnimationPosition == 1.0f)
                {
                    EventSystem.current.SetSelectedGameObject(m_InputField.gameObject, null);
                    m_InputField.OnPointerClick(new PointerEventData(EventSystem.current));

                    OnInputFieldEndEdit();
                }

            }
            else if (!m_Opened && m_AnimationPosition < 1.0f)
            {
                m_AnimationPosition = Mathf.Clamp01(m_AnimationPosition + Time.deltaTime / m_AnimationDuration);

                m_ContentPanel.anchoredPosition = Vector2.Lerp(
                    new Vector2(m_ContentPanel.anchoredPosition.x, 135.0f),
                    new Vector2(m_ContentPanel.anchoredPosition.x, 0.0f),
                    PDFInternalUtils.CubicEaseIn(1.0f - m_AnimationPosition, 0.0f, 1.0f, 1.0f));

                if (m_AnimationPosition == 1.0f)
                {
                    m_TotalResultText.text = "0 of 0";
                }
            }
        }

        private void UpdateSearchTotal()
        {
            if (m_Total > 0)
            {
                m_TotalResultText.text = (m_SearchFinished ? m_Viewer.CurrentSearchResultIndex + 1 : 1) + " of " + m_Total;
            }
            else
            {
                m_TotalResultText.text = "0 of 0";
            }

            if (string.IsNullOrEmpty(m_InputField.text.Trim()))
            {
                m_ValidatorImage.color = new Color(m_ValidatorImage.color.r, m_ValidatorImage.color.g,
                    m_ValidatorImage.color.b, 0.0f);
                m_TotalResultText.color = new Color(m_TotalResultText.color.r, m_TotalResultText.color.g,
                    m_TotalResultText.color.b, 0.0f);
            }
            else if (m_Total > 0)
            {
                m_ValidatorImage.color = new Color(m_ValidatorImage.color.r, m_ValidatorImage.color.g,
                    m_ValidatorImage.color.b, 0.0f);
                m_TotalResultText.color = new Color(m_TotalResultText.color.r, m_TotalResultText.color.g,
                    m_TotalResultText.color.b, 1.0f);
            }
            else
            {
                m_ValidatorImage.color = new Color(m_ValidatorImage.color.r, m_ValidatorImage.color.g,
                    m_ValidatorImage.color.b, m_SearchFinished ? 0.4f : 0.0f);
                m_TotalResultText.color = new Color(m_TotalResultText.color.r, m_TotalResultText.color.g,
                    m_TotalResultText.color.b, 1.0f);
            }

            RectTransform validatorTransform = (RectTransform)m_ValidatorImage.transform;
            validatorTransform.sizeDelta = new Vector2(m_TotalResultText.preferredWidth + 18.0f, validatorTransform.sizeDelta.y);
        }
#endif
    }
}