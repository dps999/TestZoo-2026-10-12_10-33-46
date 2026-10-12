using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "WanderingMovement", menuName = "TestZoo/Movement/Wandering")]
    public sealed class WanderingMovementDefinition : MovementDefinition
    {
        [SerializeField, Min(0f)] private float moveForce = 5f;
        [SerializeField, Min(0f)] private float maxSpeed = 2f;
        [SerializeField, Min(0.1f)] private float directionChangeInterval = 2f;

        public override IAnimalMovementStrategy CreateStrategy()
        {
            return new WanderingMovementStrategy(moveForce, maxSpeed, directionChangeInterval);
        }
    }
}
