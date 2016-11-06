using UnityEngine;
using UnityEngine.Assertions;

namespace UnityUIConstructor {

    public class ExampleInterfaceCoordinator2 : InterfaceCoordinator {

        public int NumberOfSlides = 3;

        public override void Setup() {

            Assert.IsTrue(NumberOfSlides > 0);

            for (int i = 1; i <= NumberOfSlides; i++) {
                CreateSlide<PlainSlide>("Slide" + i)
                    .Process(SlideConfig)
                    .Process(AddComponents);
            }

            SetStartingSlide("Slide1");

            for (int i = 1; i <= NumberOfSlides - 1; i++) {
                // Next transitions
                SetSlideTransition("Slide" + i, "Slide" + (i + 1), "Next");

                // First transitions
                SetSlideTransition("Slide" + (i + 1), "Slide1", "First");

            }

        }

        public void SlideConfig(InterfaceSlide slide) {
            slide
                .SetColour(0.9f)
                ;
        }

        public void AddComponents(InterfaceSlide slide) {
            slide.AddPanel<PlainPanel>("The Panel")
                .SetHorizonalLayout(Expand.None, new RectOffset(10, 10, 10, 10), 10)
                .SetToCoverParent()
                .AddControl<TextControl>(slide.SlideName)
                    .FitToText(5, 10)
                    .Return()
                .ProcessIf(slide.SlideName != "Slide1", (p) => AddTransitionButton(p, "First"))
                .ProcessIf(slide.SlideName != "Slide" + NumberOfSlides, (p) => AddTransitionButton(p, "Next"))
                ;
        }

        public void AddTransitionButton(InterfacePanel panel, string transition) {
            panel.AddPanel<ButtonPanel>(transition + " Button")
                .OnClick(() => panel.Coordinator.MakeSlideTransition(transition))
                .FitToText(5, 10)
                ;
        }

    }

}