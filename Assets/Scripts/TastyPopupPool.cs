using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class TastyPopupPool : MonoBehaviour
    {
        private const float PopupLifetime = 1.5f;
        private const int PrewarmCount = 4;
        private const int MaxPoolSize = 32;

        private static TastyPopupPool instance;

        private ObjectPool<RectTransform> popupPool;
        private RectTransform popupPrefab;
        private RectTransform canvasRect;

        public static void Show(RectTransform prefab, Vector3 worldPosition)
        {
            if (prefab == null)
                return;

            if (instance == null)
                CreatePool(prefab);

            instance.ShowPopup(worldPosition);
        }

        private static void CreatePool(RectTransform prefab)
        {
            GameObject poolObject = new GameObject("TastyPopupPool");
            instance = poolObject.AddComponent<TastyPopupPool>();
            instance.popupPrefab = prefab;

            GameObject canvasObject = new GameObject("TastyPopupCanvas");
            canvasObject.transform.SetParent(poolObject.transform, false);
            Canvas canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObject.AddComponent<CanvasScaler>();
            instance.canvasRect = canvas.GetComponent<RectTransform>();

            instance.popupPool = new ObjectPool<RectTransform>(
                instance.CreatePopup,
                instance.OnGetPopup,
                instance.OnReleasePopup,
                instance.OnDestroyPopup,
                collectionCheck: true,
                defaultCapacity: PrewarmCount,
                maxSize: MaxPoolSize);

            for (int i = 0; i < PrewarmCount; i++)
            {
                RectTransform popup = instance.popupPool.Get();
                instance.popupPool.Release(popup);
            }
        }

        private RectTransform CreatePopup()
        {
            RectTransform popup = Instantiate(popupPrefab, canvasRect, false);
            return popup;
        }

        private void OnGetPopup(RectTransform popup)
        {
            popup.gameObject.SetActive(true);
        }

        private void OnReleasePopup(RectTransform popup)
        {
            popup.gameObject.SetActive(false);
        }

        private void OnDestroyPopup(RectTransform popup)
        {
            Destroy(popup.gameObject);
        }

        private void ShowPopup(Vector3 worldPosition)
        {
            RectTransform popup = popupPool.Get();
            Camera mainCamera = Camera.main;
            Vector2 screenPosition = mainCamera != null
                ? (Vector2)mainCamera.WorldToScreenPoint(worldPosition)
                : new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
            screenPosition += Vector2.down * 40f;

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvasRect, screenPosition, null, out Vector2 localPosition))
            {
                popup.anchoredPosition = localPosition;
            }

            StartCoroutine(ReturnToPool(popup));
        }

        private IEnumerator ReturnToPool(RectTransform popup)
        {
            yield return new WaitForSeconds(PopupLifetime);
            popupPool.Release(popup);
        }
    }
}
