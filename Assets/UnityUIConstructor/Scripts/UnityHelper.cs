using System;
using UnityEngine;

namespace UnityUIConstructor {

    class UnityHelper {

        public static C RequireComponent<C>(GameObject obj) where C : Component {
            C component = obj.GetComponent<C>();

            if (component == null)
                throw new Exception("Could not get component " + typeof(C).Name + " from GameObject " + obj.name);

            return component;
        }

        public static C RequestComponent<C>(GameObject obj) where C : Component {
            C component = obj.GetComponent<C>();

            if (component == null)
                component = obj.AddComponent<C>();

            return component;
        }

        public static O NewObjectWith<O>(string name, Transform parent) where O : Component {
            GameObject obj = new GameObject(name);
            obj.transform.SetParent(parent);
            return obj.AddComponent<O>();
        }

        public static O NewObjectWith<O>(string name, Transform parent, Type[] otherComponents) where O : Component {
            GameObject obj = new GameObject(name, otherComponents);
            obj.transform.SetParent(parent);
            return obj.AddComponent<O>();
        }

    }

}