using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public Slider healthSlider;

    [SerializeField] private TMP_Text currentScoreText;
    [SerializeField] private TMP_Text highScoreText;

    void Start()
    {

    }
    private void OnEnable()
    {
        HeroController.OnHealthChanged += UpdateHealth;
        ScoreManager.OnScoreChanged += UpdateCurrentScoreUI;
        ScoreManager.OnHighScoreChanged += UpdateHighScoreUI;

    }
    private void OnDisable()
    {
        HeroController.OnHealthChanged -= UpdateHealth;
        ScoreManager.OnScoreChanged -= UpdateCurrentScoreUI;
        ScoreManager.OnHighScoreChanged -= UpdateHighScoreUI;
    }

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    private void UpdateCurrentScoreUI (int score)
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
