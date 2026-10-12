namespace DefaultNamespace
{
    public interface IAnimalMovementStrategy
    {
        void OnActivated(Animal animal);
        void Move(Animal animal, float deltaTime);
    }
}
