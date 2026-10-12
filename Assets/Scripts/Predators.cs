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
    public class Predators : Animal
    {
        [SerializeField] private RectTransform tastyPopupPrefab;

        protected virtual void OnCollisionEnter(Collision collision)
        {
            Animal targetAnimal = collision.gameObject.GetComponentInParent<Animal>();
            if (targetAnimal == null || targetAnimal == this)
                return;

            if (targetAnimal is Predators && GetInstanceID() > targetAnimal.GetInstanceID())
                return;

            GameManager.RegisterDeath(targetAnimal is Predators);
            Destroy(targetAnimal.gameObject);
            TastyPopupPool.Show(tastyPopupPrefab, transform.position);
        }
    }
}