using UnityEngine;

namespace DefaultNamespace
{
    public sealed class StraightMovementStrategy : IAnimalMovementStrategy
    {
        private readonly float speed;
        private readonly float directionChangeInterval;
        private float directionChangeTimer;

        public StraightMovementStrategy(float speed, float directionChangeInterval)
        {
            this.speed = speed;
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

            Rigidbody body = animal.MovementBody;
            Vector3 horizontalVelocity = animal.MoveDirection.normalized * speed;
            body.linearVelocity = new Vector3(horizontalVelocity.x, body.linearVelocity.y, horizontalVelocity.z);
        }

        private void ResetDirectionTimer()
        {
            directionChangeTimer = directionChangeInterval * Random.Range(0.75f, 1.25f);
        }
    }
}
