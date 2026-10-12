using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text preyCounter;
        [SerializeField] private TMP_Text predatorCounter;

        private static int preyDeaths;
        private static int predatorDeaths;
        private static GameManager instance;

        private void Awake()
        {
            instance = this;
            preyDeaths = 0;
            predatorDeaths = 0;
            RefreshCounters();
        }

        public static void RegisterDeath(bool wasPredator)
        {
            if (wasPredator)
                predatorDeaths++;
            else
                preyDeaths++;

            if (instance != null)
                instance.RefreshCounters();
        }

        private void RefreshCounters()
        {
            if (preyCounter != null)
                preyCounter.text = $"Погибшие жертвы: {preyDeaths}";
            if (predatorCounter != null)
                predatorCounter.text = $"Погибшие хищники: {predatorDeaths}";
        }
    }
}