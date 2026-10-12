using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public sealed class StatisticsView : MonoBehaviour
    {
        [SerializeField] private TMP_Text preyCounter;
        [SerializeField] private TMP_Text predatorCounter;

        public void Render(int preyDeaths, int predatorDeaths)
        {
            if (preyCounter != null)
                preyCounter.text = $"Death prey: {preyDeaths}";

            if (predatorCounter != null)
                predatorCounter.text = $"Death predators: {predatorDeaths}";
        }
    }
}
