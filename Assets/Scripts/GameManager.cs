using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Default manager
public class GameManager : MonoBehaviour {
    private int iScore;
    private int iLives;

    private const string HIGHSCORE_PREF = "HIGHSCORE";
    private int iHighScore;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI highScoreText;

    public GameObject restartButton;

    public static GameManager instance;

    private void Awake() {
        if (GameManager.instance == null) {
            GameManager.instance = this;
        } else if (GameManager.instance != this) {
            Destroy(GameManager.instance.gameObject);
            GameManager.instance = this;
        }
    }

    private void Start() {
        if (PlayerPrefs.HasKey(HIGHSCORE_PREF)) {
            iHighScore = GetiPref(HIGHSCORE_PREF);
        } else {
            iHighScore = 0;
        }
        iScore = 0;
        iLives = 3;
        UpdateScore();
        UpdateLives();
    }

    public void RestartScene() {
        iLives = 3;
        iScore = 0;
        UpdateScore();
        UpdateLives();
        EnemyManager.instance.StartSpawning();
        //SceneManager.LoadScene("Main");
    }

    public void GameOver() {
        //Check if iScore > Highscore if so update.
        if (iScore > iHighScore) {
            iHighScore = iScore;
            SetPref(HIGHSCORE_PREF, iHighScore);
        }
        highScoreText.text = ("High score: " + iHighScore);
        highScoreText.gameObject.SetActive(true);
        EnemyManager.instance.bSpawning = false;
        restartButton.SetActive(true);
    }

    public void PlayerDeath() {
        iLives--;
        if (iLives <= 0) {
            GameOver();
        }
        UpdateLives();
    }

    private void UpdateLives() {
        livesText.text = "Lives: " + iLives;
    }

    public void IncrementScore() {
        iScore++;
        UpdateScore();
    }

    private void UpdateScore() {
        scoreText.text = "Score: " + iScore;
    }

    private void SetPref(string _strKey, int _iValue) {
        PlayerPrefs.SetInt(_strKey, _iValue);
    }

    private int GetiPref(string _strKey, int _iDefault = 0) {
        return PlayerPrefs.GetInt(_strKey, Convert.ToInt32(_iDefault));
    }
}