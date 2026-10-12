using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public enum AnimalMovementType
    {
        Wandering = 0,
        Straight = 1,
        Jumping = 2
    }

    public enum FeedingBehaviourType
    {
        Prey = 0,
        Predator = 1
    }

    [Serializable]
    public sealed class AnimalDefinition
    {
        [SerializeField] private Animal prefab;
        [SerializeField] private AnimalMovementType movementType;
        [SerializeField] private FeedingBehaviourType feedingBehaviourType;
        [SerializeField, Min(0f)] private float moveForce = 5f;
        [SerializeField, Min(0f)] private float maxSpeed = 2f;
        [SerializeField, Min(0.1f)] private float directionChangeInterval = 2f;
        [SerializeField, Min(0f)] private float speed = 2f;
        [SerializeField, Min(0.1f)] private float jumpInterval = 2f;
        [SerializeField, Min(0f)] private float jumpDistance = 2f;
        [SerializeField, Min(0.1f)] private float jumpDuration = 0.5f;

        public Animal Prefab => prefab;
        public AnimalMovementType MovementType => movementType;
        public FeedingBehaviourType FeedingBehaviourType => feedingBehaviourType;
        public float MoveForce => moveForce;
        public float MaxSpeed => maxSpeed;
        public float DirectionChangeInterval => directionChangeInterval;
        public float Speed => speed;
        public float JumpInterval => jumpInterval;
        public float JumpDistance => jumpDistance;
        public float JumpDuration => jumpDuration;
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
