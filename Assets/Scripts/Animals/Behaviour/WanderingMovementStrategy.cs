using UnityEngine;

namespace DefaultNamespace
{
    public sealed class WanderingMovementStrategy : IAnimalMovementStrategy
    {
        private readonly float moveForce;
        private readonly float maxSpeed;
        private readonly float directionChangeInterval;
        private float directionChangeTimer;

        public WanderingMovementStrategy(float moveForce, float maxSpeed, float directionChangeInterval)
        {
            this.moveForce = moveForce;
            this.maxSpeed = maxSpeed;
            this.directionChangeInterval = directionChangeInterval;
        }

        public void OnActivated(Animal animal)
        {
            ResetDirectionTimer();
        }

        public void Move(Animal animal, float deltaTime)
        {
            directionChangeTimer -= deltaTime;
            if (directionChangeTimer <= 0f)
            {
                animal.ChooseDirection();
                ResetDirectionTimer();
            }

            animal.TurnTowardVisibleArea();

            Rigidbody body = animal.MovementBody;
            Vector3 horizontalVelocity = new Vector3(body.linearVelocity.x, 0f, body.linearVelocity.z);
            Vector3 desiredVelocity = animal.MoveDirection * maxSpeed;
            Vector3 steering = Vector3.ClampMagnitude(desiredVelocity - horizontalVelocity, moveForce);
            body.AddForce(steering, ForceMode.Acceleration);

            horizontalVelocity = new Vector3(body.linearVelocity.x, 0f, body.linearVelocity.z);
            if (horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
            {
                horizontalVelocity = horizontalVelocity.normalized * maxSpeed;
                body.linearVelocity = new Vector3(horizontalVelocity.x, body.linearVelocity.y, horizontalVelocity.z);
            }
        }

        private void ResetDirectionTimer()
        {
            directionChangeTimer = directionChangeInterval * Random.Range(0.75f, 1.25f);
        }
    }
}
