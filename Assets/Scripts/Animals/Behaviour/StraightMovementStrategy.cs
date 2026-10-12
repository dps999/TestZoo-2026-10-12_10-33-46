using UnityEngine;

namespace DefaultNamespace
{
    public sealed class StraightMovementStrategy : IAnimalMovementStrategy
    {
        private readonly float speed;

        public StraightMovementStrategy(float speed)
        {
            this.speed = speed;
        }

        public void OnActivated(Animal animal)
        {
        }

        public void Move(Animal animal, float _)
        {
            animal.TurnTowardVisibleArea();

            Rigidbody body = animal.MovementBody;
            Vector3 horizontalVelocity = animal.MoveDirection.normalized * speed;
            body.linearVelocity = new Vector3(horizontalVelocity.x, body.linearVelocity.y, horizontalVelocity.z);
        }
    }
}
