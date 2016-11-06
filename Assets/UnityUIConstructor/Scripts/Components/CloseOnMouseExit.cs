using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace UnityUIConstructor {

    public class CloseOnMouseExit : MonoBehaviour {

        List<Transform> objects;
        GameObject closeObject;

        int outFrameCount;

        public void Setup(GameObject closeObject, List<Transform> objects) {
            this.closeObject = closeObject;
            this.objects = objects;
            outFrameCount = 0;
        }

        void Update() {
            PointerEventData pointerData = new PointerEventData(EventSystem.current) {
                pointerId = -1,
            };

            pointerData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            foreach (RaycastResult result in results) {
                if (objects.Contains(result.gameObject.transform)) { 
                    outFrameCount = 0;
                    return;
                }
            }

            outFrameCount++;

            if (outFrameCount > 10) { 
                closeObject.SetActive(false);
                Destroy(this);
            }
        }

    }

}