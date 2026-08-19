using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor.SearchService;
using UnityEditor;

namespace HeroSurvivor.Core
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text highScoreText;
        [SerializeField] private SceneAsset gameSceneName;

        public GameObject howToPlayPanel;

        private const string HIGH_SCORE_KEY = "High Score";

        private void Start()
        {
            howToPlayPanel.SetActive(false);
            UpdateHighScoreUI();
        }

        public void PlayGame()
        {
            SceneManager.LoadScene(gameSceneName.name);
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
