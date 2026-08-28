using HeroSurvivor.Gameplay.Player;
using HeroSurvivor.Gameplay.Enemies;
using HeroSurvivor.Gameplay.Health;
using HeroSurvivor.Gameplay.Combat;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;


namespace HeroSurvivor.Core
{
    public class GameManager : MonoBehaviour
    {
        public GameObject gameOverPanel;
        public Button gameOverBtnRestart;

        [SerializeField] private SceneAsset gameSceneName;
        [SerializeField] private EnemySpawner enemySpawner;
        [SerializeField] private float waveInterval = 10f;
        [SerializeField] private CursorManager cursorManager;

        private EnemiesWavesController _wavesController;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        void Start()
        {
            cursorManager.SetCombatCursor();
            _wavesController = new EnemiesWavesController (enemySpawner, this, waveInterval);
            _wavesController.StartWaves();

            if (gameOverPanel != null) gameOverPanel.SetActive(false);
        }

        private void OnEnable()
        {
            _signalBus?.Subscribe<HeroDiedSignal>(GameOver);
        }

        private void OnDisable()
        {
            _signalBus?.TryUnsubscribe<HeroDiedSignal>(GameOver);
        }

        private void OnDestroy()
        {
            _wavesController?.Dispose();
        }

        public void GameOver()
        {
            Time.timeScale = 0f;
            cursorManager.SetDefaultCursor();
            gameOverPanel.SetActive(true);
        }

        public void RestartGame()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void ReturnToMainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(gameSceneName.name);
        }
    }
}
