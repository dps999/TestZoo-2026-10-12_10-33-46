using System.Collections.Generic;
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
        private readonly List<PopupTimer> activePopups = new List<PopupTimer>(MaxPoolSize);

        private struct PopupTimer
        {
            public TextMeshPro Popup;
            public float RemainingTime;

            public PopupTimer(TextMeshPro popup, float remainingTime)
            {
                Popup = popup;
                RemainingTime = remainingTime;
            }
        }

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
            popup.transform.position = worldPosition + Vector3.up * 0.7f;
            activePopups.Add(new PopupTimer(popup, PopupLifetime));
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;

            for (int i = activePopups.Count - 1; i >= 0; i--)
            {
                PopupTimer timer = activePopups[i];
                timer.RemainingTime -= deltaTime;

                if (timer.RemainingTime <= 0f)
                {
                    activePopups.RemoveAt(i);
                    popupPool.Release(timer.Popup);
                }
                else
                {
                    activePopups[i] = timer;
                }
            }
        }
    }
}
