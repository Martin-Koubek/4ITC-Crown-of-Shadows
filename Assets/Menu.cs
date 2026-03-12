using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject LoadingScreen;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject Controls;

    private bool controlsOpen;


    public int PlayedTutorial = 0;

    [SerializeField] private Image loadingBar;

    IEnumerator LoadLevelAsync(string levelToLoad)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);

        while (!loadOperation.isDone)
        {
            float progresValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadingBar.fillAmount = progresValue;
            yield return null;
        }
    }

    private void Start()
    {
        PlayedTutorial = PlayerPrefs.GetInt("playedTut");
    }

    public void LoadLevel(string levelToLoad)
    {
        if (PlayedTutorial >= 1)
        {
            mainMenu.SetActive(false);
            LoadingScreen.SetActive(true);
            levelToLoad = "MainGame";
            StartCoroutine(LoadLevelAsync(levelToLoad));
        }
        else
        {
            mainMenu.SetActive(false);
            LoadingScreen.SetActive(true);
            StartCoroutine(LoadLevelAsync(levelToLoad));
        }
            
    }
    public void openControls()
    {
        if (controlsOpen)
        {
            controlsOpen = false;
            Controls.gameObject.SetActive(false);
            mainMenu.gameObject.SetActive(true);
        }
        else
        {
            controlsOpen = true;
            mainMenu.gameObject.SetActive(false);
            Controls.gameObject.SetActive(true);
        }
    }

    public void QuitGame()
    {
        Application.Quit(); 
    }
}
