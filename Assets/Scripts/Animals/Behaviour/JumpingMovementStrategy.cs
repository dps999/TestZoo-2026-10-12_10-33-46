using UnityEngine;

namespace DefaultNamespace
{
    public sealed class JumpingMovementStrategy : IAnimalMovementStrategy
    {
        private readonly float jumpInterval;
        private readonly float jumpDistance;
        private readonly float jumpDuration;
        private float jumpTimer;

        public JumpingMovementStrategy(float jumpInterval, float jumpDistance, float jumpDuration)
        {
            this.jumpInterval = jumpInterval;
            this.jumpDistance = jumpDistance;
            this.jumpDuration = jumpDuration;
        }

        public void OnActivated(Animal animal)
        {
            jumpTimer = 0f;
        }

        public void Move(Animal animal, float deltaTime)
        {
            jumpTimer -= deltaTime;
            if (jumpTimer > 0f)
                return;

            animal.ChooseDirectionOrReturnToVisibleArea();

            Vector3 horizontalVelocity = animal.MoveDirection * (jumpDistance / jumpDuration);
            float upwardVelocity = Mathf.Abs(Physics.gravity.y) * jumpDuration * 0.5f;
            Rigidbody body = animal.MovementBody;
            body.linearVelocity = new Vector3(horizontalVelocity.x, upwardVelocity, horizontalVelocity.z);
            jumpTimer = jumpInterval;
        }
    }
}
