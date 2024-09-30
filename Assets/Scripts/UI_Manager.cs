using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _livesDisplay;
    [SerializeField]
    private Sprite[] _livesSprites;
    [SerializeField]
    private Text _gameOver_text;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private Game_Manager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOver_text.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScore(int score)
    {
        _scoreText.text = "Score: " + score;
    }

    public void UpdateLives(int currentLives)
    {
        _livesDisplay.sprite = _livesSprites[currentLives];
    }

    public void GameOver()
    {
        _gameManager.GameOver();
        _restartText.gameObject.SetActive(true);
        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        while (true) 
        {
            _gameOver_text.gameObject.SetActive(!_gameOver_text.gameObject.activeSelf);
            yield return new WaitForSeconds(.5f);
        }
    }
}
