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

                var pool = new AnimalPool(
                    objectResolver,
                    definition.Prefab,
                    poolParent,
                    configuration.InitialPoolSize,
                    configuration.MaxPoolSize);
                animalPools.Add(new AnimalPoolEntry(pool, definition));
            }

            if (animalPools.Count == 0)
                Debug.LogError("No valid animal definitions are configured.", context);
        }

        private static IAnimalMovementStrategy CreateMovementStrategy(AnimalDefinition definition)
        {
            switch (definition.MovementType)
            {
                case AnimalMovementType.Wandering:
                    return new WanderingMovementStrategy(
                        definition.MoveForce,
                        definition.MaxSpeed,
                        definition.DirectionChangeInterval);
                case AnimalMovementType.Straight:
                    return new StraightMovementStrategy(definition.Speed);
                case AnimalMovementType.Jumping:
                    return new JumpingMovementStrategy(
                        definition.JumpInterval,
                        definition.JumpDistance,
                        definition.JumpDuration);
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(definition.MovementType),
                        definition.MovementType,
                        "Unknown animal movement type.");
            }
        }

        private static IFeedingBehaviour CreateFeedingBehaviour(AnimalDefinition definition)
        {
            switch (definition.FeedingBehaviourType)
            {
                case FeedingBehaviourType.Prey:
                    return new PreyFeedingBehaviour();
                case FeedingBehaviourType.Predator:
                    return new PredatorFeedingBehaviour();
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(definition.FeedingBehaviourType),
                        definition.FeedingBehaviourType,
                        "Unknown animal feeding behaviour type.");
            }
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
                    CreateMovementStrategy(entry.Definition),
                    CreateFeedingBehaviour(entry.Definition));
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

            public AnimalPoolEntry(AnimalPool pool, AnimalDefinition definition)
            {
                Pool = pool;
                Definition = definition;
            }
        }
    }
}
