namespace HeroSurvivor.Core
{
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using TMPro;

    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text highScoreText;
        [SerializeField] private string gameSceneName = "MainScene";

        public GameObject howToPlayPanel;

        private const string HIGH_SCORE_KEY = "High Score";

        private void Start()
        {
            howToPlayPanel.SetActive(false);
            UpdateHighScoreUI();
        }

        public void PlayGame()
        {
            SceneManager.LoadScene(gameSceneName);
        }

        public void QuitGame()
        {
            Debug.Log("The game is closed!");
            Application.Quit();
        }

        private void UpdateHighScoreUI()
        {
            int highScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);

            if (highScoreText != null)
            {
                highScoreText.text = $"High Score: {highScore}";
            }
        }

        public void ResetHighScore()
        {
            PlayerPrefs.DeleteKey(HIGH_SCORE_KEY);
            PlayerPrefs.Save();
            Debug.Log("[MainMenu] The high score record has been reset!");
            UpdateHighScoreUI();
        }

        public void HowToPlayShowPanel()
        {
            howToPlayPanel.SetActive(true);
        }

        public void HowToPlayHidePanel()
        {
            howToPlayPanel.SetActive(false);
        }
    }
}
