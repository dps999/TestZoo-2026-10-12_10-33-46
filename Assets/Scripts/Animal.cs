// // © 2015 - Present Candy Smith
// // All rights reserved
// // Redistribution of this software is strictly not allowed.
// // Copy of this software can be obtained from unity asset store only.
// // THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// // IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// // FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
// // AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// // LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// // OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// // THE SOFTWARE.

using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(Rigidbody))]
    public class Animal : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float moveForce = 5f;
        [SerializeField, Min(0f)] private float maxSpeed = 2f;
        [SerializeField, Min(0.1f)] private float directionChangeInterval = 2f;
        [SerializeField, Range(0f, 0.25f)] private float screenEdgeMargin = 0.05f;

        private Rigidbody body;
        private Camera movementCamera;
        private Vector3 moveDirection;
        private float directionChangeTimer;

        private void Awake()
        {
            body = GetComponent<Rigidbody>();
            movementCamera = Camera.main;
            ChooseDirection();
        }

        private void FixedUpdate()
        {
            directionChangeTimer -= Time.fixedDeltaTime;
            if (directionChangeTimer <= 0f)
                ChooseDirection();

            if (TryGetDirectionToVisibleArea(out Vector3 returnDirection))
                moveDirection = returnDirection;

            Vector3 horizontalVelocity = new Vector3(body.linearVelocity.x, 0f, body.linearVelocity.z);
            Vector3 desiredVelocity = moveDirection * maxSpeed;
            Vector3 steering = Vector3.ClampMagnitude(desiredVelocity - horizontalVelocity, moveForce);
            body.AddForce(steering, ForceMode.Acceleration);

            horizontalVelocity = new Vector3(body.linearVelocity.x, 0f, body.linearVelocity.z);
            if (horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
            {
                horizontalVelocity = horizontalVelocity.normalized * maxSpeed;
                body.linearVelocity = new Vector3(horizontalVelocity.x, body.linearVelocity.y, horizontalVelocity.z);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (this is not Predators)
                return;

            Animal targetAnimal = collision.gameObject.GetComponentInParent<Animal>();
            if (targetAnimal == null || targetAnimal == this)
                return;

            if (targetAnimal is Predators && GetInstanceID() > targetAnimal.GetInstanceID())
                return;

            Destroy(targetAnimal.gameObject);
        }

        private void ChooseDirection()
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            moveDirection = new Vector3(randomDirection.x, 0f, randomDirection.y);
            directionChangeTimer = directionChangeInterval * Random.Range(0.75f, 1.25f);
        }

        private bool TryGetDirectionToVisibleArea(out Vector3 direction)
        {
            direction = Vector3.zero;

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
