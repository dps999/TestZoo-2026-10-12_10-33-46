namespace DefaultNamespace
{
    public interface IFeedingBehaviour
    {
        bool IsPredator { get; }
        bool TryFeed(Animal animal, Animal target);
    }

    public sealed class PreyFeedingBehaviour : IFeedingBehaviour
    {
        public bool IsPredator => false;

        public bool TryFeed(Animal animal, Animal target)
        {
            return false;
        }
    }

    public sealed class PredatorFeedingBehaviour : IFeedingBehaviour
    {
        public bool IsPredator => true;

        public bool TryFeed(Animal predator, Animal target)
        {
            if (target == null || target == predator)
                return false;

            if (target.IsPredator && predator.GetInstanceID() > target.GetInstanceID())
                return false;

            return target.TryDespawn();
        }
    }
}
