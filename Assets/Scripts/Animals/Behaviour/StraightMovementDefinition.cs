using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "StraightMovement", menuName = "TestZoo/Movement/Straight")]
    public sealed class StraightMovementDefinition : MovementDefinition
    {
        [SerializeField, Min(0f)] private float speed = 2f;
        [SerializeField, Min(0.1f)] private float directionChangeInterval = 2f;

        public override IAnimalMovementStrategy CreateStrategy()
        {
            return new StraightMovementStrategy(speed, directionChangeInterval);
        }
    }
}
