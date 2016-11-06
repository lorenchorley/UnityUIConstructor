using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Assertions;

namespace UnityUIConstructor {

    [RequireComponent(typeof(RectTransform))]
    [ExecuteInEditMode]
    public abstract class InterfaceCoordinator : MonoBehaviour {

        private struct Transition {
            public string from;
            public string to;
            public Transition(string from, string to) { this.from = from; this.to = to; }
        }

        public abstract void Setup();

        private Dictionary<string, InterfaceSlide> slides = new Dictionary<string, InterfaceSlide>();
        private Dictionary<string, List<string>> transitionsFromTo = new Dictionary<string, List<string>>();
        private Dictionary<string, List<string>> transitionsToFrom = new Dictionary<string, List<string>>();
        private Dictionary<string, List<Transition>> namedTransitions = new Dictionary<string, List<Transition>>();
        private string startingSlide = null;
        private InterfaceSlide currentSlide;

        [Tooltip("Builds skeleton interface in editor mode, but cannot make many runtime links.")]
        public bool BuildInEditor = false;
        public bool RemoveFromEditor = false;
        public bool InterfaceIsActive { get; private set; }
        public bool InterfaceIsEnabled { get; private set; }
        [HideInInspector]
        public bool InterfaceIsBuilt = false;
        [HideInInspector]
        public Canvas ParentCanvas;
        public LayerMask LayerMask;

        [Space]

        public bool startVisible = false;
        public bool IndicateTypesInHierarchy = false;

        [Space]

        public Font InterfaceFont;
        public Sprite ButtonSprite;
        public Image.Type ImageType;
        public Scrollbar HorizontalScrollbar;
        public Scrollbar VertcialScrollbar;
        public IntervalSlider IntervalSlider;
        public Angler Angler;

        [Space]

        public Texture2D ResizeCursor;
        public Vector2 hotSpot = Vector2.zero;

        [Space]

        private UnityEvent OnFinishGeneratingInterface;
        private bool FinishedGeneratingInterface = false;

        private UnityEvent OnInterfaceConfigured;
        private bool InterfaceConfigured = false;

        private RectTransform _RectTransform;
        public RectTransform RectTransform { get { if (_RectTransform == null) { _RectTransform = GetComponent<RectTransform>(); } return _RectTransform; } }

        void Start() {
            if (Application.isPlaying)
                BuildInterface();
        }

        void Update() {
            if (BuildInEditor) {
                BuildInEditor = false;
                BuildInterface();
            } else if (RemoveFromEditor) {
                RemoveFromEditor = false;
                InterfaceIsBuilt = false;

                for (int i = transform.childCount - 1; i >= 0; i--) {
                    DestroyImmediate(transform.GetChild(i).gameObject);
                }

                foreach (NextFrameCallback nfc in GetComponentsInChildren<NextFrameCallback>())
                    DestroyImmediate(nfc);

            }
        }

        private void BuildInterface() {
            if (InterfaceIsBuilt)
                return;
            InterfaceIsBuilt = true;
            InterfaceIsEnabled = true;
            InterfaceIsActive = false;

            if (OnFinishGeneratingInterface == null)
                OnFinishGeneratingInterface = new UnityEvent();
            if (OnInterfaceConfigured == null)
                OnInterfaceConfigured = new UnityEvent();

            ParentCanvas = GetComponentInParent<Canvas>();

            slides = new Dictionary<string, InterfaceSlide>();
            transitionsFromTo = new Dictionary<string, List<string>>();
            transitionsToFrom = new Dictionary<string, List<string>>();
            namedTransitions = new Dictionary<string, List<Transition>>();
            startingSlide = null;

            if (InterfaceFont == null)
                InterfaceFont = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;

            Setup();

            // Set starting slide
            if (startingSlide == null)
                throw new Exception("No starting slide or no slides set");
            if (!slides.TryGetValue(startingSlide, out currentSlide))
                throw new Exception("No such slide: " + startingSlide);

            // Inform the interface that it has been finished
            OnFinishGeneratingInterface.Invoke();
            FinishedGeneratingInterface = true;

            gameObject.AddComponent<NextFrameCallback>().SetCallback(delegate {

                // Inform the interface that it has been finished and that the components have had enough time to organise themselves
                OnInterfaceConfigured.Invoke();
                InterfaceConfigured = true;

                // Set visibility of slides
                foreach (InterfaceSlide slide in slides.Values)
                    slide.gameObject.SetActive(false);
                if (startVisible)
                    currentSlide.SetVisibility(true);
                InterfaceIsActive = startVisible;

            }, 1); // Allow one entire frame so that there's an entire Update cycle for interface elements to do their thing

        }

        public void RunAfterFinishedGeneratingInterface(UnityAction action) {
            if (FinishedGeneratingInterface) {
                action.Invoke();
            } else {
                OnFinishGeneratingInterface.AddListener(action);
            }
        }

        public void RunAfterInterfaceConfigured(UnityAction action) {
            if (InterfaceConfigured) {
                action.Invoke();
            } else {
                OnInterfaceConfigured.AddListener(action);
            }
        }

        public IC InstantiateControl<IC>(string name, Transform parent) where IC : InterfaceControl {
            return _InstantiateComponent<IC>((IndicateTypesInHierarchy ? "Control: " : "") + name, parent);
        }

        public IP InstantiatePanel<IP>(string name, Transform parent) where IP : InterfacePanel {
            return _InstantiateComponent<IP>((IndicateTypesInHierarchy ? "Panel: " : "") + name, parent);
        }

        public IS InstantiateSlide<IS>(string name, Transform parent) where IS : InterfaceSlide {
            return _InstantiateComponent<IS>((IndicateTypesInHierarchy ? "Slide: " : "") + name, parent);
        }

        private IE _InstantiateComponent<IE>(string name, Transform parent) where IE : InterfaceElement {
            
            GameObject obj = null;
            switch (typeof(IE).Name) { // TODO Replace this with a  Dictionary<Type, GameObject> PrefabComponents
            case "IntervalSliderPanel":
                obj = GameObject.Instantiate<GameObject>(IntervalSlider.gameObject);
                break;
            case "AnglerPanel":
                obj = GameObject.Instantiate<GameObject>(Angler.gameObject);
                break;
            default:
                obj = new GameObject(name);
                break;
            }

            obj.transform.SetParent(parent);
            return obj.AddComponent<IE>();
        }

        public InterfaceSlide CreateSlide<S>(string name, out S slide) where S : InterfaceSlide {
            slide = CreateSlide<S>(name);
            return slide;
        }

        public S CreateSlide<S>(string name) where S : InterfaceSlide {
            S slide = UnityHelper.NewObjectWith<S>((IndicateTypesInHierarchy ? "Slide: " : "") + name, transform);
            slide.Coordinator = this;
            slide.SlideName = name;
            slides.Add(name, slide);

            if (startingSlide == null)
                startingSlide = name;

            slide.SetupSlide();

            return slide;
        }

        public S GetSlide<S>(string name) where S : InterfaceSlide {
            InterfaceSlide slide;
            if (slides.TryGetValue(name, out slide)) {
                if (slide is S)
                    return slide as S;
                else
                    throw new Exception("Slide was not of type " + typeof(S));
            } else
                throw new Exception("No slide of name '" + name + "' was found");
        }

        public void SetStartingSlide(string name) {
            startingSlide = name;
        }

        public void SetSlideTransition(string from, string to, string transitionName = null) {
            if (!slides.ContainsKey(from))
                throw new Exception("From slide not found: " + from);

            if (!slides.ContainsKey(to))
                throw new Exception("To slide not found: " + to);

            List<string> toList;
            if (!transitionsFromTo.TryGetValue(from, out toList)) {
                toList = new List<string>();
                transitionsFromTo.Add(from, toList);
            }

            List<string> fromList;
            if (!transitionsToFrom.TryGetValue(to, out fromList)) {
                fromList = new List<string>();
                transitionsToFrom.Add(to, fromList);
            }

            if (toList.Contains(to))
                throw new Exception("Transition " + from + " -> " + to + " already exists");

            if (fromList.Contains(from))
                throw new Exception("Transition " + from + " -> " + to + " already exists");

            toList.Add(to);
            fromList.Add(from);

            if (transitionName != null) {
                List<Transition> transitions;
                if (!namedTransitions.TryGetValue(transitionName, out transitions)) {
                    transitions = new List<Transition>();
                    namedTransitions.Add(transitionName, transitions);
                }
                transitions.Add(new Transition(from, to));
            }

        }

        public void SetSlideTransitionDestinationAlwaysPossible(string to) {
            // TODO
        }

        public void MakeSlideTransition(string transitionName) {
            List<Transition> transitions;
            if (!namedTransitions.TryGetValue(transitionName, out transitions))
                throw new Exception("No such transition '" + transitionName + "'");

            string to = null;
            for (int i = 0; i < transitions.Count; i++) {
                if (transitions[i].from == currentSlide.SlideName) {
                    to = transitions[i].to;
                    break;
                }
            }

            if (to == null)
                throw new Exception("No such transition '" + transitionName + "' coming from " + currentSlide.SlideName);

            MakeTransition(to);
        }

        public void MakeSlideTransitionTo(string to) {
            List<string> fromList;
            if (!transitionsToFrom.TryGetValue(to, out fromList) || !fromList.Contains(currentSlide.SlideName))
                throw new Exception("Transition from " + currentSlide.SlideName + " to " + to + " not allowed");

            MakeTransition(to);
        }

        private void MakeTransition(string to) {
            currentSlide.SetVisibility(false);
            currentSlide = slides[to];
            currentSlide.SetVisibility(true);
        }

        public void OpenInterface() {
            if (!InterfaceIsEnabled)
                return;

            Assert.IsFalse(InterfaceIsActive);
            InterfaceIsActive = true;

            if (!slides.TryGetValue(startingSlide, out currentSlide))
                throw new Exception("No such slide: " + startingSlide);

            currentSlide.SetVisibility(true);

        }

        public void OpenInterface(string openNonStartingSlide) {
            if (!InterfaceIsEnabled)
                return;

            Assert.IsFalse(InterfaceIsActive);
            InterfaceIsActive = true;

            if (!slides.TryGetValue(openNonStartingSlide, out currentSlide))
                throw new Exception("No such slide: " + openNonStartingSlide);

            currentSlide.SetVisibility(true);
        }

        public void CloseInterface() {
            if (!InterfaceIsEnabled)
                return;

            Assert.IsTrue(InterfaceIsActive);
            InterfaceIsActive = false;

            currentSlide.SetVisibility(false);
        }

        public void ToggleInterface() {
            if (InterfaceIsActive)
                CloseInterface();
            else
                OpenInterface();
        }

        public void EnableInterface() {
            InterfaceIsEnabled = true;
        }

        public void DisableInterface() {
            if (InterfaceIsActive)
                CloseInterface();
            InterfaceIsEnabled = false;
        }

    }
}