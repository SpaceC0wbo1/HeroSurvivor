using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public Slider healthSlider;

    private int score = 0;
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }
    private void OnEnable()
    {
        BaseEnemy.OnEnemyDied += AddScore;
        HeroController.OnHealthChanged += UpdateHealth;
      
    }
    private void OnDisable()
    {
        BaseEnemy.OnEnemyDied -= AddScore;
        HeroController.OnHealthChanged -= UpdateHealth;
    }
    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = "Kills: " + score;
        gameManager.CheckWinCondition(score);
    }

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }
}
