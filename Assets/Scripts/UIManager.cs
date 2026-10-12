using TMPro;
using UnityEngine;
using VContainer;

namespace DefaultNamespace
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text preyCounter;
        [SerializeField] private TMP_Text predatorCounter;

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
            if (gameStatistics == null)
                return;

            if (preyCounter != null)
                preyCounter.text = $"Death prey: {gameStatistics.PreyDeaths}";
            if (predatorCounter != null)
                predatorCounter.text = $"Death predators: {gameStatistics.PredatorDeaths}";
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
