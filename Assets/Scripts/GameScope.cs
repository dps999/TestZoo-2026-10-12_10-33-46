using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace DefaultNamespace
{
    public sealed class GameScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<IGameEventBus, GameEventBus>(Lifetime.Singleton);
            builder.Register<GameStatistics>(Lifetime.Singleton);
            builder.RegisterComponentInHierarchy<Camera>();
            builder.RegisterComponentInHierarchy<UIManager>();
            builder.RegisterComponentInHierarchy<AnimalSpawnSystem>();
        }
    }
}
