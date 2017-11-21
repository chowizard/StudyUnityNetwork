using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Chowizard.UnityNetwork.Client.Core;
using Chowizard.UnityNetwork.Client.Network;

namespace Chowizard.UnityNetwork.Client.Ui
{
    public class UiSetLogWindow : UiSet
    {
        public RectTransform uiScrollView;
        public Button uiButtonOpenCloseToggle;

        public Scrollbar uiScrollbarHorizontal;
        public Scrollbar uiScrollbarVertical;
        public Text uiTextLogView;

        private bool isOpen;

        public void OnClickOpenCloseToggle()
        {
            if(isOpen)
            {
                isOpen = false;
                CloseLogView();
            }
            else
            {
                isOpen = true;
                OpenLogView();
            }
        }

        // Use this for initialization
        private void Start()
        {
            Debug.Assert(uiScrollView != null);
            Debug.Assert(uiButtonOpenCloseToggle != null);

            Debug.Assert(uiScrollbarHorizontal != null);
            Debug.Assert(uiScrollbarVertical != null);
            Debug.Assert(uiTextLogView != null);

            isOpen = true;
        }

        // Update is called once per frame
        private void Update()
        {
            UpdateLogText();
        }

        private void UpdateLogText()
        {
            if(uiTextLogView.text != NetworkManager.Instance.message)
                uiTextLogView.text = NetworkManager.Instance.message;

            uiScrollbarVertical.value = 1.0f;
        }

        private void OpenLogView()
        {
            if(uiScrollView.gameObject.activeSelf == false)
                uiScrollView.gameObject.SetActive(true);
        }

        private void CloseLogView()
        {
            if(uiScrollView.gameObject.activeSelf == true)
                uiScrollView.gameObject.SetActive(false);
        }
    }
}

