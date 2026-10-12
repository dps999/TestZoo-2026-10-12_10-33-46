using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

namespace DefaultNamespace
{
    public class TastyPopupPool : MonoBehaviour
    {
        private const float PopupLifetime = 1.5f;
        private const int PrewarmCount = 4;
        private const int MaxPoolSize = 32;

        private static TastyPopupPool instance;

        private ObjectPool<TextMeshPro> popupPool;
        private TextMeshPro popupPrefab;

        public static void Show(TextMeshPro prefab, Vector3 worldPosition)
        {
            if (prefab == null)
                return;

            if (instance == null)
                CreatePool(prefab);

            instance.ShowPopup(worldPosition);
        }

        private static void CreatePool(TextMeshPro prefab)
        {
            GameObject poolObject = new GameObject("TastyPopupPool");
            instance = poolObject.AddComponent<TastyPopupPool>();
            instance.popupPrefab = prefab;

            instance.popupPool = new ObjectPool<TextMeshPro>(
                instance.CreatePopup,
                instance.OnGetPopup,
                instance.OnReleasePopup,
                instance.OnDestroyPopup,
                collectionCheck: true,
                defaultCapacity: PrewarmCount,
                maxSize: MaxPoolSize);

            for (int i = 0; i < PrewarmCount; i++)
            {
                TextMeshPro popup = instance.popupPool.Get();
                instance.popupPool.Release(popup);
            }
        }

        private TextMeshPro CreatePopup()
        {
            TextMeshPro popup = Instantiate(popupPrefab, transform, false);
            popup.gameObject.name = "TastyPopup";
            popup.text = "Tasty!";
            return popup;
        }

        private void OnGetPopup(TextMeshPro popup)
        {
            popup.gameObject.SetActive(true);
        }

        private void OnReleasePopup(TextMeshPro popup)
        {
            popup.gameObject.SetActive(false);
        }

        private void OnDestroyPopup(TextMeshPro popup)
        {
            Destroy(popup.gameObject);
        }

        private void ShowPopup(Vector3 worldPosition)
        {
            TextMeshPro popup = popupPool.Get();
            Camera mainCamera = Camera.main;
            popup.transform.position = worldPosition + Vector3.up * 0.7f;

            if (mainCamera != null)
            {
                popup.transform.rotation = Quaternion.LookRotation(
                    mainCamera.transform.position - popup.transform.position,
                    mainCamera.transform.up);
                popup.transform.position -= mainCamera.transform.up * 0.6f;
            }

            StartCoroutine(ReturnToPool(popup));
        }

        private IEnumerator ReturnToPool(TextMeshPro popup)
        {
            yield return new WaitForSeconds(PopupLifetime);
            popupPool.Release(popup);
        }
    }
}
