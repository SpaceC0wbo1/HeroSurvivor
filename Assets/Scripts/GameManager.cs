using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject winPanel;

    public Button winButtonRestart;
    public Button gameOverBtnRestart;

    public int scoreToWin = 10;

    private bool isGameActive = true;
    
    
    void Start()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);

        if (winButtonRestart != null)
        {
            winButtonRestart.onClick.AddListener(RestartGame);
        }

        if (gameOverBtnRestart != null)
        {
            gameOverBtnRestart.onClick.AddListener(RestartGame);
        }
    }

    private void OnEnable()
    {
        HeroController.OnHeroDied += GameOver;
    }

    private void OnDisable()
    {
        HeroController.OnHeroDied -= GameOver;
    }

    void Update()
    {
      
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
        isGameActive = false;
    }

    public void Victory()
    {
        Time.timeScale = 0f;
        winPanel.SetActive(true);
        isGameActive = false;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void CheckWinCondition(int currentScore)
    {
        if (currentScore >= scoreToWin && isGameActive)
        {
            Victory();
        }
    }
}
