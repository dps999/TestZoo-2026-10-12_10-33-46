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
    [RequireComponent(typeof(Rigidbody), typeof(AnimalVisibilityBounds))]
    public class Animal : MonoBehaviour
    {
        private Rigidbody body;
        private Renderer cachedRenderer;
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
            body.linearVelocity = Vector3.zero;
            body.angularVelocity = Vector3.zero;
            body.WakeUp();
            isDespawning = false;
            ChooseDirection();
            movementStrategy?.OnActivated(this);
        }

        internal void ConfigureStrategies(IAnimalMovementStrategy movement, IFeedingBehaviour feeding)
        {
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

    }
}
