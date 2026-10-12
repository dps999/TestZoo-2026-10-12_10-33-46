using UnityEngine;

namespace DefaultNamespace
{
    public class Snake : Predators
    {
        [SerializeField, Min(0f)] private float speed = 2f;

        protected override void MoveAnimal()
        {
            Vector3 direction = moveDirection.normalized;
            Vector3 horizontalVelocity = direction * speed;
            body.linearVelocity = new Vector3(horizontalVelocity.x, body.linearVelocity.y, horizontalVelocity.z);
        }
    }
}
