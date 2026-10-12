using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace DefaultNamespace
{
    public sealed class AnimalFactory : IDisposable
    {
        private readonly IObjectResolver objectResolver;
        private readonly List<AnimalPoolEntry> animalPools = new List<AnimalPoolEntry>();
        private bool isInitialized;
        private bool isDisposed;

        public AnimalFactory(IObjectResolver objectResolver)
        {
            this.objectResolver = objectResolver;
        }

        public void Initialize(AnimalConfiguration configuration, Transform poolParent, UnityEngine.Object context)
        {
            if (isInitialized)
                throw new InvalidOperationException("AnimalFactory has already been initialized.");
            if (isDisposed)
                throw new ObjectDisposedException(nameof(AnimalFactory));

            isInitialized = true;

            foreach (AnimalDefinition definition in configuration.Animals)
            {
                if (definition == null || definition.Prefab == null)
                    continue;

                if (definition.Movement == null || definition.Feeding == null)
                {
                    Debug.LogError($"Animal '{definition.Name}' must have movement and feeding definitions assigned.", context);
                    continue;
                }

                var pool = new AnimalPool(
                    objectResolver,
                    definition.Prefab,
                    poolParent,
                    configuration.InitialPoolSize,
                    configuration.MaxPoolSize);
                string displayName = string.IsNullOrWhiteSpace(definition.Name)
                    ? pool.Prefab.name
                    : definition.Name;
                animalPools.Add(new AnimalPoolEntry(
                    pool,
                    definition,
                    displayName));
            }

            if (animalPools.Count == 0)
                Debug.LogError("No valid animal definitions are configured.", context);
        }

        public bool TryCreateRandom(out Animal animal, out Quaternion rotation)
        {
            if (isDisposed)
                throw new ObjectDisposedException(nameof(AnimalFactory));

            if (animalPools.Count == 0)
            {
                animal = null;
                rotation = Quaternion.identity;
                return false;
            }

            AnimalPoolEntry entry = animalPools[UnityEngine.Random.Range(0, animalPools.Count)];
            animal = entry.Pool.GetInactive();
            if (!animal.HasStrategies)
            {
                animal.ConfigureStrategies(
                    entry.Definition.Movement.CreateStrategy(),
                    entry.Definition.Feeding.CreateBehaviour());
                animal.ConfigureAppearance(entry.DisplayName, entry.Definition.Material);
            }
            entry.Pool.Activate(animal);
            rotation = entry.Pool.Prefab.transform.rotation;
            return true;
        }

        public void Dispose()
        {
            if (isDisposed)
                return;

            isDisposed = true;
            foreach (AnimalPoolEntry entry in animalPools)
                entry.Pool.Dispose();

            animalPools.Clear();
        }

        private sealed class AnimalPoolEntry
        {
            public AnimalPool Pool { get; }
            public AnimalDefinition Definition { get; }
            public string DisplayName { get; }

            public AnimalPoolEntry(
                AnimalPool pool,
                AnimalDefinition definition,
                string displayName)
            {
                Pool = pool;
                Definition = definition;
                DisplayName = displayName;
            }
        }
    }
}
