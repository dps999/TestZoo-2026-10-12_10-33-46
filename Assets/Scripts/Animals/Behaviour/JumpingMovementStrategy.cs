using UnityEngine;

namespace DefaultNamespace
{
    public sealed class JumpingMovementStrategy : IAnimalMovementStrategy
    {
        private readonly float jumpInterval;
        private readonly float jumpDistance;
        private readonly float jumpDuration;
        private float jumpTimer;
        private float jumpTimeRemaining;
        private bool isJumping;

        public JumpingMovementStrategy(float jumpInterval, float jumpDistance, float jumpDuration)
        {
            this.jumpInterval = jumpInterval;
            this.jumpDistance = jumpDistance;
            this.jumpDuration = jumpDuration;
        }

        public void OnActivated(Animal animal)
        {
            jumpTimer = 0f;
            jumpTimeRemaining = 0f;
            isJumping = false;
        }

        public void Move(Animal animal, float deltaTime)
        {
            Rigidbody body = animal.MovementBody;

            if (isJumping)
            {
                if (animal.TurnTowardVisibleArea())
                {
                    Vector3 correctedHorizontalVelocity = animal.MoveDirection * (jumpDistance / jumpDuration);
                    Vector3 velocity = body.linearVelocity;
                    body.linearVelocity = new Vector3(
                        correctedHorizontalVelocity.x,
                        velocity.y,
                        correctedHorizontalVelocity.z);
                }

                jumpTimeRemaining -= deltaTime;
                if (jumpTimeRemaining <= 0f)
                {
                    StopHorizontalMovement(body);
                    isJumping = false;
                    jumpTimer = jumpInterval;
                }

                return;
            }

            StopHorizontalMovement(body);
            jumpTimer -= deltaTime;
            if (jumpTimer > 0f)
                return;

            animal.ChooseDirectionOrReturnToVisibleArea();

            Vector3 horizontalVelocity = animal.MoveDirection * (jumpDistance / jumpDuration);
            float upwardVelocity = Mathf.Abs(Physics.gravity.y) * jumpDuration * 0.5f;
            body.linearVelocity = new Vector3(horizontalVelocity.x, upwardVelocity, horizontalVelocity.z);
            jumpTimeRemaining = jumpDuration;
            isJumping = true;
        }

        private static void StopHorizontalMovement(Rigidbody body)
        {
            Vector3 velocity = body.linearVelocity;
            body.linearVelocity = new Vector3(0f, velocity.y, 0f);
        }
    }
}
