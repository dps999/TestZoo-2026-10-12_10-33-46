using UnityEngine;

namespace DefaultNamespace
{
    public abstract class MovementDefinition : ScriptableObject
    {
        // Each animal owns its strategy and its mutable state.
        public abstract IAnimalMovementStrategy CreateStrategy();
    }
}
