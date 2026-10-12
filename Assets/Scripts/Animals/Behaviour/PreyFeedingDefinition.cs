using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "PreyFeeding", menuName = "TestZoo/Feeding/Prey")]
    public sealed class PreyFeedingDefinition : FeedingBehaviourDefinition
    {
        public override IFeedingBehaviour CreateBehaviour()
        {
            return new PreyFeedingBehaviour();
        }
    }
}
