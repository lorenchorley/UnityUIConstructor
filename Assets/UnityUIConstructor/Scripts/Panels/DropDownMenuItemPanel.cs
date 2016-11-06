using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

namespace UnityUIConstructor {

    public class DropDownMenuItemPanel : ButtonPanel {

        public bool DropDownPanelIsOpen
        {
            get
            {
                return DropDownPanel.gameObject.activeSelf;
            }
        }
        private InterfacePanel DropDownPanel;

        protected override void Setup() {
            base.Setup();
        }

        public DropDownMenuItemPanel SetMenuItemName(string name) {
            TextControl.Text.text = name;
            return this;
        }

        public DropDownMenuItemPanel OpenOnClick() {
            OnClick(delegate {
                DropDownPanel.gameObject.SetActive(!DropDownPanelIsOpen);

                CloseOnMouseExit CloseOnMouseExit;
                if (DropDownPanelIsOpen) {
                    List<Transform> objects = new List<Transform>();
                    objects.Add(transform);
                    objects.Add(DropDownPanel.transform);
                    DropDownPanel.gameObject.AddComponent<CloseOnMouseExit>().Setup(DropDownPanel.gameObject, objects);
                } else {
                    CloseOnMouseExit = DropDownPanel.gameObject.GetComponent<CloseOnMouseExit>();
                    if (CloseOnMouseExit != null)
                        Destroy(CloseOnMouseExit);
                }
            });
            return this;
        }

        public DropDownMenuItemPanel OpenOnHover() {
            SetMouseHoverActions(() => OpenDropDownMenu(), () => CloseDropDownMenu(), TextControl.gameObject);
            return this;
        }

        public void OpenDropDownMenu() {
            DropDownPanel.gameObject.SetActive(true);
        }

        public void CloseDropDownMenu() {
            DropDownPanel.gameObject.SetActive(false);
        }

        public DropDownMenuItemPanel Position(SubMenuAlignment alignment) {
            DropDownPanel.LayoutElement.ignoreLayout = true;

            float anchorX = 0;
            float anchorY = 0;
            float pivotX = 0;
            float pivotY = 0;

            switch (alignment) {
            case SubMenuAlignment.TopLeft:
                anchorX = 1;
                anchorY = 1;
                pivotX = 1;
                pivotY = 0;
                break;
            case SubMenuAlignment.TopCenter:
                anchorX = 0.5f;
                anchorY = 1;
                pivotX = 0.5f;
                pivotY = 0;
                break;
            case SubMenuAlignment.TopRight:
                anchorX = 0;
                anchorY = 1;
                pivotX = 0;
                pivotY = 0;
                break;

            case SubMenuAlignment.LeftTop:
                anchorX = 0;
                anchorY = 0;
                pivotX = 1;
                pivotY = 0;
                break;
            case SubMenuAlignment.LeftCenter:
                anchorX = 0;
                anchorY = 0.5f;
                pivotX = 1;
                pivotY = 0.5f;
                break;
            case SubMenuAlignment.LeftBottom:
                anchorX = 0;
                anchorY = 1;
                pivotX = 1;
                pivotY = 1;
                break;

            case SubMenuAlignment.RightTop:
                anchorX = 1;
                anchorY = 0;
                pivotX = 0;
                pivotY = 0;
                break;
            case SubMenuAlignment.RightCenter:
                anchorX = 1;
                anchorY = 0.5f;
                pivotX = 0;
                pivotY = 0.5f;
                break;
            case SubMenuAlignment.RightBottom:
                anchorX = 1;
                anchorY = 1;
                pivotX = 0;
                pivotY = 1;
                break;

            case SubMenuAlignment.BottomLeft:
                anchorX = 1;
                anchorY = 0;
                pivotX = 1;
                pivotY = 1;
                break;
            case SubMenuAlignment.BottomCenter:
                anchorX = 0.5f;
                anchorY = 0;
                pivotX = 0.5f;
                pivotY = 1;
                break;
            case SubMenuAlignment.BottomRight:
                anchorX = 0;
                anchorY = 0;
                pivotX = 0;
                pivotY = 1;
                break;
            default:
                throw new Exception("Alignment not supported: " + alignment);
            }

            DropDownPanel.RectTransform.anchorMin = new Vector2(anchorX, anchorY);
            DropDownPanel.RectTransform.anchorMax = new Vector2(anchorX, anchorY);
            DropDownPanel.RectTransform.pivot = new Vector2(pivotX, pivotY);
            DropDownPanel.RectTransform.anchoredPosition = Vector2.zero;

            return this;
        }

        public P GetDropDownPanel<P>() where P : InterfacePanel {
            if (DropDownPanel == null)
                AddPanel<P>("DropDown");

            DropDownPanel = subpanels["DropDown"];
            DropDownPanel.gameObject.SetActive(false);
            //DropDownPanel.Image.raycastTarget = false;

            return (P) DropDownPanel;
        }

        public P GetDropDownPanel<P>(out P panel) where P : InterfacePanel {
            panel = GetDropDownPanel<P>();
            return panel;
        }

        public override InterfacePanel ClearContents() {
            DropDownPanel.ClearContents();
            return this;
        }

    }

}