using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine;

namespace UnityUIConstructor {

    public class ResizeDetector : UIBehaviour {

        public UnityEvent OnChange;

        NextFrameCallback _NextFrameCallback;
        NextFrameCallback NextFrameCallback
        {
            get
            {
                if (_NextFrameCallback == null)
                    _NextFrameCallback = gameObject.AddComponent<NextFrameCallback>();

                return _NextFrameCallback;
            }
        }

        public ResizeDetector() {
            OnChange = new UnityEvent();
        }

        protected override void OnRectTransformDimensionsChange() {

            // This event is called far too often to call the event every time, so we only call once for every two frames but on the following frame. 
            // This seems to be enough to appear smooth, but if required, change the second parameter below to 0 make it once every frame
            if (_NextFrameCallback == null)
                NextFrameCallback.SetCallback(() => OnChange.Invoke(), 1);

        }

    }
}