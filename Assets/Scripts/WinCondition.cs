using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCondition : MonoBehaviour
{
    [SerializeField] GameObject winCanvas;
    [SerializeField] float winScreenDelay = 2f;

    // Start is called before the first frame update
    void Start()
    {
        winCanvas.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(YouWinScreen());
    }

    IEnumerator YouWinScreen()
    {
        yield return new WaitForSeconds(winScreenDelay);
        winCanvas.SetActive(true);
    }

    public void LoadMainMenu()
    {
        FindObjectOfType<GameSession>().ResetGameSession();
    }
}
