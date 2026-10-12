using UnityEngine;

namespace DefaultNamespace
{
    [DefaultExecutionOrder(100)]
    public sealed class AnimalVisibilityBounds : MonoBehaviour
    {
        [SerializeField, Range(0f, 0.25f)] private float screenEdgeMargin = 0.05f;

        private Animal animal;
        private Rigidbody body;
        private Camera movementCamera;

        private void Awake()
        {
            animal = GetComponent<Animal>();
            body = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (!TryGetDirectionToVisibleArea(out Vector3 returnDirection))
                return;

            animal.MoveDirection = returnDirection;

            Vector3 velocity = body.linearVelocity;
            Vector3 horizontalVelocity = returnDirection * new Vector2(velocity.x, velocity.z).magnitude;
            body.linearVelocity = new Vector3(horizontalVelocity.x, velocity.y, horizontalVelocity.z);
            body.MoveRotation(Quaternion.LookRotation(returnDirection, Vector3.up));
        }

        private bool TryGetDirectionToVisibleArea(out Vector3 direction)
        {
            direction = Vector3.zero;

            if (movementCamera == null)
                movementCamera = Camera.main;
            if (movementCamera == null)
                return false;

            Vector3 viewportPosition = movementCamera.WorldToViewportPoint(body.position);
            bool nearOrOutsideEdge = viewportPosition.z <= 0f
                || viewportPosition.x < screenEdgeMargin
                || viewportPosition.x > 1f - screenEdgeMargin
                || viewportPosition.y < screenEdgeMargin
                || viewportPosition.y > 1f - screenEdgeMargin;
            if (!nearOrOutsideEdge)
                return false;

            Ray centerRay = movementCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            UnityEngine.Plane movementPlane = new UnityEngine.Plane(Vector3.up, body.position);
            if (!movementPlane.Raycast(centerRay, out float hitDistance))
                return false;

            Vector3 target = centerRay.GetPoint(hitDistance);
            Vector3 toVisibleArea = target - body.position;
            toVisibleArea.y = 0f;
            if (toVisibleArea.sqrMagnitude < 0.0001f)
                return false;

            direction = toVisibleArea.normalized;
            return true;
        }
    }
}
