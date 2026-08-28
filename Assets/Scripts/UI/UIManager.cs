using HeroSurvivor.Gameplay.Combat;
using HeroSurvivor.Gameplay.Health;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HeroSurvivor.Core;
using Zenject;


namespace HeroSurvivor.UI
{
    public class UIManager : MonoBehaviour
    {
        public TextMeshProUGUI scoreText;
        public Slider healthSlider;
        public GameObject fillerHealth;

        [SerializeField] private TMP_Text currentScoreText;
        [SerializeField] private TMP_Text highScoreText;

        private SignalBus _signalBus;

        [Inject]
        public void Construct (SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            _signalBus?.Subscribe<HeroHealthChangedSignal>(OnHeroHealthChanged);

            ScoreManager.OnScoreChanged += UpdateCurrentScoreUI;
            ScoreManager.OnHighScoreChanged += UpdateHighScoreUI;

        }
        private void OnDisable()
        {
            _signalBus?.TryUnsubscribe<HeroHealthChangedSignal>(OnHeroHealthChanged);

            ScoreManager.OnScoreChanged -= UpdateCurrentScoreUI;
            ScoreManager.OnHighScoreChanged -= UpdateHighScoreUI;
        }

        private void OnHeroHealthChanged (HeroHealthChangedSignal signal)
        {
            UpdateHealth(signal.CurrentHealth, signal.MaxHealth);
        }

        public void UpdateHealth(int currentHealth, int maxHealth)
        {
            if (healthSlider != null)
            {
                healthSlider.maxValue = maxHealth;
                healthSlider.value = currentHealth;
            }

            if (fillerHealth != null)
            {
                fillerHealth.SetActive(currentHealth > 0);
            }
        }

        private void UpdateCurrentScoreUI(int score)
        {
            if (scoreText != null)
            {
                scoreText.text = "Kills: " + score;
            }

            if (currentScoreText != null)
            {
                currentScoreText.text = $"Score: {score}";
            }
        }

        private void UpdateHighScoreUI(int newHighScore)
        {
            if (highScoreText != null)
            {
                highScoreText.text = $"High Score: {newHighScore}";
            }
        }
    }
}


