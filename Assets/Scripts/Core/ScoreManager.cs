using HeroSurvivor.Gameplay.Combat;
using HeroSurvivor.Gameplay.Health;
using System;
using UnityEngine;
using Zenject;


namespace HeroSurvivor.Core 
{
    public class ScoreManager : MonoBehaviour
    {
        private int currentScore = 0;
        private int highScore = 0;

        private const string HIGH_SCORE_KEY = "High Score";

        public static event Action<int> OnScoreChanged;
        public static event Action<int> OnHighScoreChanged;

        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        void Start()
        {
            LoadHighScore();
        }

        private void OnEnable()
        {
            _signalBus?.Subscribe<EnemyDiedSignal>(OnEnemyDied);
        }

        private void OnDisable()
        {
            _signalBus?.TryUnsubscribe<EnemyDiedSignal>(OnEnemyDied);
        }

        private void OnEnemyDied (EnemyDiedSignal signal)
        {
            AddScore(signal.RewardPoints);
        }

        private void LoadHighScore()
        {
            if (PlayerPrefs.HasKey(HIGH_SCORE_KEY))
            {
                highScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY);
                Debug.Log($"[ScoreManager] Record loaded: {highScore}");
            }
            else
            {
                highScore = 0;
                Debug.Log("[ScoreManager] No record found. First launch!");
            }

            OnHighScoreChanged?.Invoke(highScore);
        }

        public void AddScore(int amount)
        {
            currentScore += amount;
            OnScoreChanged?.Invoke(currentScore);

            if (currentScore > highScore)
            {
                highScore = currentScore;
                SaveHighScore();
                OnHighScoreChanged?.Invoke(highScore);
            }
        }

        private void SaveHighScore()
        {
            PlayerPrefs.SetInt(HIGH_SCORE_KEY, highScore);
            PlayerPrefs.Save();
            Debug.Log($"[ScoreManager] New record saved: {highScore}");
        }

        public void ResetHighScore()
        {
            PlayerPrefs.DeleteKey(HIGH_SCORE_KEY);
            highScore = 0;
            OnHighScoreChanged?.Invoke(highScore);
            Debug.Log("[ScoreManager] Record reset!");
        }
    }

}

