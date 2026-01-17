using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ProjectX.CodeBase.Utils
{
    public static class ClickOverIU
    {
        public static bool IsOverUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return true;

            var eventData = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
            var raycastResults = new List<RaycastResult>();
            EventSystem.current?.RaycastAll(eventData, raycastResults);

            return raycastResults.Count > 0;
        }

        public static bool IsOverUI(List<string> ignoreList)
        {

            var eventData = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
            var raycastResults = new List<RaycastResult>();
            EventSystem.current?.RaycastAll(eventData, raycastResults);
            for (int i = 0; i < raycastResults.Count; i++)
            {
                if (i < 0)
                    break;
                var name = raycastResults[i].gameObject.name;
                if (ignoreList.Any(ignoreItem => name.Contains(ignoreItem)))
                {
                    raycastResults.RemoveAt(i);
                }
            }

            return raycastResults.Count > 0;
        }


        public static bool IsOverGameObject(GameObject gameObject)
        {
            var eventData = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
            var raycastResults = new List<RaycastResult>();
            EventSystem.current?.RaycastAll(eventData, raycastResults);
            var containsObject = raycastResults.Count != 0 && raycastResults.Any(x => x.gameObject == gameObject
                || IsChild(x.gameObject, gameObject.transform));
            return containsObject;
        }

        public static bool IsOverGameObjectOnly(GameObject gameObject)
        {
            var eventData = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
            var raycastResults = new List<RaycastResult>();
            EventSystem.current?.RaycastAll(eventData, raycastResults);
            var containsObject = raycastResults.Count != 0 && (raycastResults[0].gameObject == gameObject
                                                               || IsChild(raycastResults[0].gameObject,
                                                                   gameObject.transform));
            return containsObject;
        }

        private static bool IsChild(GameObject current, Transform parent)
        {
            var parentTransform = current.transform.parent;
            while (parentTransform)
            {
                if (parentTransform == parent)
                {
                    return true;
                }

                parentTransform = parentTransform.parent;
            }

            return false;
        }
    }
}