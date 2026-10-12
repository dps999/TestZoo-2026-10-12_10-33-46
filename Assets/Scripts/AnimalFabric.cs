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
using System.Collections;

namespace DefaultNamespace
{
    public class AnimalFabric : MonoBehaviour
    {
        [SerializeField] private Animal animalPrefab;
        [SerializeField, Min(0.1f)] private float minimumSpawnInterval = 1f;
        [SerializeField, Min(0.1f)] private float maximumSpawnInterval = 2f;
        [SerializeField, Min(0f)] private float spawnHeight = 0.5f;

        private Camera spawnCamera;

        private void Awake()
        {
            spawnCamera = Camera.main;
            StartCoroutine(SpawnAnimals());
        }

        private IEnumerator SpawnAnimals()
        {
            while (true)
            {
                float interval = Random.Range(minimumSpawnInterval, maximumSpawnInterval);
                yield return new WaitForSeconds(interval);
                SpawnAnimal();
            }
        }

        private void SpawnAnimal()
        {
            Vector3 spawnPosition = GetSpawnPosition();
            if (animalPrefab != null)
            {
                Animal animal = Instantiate(animalPrefab, spawnPosition, animalPrefab.transform.rotation);
                animal.name = animalPrefab.name;
                return;
            }

            GameObject animalObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            animalObject.name = "Animal";
            animalObject.transform.position = spawnPosition;
            animalObject.transform.localScale = Vector3.one * 0.75f;
            animalObject.AddComponent<Animal>();
        }

        private Vector3 GetSpawnPosition()
        {
            if (spawnCamera == null)
                spawnCamera = Camera.main;

            if (spawnCamera != null)
            {
                Vector3 viewportPoint = new Vector3(Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), 0f);
                Ray ray = spawnCamera.ViewportPointToRay(viewportPoint);
                UnityEngine.Plane spawnPlane = new UnityEngine.Plane(Vector3.up, Vector3.up * spawnHeight);
                if (spawnPlane.Raycast(ray, out float hitDistance))
                    return ray.GetPoint(hitDistance);
            }

            return new Vector3(0f, spawnHeight, 0f);
        }
    }
}
