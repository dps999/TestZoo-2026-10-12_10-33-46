using System;

namespace DefaultNamespace
{
    public readonly struct AnimalDiedEvent
    {
        public AnimalDiedEvent(bool wasPredator)
        {
            WasPredator = wasPredator;
        }

        public bool WasPredator { get; }
    }

    public interface IGameEventBus
    {
        event Action<AnimalDiedEvent> AnimalDied;
        void Publish(AnimalDiedEvent gameEvent);
    }

    public sealed class GameEventBus : IGameEventBus
    {
        public event Action<AnimalDiedEvent> AnimalDied;

        public void Publish(AnimalDiedEvent gameEvent)
        {
            AnimalDied?.Invoke(gameEvent);
        }
    }
}
