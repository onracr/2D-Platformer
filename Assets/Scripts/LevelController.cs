using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2f;
    int currentScene;

    void Start()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
    }

    public int GetCurrentSceneIndex() { return currentScene; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(LevelLoader());
    }

    IEnumerator LevelLoader()
    {
        yield return new WaitForSeconds(levelLoadDelay);
        SceneManager.LoadScene(currentScene++);
    }

    public void LoadStartScene()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
