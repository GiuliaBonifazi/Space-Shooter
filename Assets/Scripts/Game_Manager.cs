using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{
    bool _isGameOver = false;

    void Update()
    {
        if (Input.GetButton("Restart") && _isGameOver)
        {
            SceneManager.LoadScene(0);
        }
    }

    public bool IsGameOver()
    {
        return _isGameOver;
    }

    public void GameOver()
    {
        _isGameOver = true;
    }
}
