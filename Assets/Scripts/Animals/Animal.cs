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

using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    [RequireComponent(typeof(Rigidbody))]
    public class Animal : MonoBehaviour
    {
        [SerializeField, Range(0f, 0.25f)] private float screenEdgeMargin = 0.05f;

        private Rigidbody body;
        private Renderer cachedRenderer;
        private Camera movementCamera;
        private Vector3 moveDirection;
        private IAnimalMovementStrategy movementStrategy;
        private IFeedingBehaviour feedingBehaviour;
        private System.Action<Animal> returnToPool;
        private bool isDespawning;

        public bool IsPredator => feedingBehaviour != null && feedingBehaviour.IsPredator;

        internal bool HasStrategies => movementStrategy != null && feedingBehaviour != null;
        internal IFeedingBehaviour FeedingBehaviour => feedingBehaviour;

        private void Awake()
        {
            body = GetComponent<Rigidbody>();
            cachedRenderer = GetComponentInChildren<Renderer>(true);
        }

        private void OnEnable()
        {
            movementCamera = Camera.main;
            body.linearVelocity = Vector3.zero;
            body.angularVelocity = Vector3.zero;
            body.WakeUp();
            isDespawning = false;
            ChooseDirection();
            movementStrategy?.OnActivated(this);
        }

        internal void ConfigureStrategies(IAnimalMovementStrategy movement, IFeedingBehaviour feeding)
        {
            if (movement == null)
                throw new ArgumentNullException(nameof(movement));
            if (feeding == null)
                throw new ArgumentNullException(nameof(feeding));
            if (HasStrategies)
                throw new InvalidOperationException("Animal strategies have already been configured.");

            movementStrategy = movement;
            feedingBehaviour = feeding;
        }

        internal void ConfigureAppearance(string displayName, Material material)
        {
            name = displayName;
            if (material != null && cachedRenderer != null)
                cachedRenderer.sharedMaterial = material;
        }

        internal void SetReturnToPool(System.Action<Animal> callback)
        {
            returnToPool = callback;
        }

        public bool TryDespawn()
        {
            if (isDespawning || !gameObject.activeSelf)
                return false;

            isDespawning = true;
            if (returnToPool != null)
                returnToPool(this);
            else
                Destroy(gameObject);

            return true;
        }

        private void FixedUpdate()
        {
            movementStrategy?.Move(this, Time.fixedDeltaTime);
        }

        internal Rigidbody MovementBody => body;
        internal Vector3 MoveDirection
        {
            get => moveDirection;
            set => moveDirection = value;
        }
        internal void ChooseDirection()
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            moveDirection = new Vector3(randomDirection.x, 0f, randomDirection.y);
        }

        internal void ChooseDirectionOrReturnToVisibleArea()
        {
            if (TryGetDirectionToVisibleArea(out Vector3 returnDirection))
                moveDirection = returnDirection;
            else
                ChooseDirection();
        }

        internal bool TurnTowardVisibleArea()
        {
            if (TryGetDirectionToVisibleArea(out Vector3 returnDirection))
            {
                moveDirection = returnDirection;
                return true;
            }

            return false;
        }

        internal bool TryGetDirectionToVisibleArea(out Vector3 direction)
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
