using TMPro;
using UnityEngine;
using VContainer;

namespace DefaultNamespace
{
    [RequireComponent(typeof(Animal))]
    public sealed class AnimalCollisionHandler : MonoBehaviour
    {
        [SerializeField] private TextMeshPro tastyPopupPrefab;

        [Inject] private IGameEventBus gameEventBus;

        private Animal animal;

        private void Awake()
        {
            animal = GetComponent<Animal>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            Animal target = collision.gameObject.GetComponentInParent<Animal>();
            if (target == null || target == animal)
                return;

            if (!animal.FeedingBehaviour.TryFeed(animal, target))
                return;

            gameEventBus.Publish(new AnimalDiedEvent(target.IsPredator));
            TastyPopupPool.Show(tastyPopupPrefab, animal.transform.position);
        }
    }
}
