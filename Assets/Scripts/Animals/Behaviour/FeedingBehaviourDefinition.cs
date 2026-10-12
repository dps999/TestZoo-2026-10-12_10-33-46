using UnityEngine;

namespace DefaultNamespace
{
    public abstract class FeedingBehaviourDefinition : ScriptableObject
    {
        public abstract IFeedingBehaviour CreateBehaviour();
    }
}
