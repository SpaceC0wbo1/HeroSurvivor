using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverPanel;

    public Button gameOverBtnRestart;

    public int scoreToWin = 10;

    private bool isGameActive = true;
    
    
    void Start()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(false);

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

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
