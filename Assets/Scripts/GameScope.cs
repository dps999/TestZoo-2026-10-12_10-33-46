using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace DefaultNamespace
{
    public sealed class GameScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<Camera>();
            builder.RegisterComponentInHierarchy<GameManager>();
            builder.RegisterComponentInHierarchy<AnimalFabric>();
        }
    }
}
