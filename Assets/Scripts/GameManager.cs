using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    private GameObject _startPanel, _scoreObject, _endPanel,_player;

    [SerializeField]
    private TMP_Text _scoreText, _endScoreText, _highScoreText;

    private int _score;
    private int _Score
    {
        get
        {
            return _score;
        }
        set
        {
            StartCoroutine(UpdateText(value, _scoreText));
            _score = value;
        }
    }

    private int _endScore;
    private int _EndScore
    {
        get
        {
            return _endScore;
        }
        set
        {
            StartCoroutine(UpdateText(value, _endScoreText));
            _endScore = value;
        }
    }

    private int _highScore;
    private int _HighScore
    {
        get
        {
            return _highScore;
        }
        set
        {
            StartCoroutine(UpdateText(value, _highScoreText));
            _highScore = value;
        }
    }

    IEnumerator UpdateText(int finalValue,TMP_Text currentText)
    {
        int currentValue = int.Parse(currentText.text);
        int diff = (int)((finalValue - currentValue) * 0.01f);
        while(currentValue < finalValue)
        {
            currentValue += (diff > 0 ? diff : Random.Range(0, 5));
            currentText.text = currentValue.ToString();
            yield return new WaitForSeconds(0.01f);
        }
        currentText.text = finalValue.ToString();
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        _startPanel.SetActive(true);
        _player.SetActive(false);
        _scoreObject.SetActive(false);
        _endPanel.SetActive(false);
        Camera.main.GetComponent<CameraFollow>()._canFollow = false;
        _Score = 0;
        _endScore= 0;
        _highScore = 0;
    }

    public void UpdateScore(int currentScore)
    {
        _Score = currentScore;
    }

    public void GameStart()
    {
        _startPanel.SetActive(false);
        _scoreObject.SetActive(true);
        _player.SetActive(true);
        Camera.main.GetComponent<CameraFollow>()._canFollow = true;
    }

    public void GameEnd()
    {
        Destroy(_player, 2f);
        Camera.main.GetComponent<CameraFollow>()._canFollow = false;
        _scoreObject.SetActive(false);
        _endPanel.SetActive(true);
        _EndScore = _Score;

        int highScore = PlayerPrefs.HasKey("HighScore") ? PlayerPrefs.GetInt("HighScore") : _Score;
        if(_Score > highScore)
        {
            highScore = _Score;
        }
        _HighScore = highScore;
    }

    public void GameQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void GameRestart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
