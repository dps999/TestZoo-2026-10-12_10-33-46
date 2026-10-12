using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "JumpingMovement", menuName = "TestZoo/Movement/Jumping")]
    public sealed class JumpingMovementDefinition : MovementDefinition
    {
        [SerializeField, Min(0.1f)] private float jumpInterval = 2f;
        [SerializeField, Min(0f)] private float jumpDistance = 2f;
        [SerializeField, Min(0.1f)] private float jumpDuration = 0.5f;

        public override IAnimalMovementStrategy CreateStrategy()
        {
            return new JumpingMovementStrategy(jumpInterval, jumpDistance, jumpDuration);
        }
    }
}
