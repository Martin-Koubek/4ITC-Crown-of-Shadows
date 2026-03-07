using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameQuit : MonoBehaviour
{
    public void Start()
    {

    }

    IEnumerator LoadLevelAsync(string levelToLoad)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);
        yield return null;
    }

    public void LoadLevel(string levelToLoad)
    {
        Time.timeScale = 1.0f;
        StartCoroutine(LoadLevelAsync(levelToLoad));
    }
    public void QuitGame()
    {
        Debug.Log("1. Tlaèítko bylo fyzicky stisknuto!");
        Application.Quit();
    }

    [System.Obsolete]
    public void OpenMenu()
    {
        PlayerController playerController = GameObject.FindObjectOfType<PlayerController>();

        if (playerController.menOpen == false)
        {
            playerController.menOpen = true;
            Time.timeScale = 0f;
            playerController.pointer.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            playerController.menu.gameObject.SetActive(true);
        }
        else if (playerController.menOpen == true)
        {
            playerController.menOpen = false;
            Time.timeScale = 1f;
            playerController.pointer.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            playerController.menu.gameObject.SetActive(false);
        }

        else Debug.Log("Hráè nenalezen");
    }
}
