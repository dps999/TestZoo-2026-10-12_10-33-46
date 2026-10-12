using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using VContainer;
using VContainer.Unity;

namespace DefaultNamespace
{
    public sealed class AnimalPool : IDisposable
    {
        private readonly Transform parent;
        private readonly ObjectPool<Animal> pool;
        private readonly HashSet<Animal> animals = new HashSet<Animal>();
        private readonly HashSet<Animal> activeAnimals = new HashSet<Animal>();
        private readonly IObjectResolver objectResolver;
        private bool isDisposed;

        public Animal Prefab { get; }

        public AnimalPool(IObjectResolver objectResolver, Animal prefab, Transform parent, int initialSize, int maxSize)
        {
            this.objectResolver = objectResolver;
            Prefab = prefab;
            this.parent = parent;

            int poolCapacity = Mathf.Max(1, maxSize);
            int prewarmCount = Mathf.Clamp(initialSize, 0, poolCapacity);

            pool = new ObjectPool<Animal>(
                createFunc: CreateAnimal,
                actionOnGet: MarkAsRented,
                actionOnRelease: StoreAnimal,
                actionOnDestroy: DestroyAnimal,
                collectionCheck: true,
                defaultCapacity: prewarmCount,
                maxSize: poolCapacity);

            Prewarm(prewarmCount);
        }

        public Animal GetInactive()
        {
            if (isDisposed)
                throw new ObjectDisposedException(nameof(AnimalPool));

            return pool.Get();
        }

        public void Activate(Animal animal)
        {
            if (!activeAnimals.Contains(animal))
                throw new InvalidOperationException("Animal must be obtained from this pool before activation.");
            if (!animal.HasStrategies)
                throw new InvalidOperationException("Configure the animal's strategies before activation.");

            animal.gameObject.SetActive(true);
        }

        public void Dispose()
        {
            if (isDisposed)
                return;

            isDisposed = true;
            pool.Clear();

            Animal[] activeAnimalsCopy = new Animal[activeAnimals.Count];
            activeAnimals.CopyTo(activeAnimalsCopy);
            foreach (Animal animal in activeAnimalsCopy)
                DestroyAnimal(animal);
        }

        private Animal CreateAnimal()
        {
            Animal animal = objectResolver.Instantiate(Prefab, parent, false);
            animal.name = Prefab.name;
            animal.SetReturnToPool(ReturnAnimal);
            animal.gameObject.SetActive(false);
            animals.Add(animal);
            return animal;
        }

        private void MarkAsRented(Animal animal)
        {
            activeAnimals.Add(animal);
        }

        private void StoreAnimal(Animal animal)
        {
            activeAnimals.Remove(animal);
            animal.gameObject.SetActive(false);
            animal.transform.SetParent(parent, true);
        }

        private void DestroyAnimal(Animal animal)
        {
            activeAnimals.Remove(animal);
            animals.Remove(animal);

            if (animal != null)
                UnityEngine.Object.Destroy(animal.gameObject);
        }

        private void ReturnAnimal(Animal animal)
        {
            if (animal == null)
                return;

            if (isDisposed || !animals.Contains(animal))
            {
                DestroyAnimal(animal);
                return;
            }

            pool.Release(animal);
        }

        private void Prewarm(int count)
        {
            List<Animal> prewarmedAnimals = new List<Animal>(count);
            for (int i = 0; i < count; i++)
                prewarmedAnimals.Add(pool.Get());

            foreach (Animal animal in prewarmedAnimals)
                pool.Release(animal);
        }
    }
}
