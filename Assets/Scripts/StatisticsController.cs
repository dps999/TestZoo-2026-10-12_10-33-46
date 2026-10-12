using UnityEngine;
using VContainer;

namespace DefaultNamespace
{
    public sealed class StatisticsController : MonoBehaviour
    {
        [SerializeField] private StatisticsView view;

        private GameStatistics gameStatistics;
        private bool isSubscribed;

        [Inject]
        private void Construct(GameStatistics statistics)
        {
            gameStatistics = statistics;
            SubscribeToStatistics();
        }

        private void OnEnable()
        {
            SubscribeToStatistics();
        }

        private void OnDisable()
        {
            if (!isSubscribed)
                return;

            gameStatistics.Changed -= RefreshCounters;
            isSubscribed = false;
        }

        private void RefreshCounters()
        {
            if (gameStatistics == null || view == null)
                return;

            view.Render(gameStatistics.PreyDeaths, gameStatistics.PredatorDeaths);
        }

        private void SubscribeToStatistics()
        {
            if (!isActiveAndEnabled || gameStatistics == null || isSubscribed)
                return;

            gameStatistics.Changed += RefreshCounters;
            isSubscribed = true;
            RefreshCounters();
        }
    }
}
