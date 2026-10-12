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

using System.Collections;
using UnityEngine;
using VContainer;

namespace DefaultNamespace
{
    public sealed class AnimalSpawnSystem : MonoBehaviour
    {
        [SerializeField] private AnimalConfiguration animalConfiguration;

        [Inject] private IObjectResolver objectResolver;

        private Camera spawnCamera;
        private AnimalFactory animalFactory;

        private void Awake()
        {
            if (animalConfiguration == null)
            {
                Debug.LogError("AnimalConfiguration is not assigned.", this);
                enabled = false;
                return;
            }

            spawnCamera = Camera.main;
            animalFactory = new AnimalFactory(objectResolver);
            animalFactory.Initialize(
                animalConfiguration,
                transform,
                this);
            StartCoroutine(SpawnAnimals());
        }

        private void OnDestroy()
        {
            animalFactory?.Dispose();
        }

        private IEnumerator SpawnAnimals()
        {
            while (true)
            {
                float interval = Random.Range(
                    animalConfiguration.MinimumSpawnInterval,
                    animalConfiguration.MaximumSpawnInterval);
                yield return new WaitForSeconds(interval);
                SpawnAnimal();
            }
        }

        private void SpawnAnimal()
        {
            if (!animalFactory.TryCreateRandom(out Animal animal, out Quaternion rotation))
                return;

            animal.transform.SetParent(null, true);
            animal.transform.SetPositionAndRotation(GetSpawnPosition(), rotation);
        }

        private Vector3 GetSpawnPosition()
        {
            if (spawnCamera == null)
                spawnCamera = Camera.main;

            if (spawnCamera != null)
            {
                Vector3 viewportPoint = new Vector3(Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), 0f);
                Ray ray = spawnCamera.ViewportPointToRay(viewportPoint);
                UnityEngine.Plane spawnPlane = new UnityEngine.Plane(
                    Vector3.up,
                    Vector3.up * animalConfiguration.SpawnHeight);
                if (spawnPlane.Raycast(ray, out float hitDistance))
                    return ray.GetPoint(hitDistance);
            }

            return new Vector3(0f, animalConfiguration.SpawnHeight, 0f);
        }
    }
}
