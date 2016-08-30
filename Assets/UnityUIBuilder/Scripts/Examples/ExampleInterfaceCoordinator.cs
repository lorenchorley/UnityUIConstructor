using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UnityUIConstructor { 

    public class ExampleInterfaceCoordinator : InterfaceCoordinator {

        public Sprite[] images;

        public override void Setup() {
        
            CreateSlide<FullScreenSlide>("The Slide")
                .SetVerticalLayout(Expand.Horizontal) // Booleans refer to whether or not to expand the child elements
                .AddPanel<SplitResizablePanel>("Main Panel")
                    .SetupSplitResizablePanel(false) // Boolean refers to whether the panel is split vertically or horizontally
                    .GetLeftOrTopPanel<CameraPanel>("Scene")
                        .SetCamera(GameObject.Find("Scene Camera").GetComponent<Camera>(), true)
                        .SetToFillParent()
                        .SetFlexibleWidth(20)
                        .Return<SplitResizablePanel>()
                    .GetRightOrBottomPanel<ScrollablePanel>("Images")
                        .SetVerticalScrolling(true, true, true)
                        .SetScrollingType(ScrollRect.MovementType.Clamped)
                        .GetScrollingPanel<PlainPanel>()
                            .SetGridLayout(TextAnchor.UpperCenter, new RectOffset(20, 10, 10, 10), 20, 20, new Vector2(100, 100)) // Figures refer to padding and spacing
                            .FitContentVertically()
                            .SetColour(0.4f) // A gray scale colour
                            .Process(FillImages) // Process another method on this particular panel
                            .Return<ScrollablePanel>()
                        .SetColour(0.4f)
                        .SetFlexibleWidth(20)
                        .ReturnPanel()
                    .SetHorizonalLayout(Expand.Both)
                    .SetFlexibleHeight(20)
                    .Return()
                .Process(AddBottomMenuBar)
                ;
        
            SetStartingSlide("The Slide");

        }

        // Can be used many times using the Process method, as long as it is on a panel
        public void AddButton(InterfacePanel menuPanel, string name, UnityAction action) {
            menuPanel.AddPanel<ButtonPanel>(name)
                .FitToText(5, 30)
                .OnClick(action)
                .GetText()
                    .SetTextColour(0.1f)
                    .Return()
                .SetColour(1)
                ;
        }

        public void FillImages(InterfacePanel panel) {
            foreach (Sprite t in images) {
                string name = t.name;
                panel
                    .AddControl<ImageButtonControl>(t.name)
                        .OnClick(() => Debug.Log("Clicked image: " + name))
                        .SetImage(t);
            }

        }
    
        // There are three ways to get a reference to an object
        public void AddBottomMenuBar(InterfaceSlide slide) {
            PlainPanel menuPanel_firstWay;
            slide
                .AddPanel<PlainPanel>("MenuBar", out menuPanel_firstWay)
                    .SetHorizonalLayout(Expand.None, new RectOffset(10, 10, 4, 4), 10)
                    .SetColour(0.2f)
                    .Process((menuPanel_secondWay) => AddButton(menuPanel_secondWay, "Close", () => menuPanel_secondWay.ParentSlide.SetVisibility(false)))
                    .Process((p) => 
                        AddSubMenu<PlainPanel>(menuPanel_firstWay, "Menu", SubMenuAlignment.TopRight, delegate (InterfacePanel panel) {
                            panel.AddPanel<MenuItemPanel>("First").OnClick(() => Debug.Log("First"));
                            panel.AddPanel<MenuItemPanel>("Second").OnClick(() => Debug.Log("Second"));
                        })
                    )
                    .Process((p) => {
                        InterfacePanel menuPanel_thirdWay = slide.GetPanel<PlainPanel>("MenuBar");
                        AddSubMenu<PlainPanel>(menuPanel_thirdWay, "Another menu", SubMenuAlignment.TopRight, delegate (InterfacePanel panel) {
                            panel.AddPanel<MenuItemPanel>("First").OnClick(() => Debug.Log("First"));
                            panel.AddPanel<MenuItemPanel>("Second").OnClick(() => Debug.Log("Second"));
                            panel.AddPanel<MenuItemPanel>("Third").OnClick(() => Debug.Log("Third"));
                        });
                    })
                    ;

        }

        public DropDownMenuItemPanel AddSubMenu<P>(InterfacePanel menuPanel, string name, SubMenuAlignment alignment, Action<InterfacePanel> setupCallback) where P : InterfacePanel {
            DropDownMenuItemPanel itemPanel;
            menuPanel.AddPanel<DropDownMenuItemPanel>(name, out itemPanel)
                .SetMenuItemName(name)
                .OpenOnClick()
                .GetDropDownPanel<P>()
                    .FitContentVertically()
                    .SetVerticalLayout(Expand.Horizontal, new RectOffset(1, 1, 1, 1), 0)
                    .SetMinWidth(itemPanel.TextControl.Text.preferredWidth)
                    .SetColour(0.5f)
                    .Process(setupCallback) // A callback to make this method even more flexible
                    .Return<DropDownMenuItemPanel>()
                .Position(alignment)
                .FitToText(5, 30)
                .SetTextColour(0.5f)
                .FitContentVertically()
                .SetColour(1)
                ;
        
            return itemPanel;
        }
    
    }

}

