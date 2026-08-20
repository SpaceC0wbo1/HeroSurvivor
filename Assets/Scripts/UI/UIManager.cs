using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HeroSurvivor.Core;
using HeroSurvivor.Gameplay.Combat;

namespace HeroSurvivor.UI
{
    public class UIManager : MonoBehaviour
    {
        public TextMeshProUGUI scoreText;
        public Slider healthSlider;
        public GameObject fillerHealth;

        [SerializeField] private TMP_Text currentScoreText;
        [SerializeField] private TMP_Text highScoreText;

        private void OnEnable()
        {
            HealthHero.OnHealthChanged += UpdateHealth;
            ScoreManager.OnScoreChanged += UpdateCurrentScoreUI;
            ScoreManager.OnHighScoreChanged += UpdateHighScoreUI;

        }
        private void OnDisable()
        {
            HealthHero.OnHealthChanged -= UpdateHealth;
            ScoreManager.OnScoreChanged -= UpdateCurrentScoreUI;
            ScoreManager.OnHighScoreChanged -= UpdateHighScoreUI;
        }

        public void UpdateHealth(int currentHealth, int maxHealth)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;

            if (currentHealth <= 0)
            {
                fillerHealth.SetActive(false);
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


