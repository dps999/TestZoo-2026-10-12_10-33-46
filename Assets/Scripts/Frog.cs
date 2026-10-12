using UnityEngine;

namespace DefaultNamespace
{
    public class Frog : Prey
    {
        [SerializeField, Min(0.1f)] private float jumpInterval = 2f;
        [SerializeField, Min(0f)] private float jumpDistance = 2f;
        [SerializeField, Min(0.1f)] private float jumpDuration = 0.5f;

        private float jumpTimer;

        protected override void MoveAnimal()
        {
            jumpTimer -= Time.fixedDeltaTime;
            if (jumpTimer > 0f)
                return;

            if (TryGetDirectionToVisibleArea(out Vector3 returnDirection))
                moveDirection = returnDirection;
            else
                ChooseDirection();

            Vector3 horizontalVelocity = moveDirection * (jumpDistance / jumpDuration);
            float upwardVelocity = Mathf.Abs(Physics.gravity.y) * jumpDuration * 0.5f;
            body.linearVelocity = new Vector3(horizontalVelocity.x, upwardVelocity, horizontalVelocity.z);
            jumpTimer = jumpInterval;
        }
    }
}
