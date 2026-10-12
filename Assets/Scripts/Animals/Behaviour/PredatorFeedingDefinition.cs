using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "PredatorFeeding", menuName = "TestZoo/Feeding/Predator")]
    public sealed class PredatorFeedingDefinition : FeedingBehaviourDefinition
    {
        public override IFeedingBehaviour CreateBehaviour()
        {
            return new PredatorFeedingBehaviour();
        }
    }
}
