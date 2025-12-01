using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject LoadingScreen;
    [SerializeField] private GameObject mainMenu;

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

    public void LoadLevel(string levelToLoad)
    {
        mainMenu.SetActive(false);
        LoadingScreen.SetActive(true);
        StartCoroutine(LoadLevelAsync(levelToLoad));
    }

    public void QuitGame()
    {
        Application.Quit(); 
    }
}
