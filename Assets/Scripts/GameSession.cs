using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int score = 0;
    [SerializeField] Text scoreText;
    [SerializeField] Image[] hearts;
    
    
    void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;

        if (numGameSessions > 1)
            Destroy(gameObject);
        else {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Update()
    {
        DisplayHud();
    }

    private void DisplayHud()
    {
        for (int i = 0; i < playerLives; i++) {
            hearts[i].enabled = true;
            if (playerLives < hearts.Length)
                hearts[playerLives].enabled = false;
        }
        scoreText.text = score.ToString();
    }

    public void AddToScore(int amount)
    {
        score += amount;
    }

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
            StartCoroutine(TakeLife());
        else
            ResetGameSession();
    }

    IEnumerator TakeLife()
    {
        playerLives--;
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(FindObjectOfType<LevelController>().GetCurrentSceneIndex());
    }

    public void ResetGameSession()
    {
        SceneManager.LoadScene("Main Menu");
        Destroy(gameObject);
    }
}
