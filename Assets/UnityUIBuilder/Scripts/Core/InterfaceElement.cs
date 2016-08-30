using UnityEngine;
using UnityEngine.UI;

namespace UnityUIConstructor {

    public abstract class InterfaceElement : MonoBehaviour {

        public InterfaceCoordinator Coordinator;

        private RectTransform _RectTransform;
        public RectTransform RectTransform
        {
            get
            {
                RequestRectTransform();
                return _RectTransform;
            }
        }
        public void RequestRectTransform() {
            if (_RectTransform == null) {
                _RectTransform = GetComponent<RectTransform>();
                if (_RectTransform == null)
                    _RectTransform = gameObject.AddComponent<RectTransform>();
            }
        }

        private ResizeDetector _ResizeDetector;
        public ResizeDetector ResizeDetector
        {
            get
            {
                RequestResizeDetector();
                return _ResizeDetector;
            }
        }
        public void RequestResizeDetector() {
            if (_ResizeDetector == null) {
                _ResizeDetector = GetComponent<ResizeDetector>();
                if (_ResizeDetector == null)
                    _ResizeDetector = gameObject.AddComponent<ResizeDetector>();
            }
        }

        private LayoutElement _LayoutElement;
        public LayoutElement LayoutElement
        {
            get
            {
                RequestLayoutElement();
                return _LayoutElement;
            }
        }
        public void RequestLayoutElement() {
            if (_LayoutElement == null) {
                _LayoutElement = GetComponent<LayoutElement>();
                if (_LayoutElement == null)
                    _LayoutElement = gameObject.AddComponent<LayoutElement>();
            }
        }

        private ContentSizeFitter _ContentSizeFitter;
        public ContentSizeFitter ContentSizeFitter
        {
            get
            {
                RequestContentSizeFitter();
                return _ContentSizeFitter;
            }
        }
        public void RequestContentSizeFitter() {
            if (_ContentSizeFitter == null) {
                _ContentSizeFitter = GetComponent<ContentSizeFitter>();
                if (_ContentSizeFitter == null)
                    _ContentSizeFitter = gameObject.AddComponent<ContentSizeFitter>();
            }
        }

        private HorizontalLayoutGroup _HorizontalLayoutGroup;
        public HorizontalLayoutGroup HorizontalLayoutGroup
        {
            get
            {
                RequestHorizontalLayoutGroup();
                return _HorizontalLayoutGroup;
            }
        }
        public void RequestHorizontalLayoutGroup() {
            if (_HorizontalLayoutGroup == null) {
                _HorizontalLayoutGroup = GetComponent<HorizontalLayoutGroup>();
                if (_HorizontalLayoutGroup == null)
                    _HorizontalLayoutGroup = gameObject.AddComponent<HorizontalLayoutGroup>();
            }
        }

        private VerticalLayoutGroup _VerticalLayoutGroup;
        public VerticalLayoutGroup VerticalLayoutGroup
        {
            get
            {
                RequestVerticalLayoutGroup();
                return _VerticalLayoutGroup;
            }
        }
        public void RequestVerticalLayoutGroup() {
            if (_VerticalLayoutGroup == null) {
                _VerticalLayoutGroup = GetComponent<VerticalLayoutGroup>();
                if (_VerticalLayoutGroup == null)
                    _VerticalLayoutGroup = gameObject.AddComponent<VerticalLayoutGroup>();
            }
        }

        private GridLayoutGroup _GridLayoutGroup;
        public GridLayoutGroup GridLayoutGroup
        {
            get
            {
                RequestGridLayoutGroup();
                return _GridLayoutGroup;
            }
        }
        public void RequestGridLayoutGroup() {
            if (_GridLayoutGroup == null) {
                _GridLayoutGroup = GetComponent<GridLayoutGroup>();
                if (_GridLayoutGroup == null)
                    _GridLayoutGroup = gameObject.AddComponent<GridLayoutGroup>();
            }
        }

        private Image _Image;
        public Image Image
        {
            get
            {
                RequestImage();
                return _Image;
            }
        }
        public void RequestImage() {
            if (_Image == null) {
                _Image = gameObject.GetComponent<Image>();
                if (_Image == null)
                    _Image = gameObject.AddComponent<Image>();
            }
        }

        private Text _Text;
        public Text Text
        {
            get
            {
                RequestText();
                return _Text;
            }
        }
        public void RequestText() {
            if (_Text == null) {
                _Text = gameObject.GetComponent<Text>();
                if (_Text == null)
                    _Text = gameObject.AddComponent<Text>();
            }
        }

        private Button _Button;
        public Button Button
        {
            get
            {
                RequestButton();
                return _Button;
            }
        }
        public void RequestButton() {
            if (_Button == null) {
                _Button = gameObject.GetComponent<Button>();
                if (_Button == null)
                    _Button = gameObject.AddComponent<Button>();
            }
        }

        public abstract ElementLayoutType GetLayoutType();
        protected abstract void Setup();

    }

}