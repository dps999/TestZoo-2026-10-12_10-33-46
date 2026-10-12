using System;

namespace DefaultNamespace
{
    public sealed class GameStatistics : IDisposable
    {
        private readonly IGameEventBus gameEventBus;

        public int PreyDeaths { get; private set; }
        public int PredatorDeaths { get; private set; }

        public event Action Changed;

        public GameStatistics(IGameEventBus gameEventBus)
        {
            this.gameEventBus = gameEventBus;
            this.gameEventBus.AnimalDied += HandleAnimalDied;
        }

        public void Dispose()
        {
            gameEventBus.AnimalDied -= HandleAnimalDied;
        }

        private void HandleAnimalDied(AnimalDiedEvent gameEvent)
        {
            if (gameEvent.WasPredator)
                PredatorDeaths++;
            else
                PreyDeaths++;

            Changed?.Invoke();
        }
    }
}
