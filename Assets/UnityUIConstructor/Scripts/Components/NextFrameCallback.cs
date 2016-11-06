using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;
using UnityEngine;

namespace UnityUIConstructor {

    public class NextFrameCallback : UIBehaviour {

        Action callback;
        int count = 0;
        int skipFrames;

        public void SetCallback(Action callback, int skipFrames = 0) {
            if (callback == null)
                throw new Exception("Callback is null");
            this.callback = callback;
            this.skipFrames = skipFrames;
        }

        void Update() {
            count++;
            if (count > skipFrames) {
                Destroy(this);
                if (callback == null)
                    throw new Exception("Callback is null");
                else
                    callback.Invoke();
            }
        }

    }
}