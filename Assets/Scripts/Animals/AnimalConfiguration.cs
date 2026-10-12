using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    [Serializable]
    public sealed class AnimalDefinition
    {
        [SerializeField]
        private string name;

        [SerializeField] private Animal prefab;
        [SerializeField] private Material material;
        [SerializeField] private MovementDefinition movement;
        [SerializeField] private FeedingBehaviourDefinition feeding;

        public string Name => name;
        public Animal Prefab => prefab;
        public Material Material => material;
        public MovementDefinition Movement => movement;
        public FeedingBehaviourDefinition Feeding => feeding;
    }

    [CreateAssetMenu(fileName = "AnimalConfiguration", menuName = "TestZoo/Animal Configuration")]
    public sealed class AnimalConfiguration : ScriptableObject
    {
        [SerializeField] private AnimalDefinition[] animals = new AnimalDefinition[0];
        [SerializeField, Min(0.1f)] private float minimumSpawnInterval = 1f;
        [SerializeField, Min(0.1f)] private float maximumSpawnInterval = 2f;
        [SerializeField, Min(0f)] private float spawnHeight = 0.5f;
        [SerializeField, Min(0)] private int initialPoolSize = 4;
        [SerializeField, Min(1)] private int maxPoolSize = 64;

        public IReadOnlyList<AnimalDefinition> Animals => animals;
        public float MinimumSpawnInterval => minimumSpawnInterval;
        public float MaximumSpawnInterval => maximumSpawnInterval;
        public float SpawnHeight => spawnHeight;
        public int InitialPoolSize => initialPoolSize;
        public int MaxPoolSize => maxPoolSize;
    }
}
