using Assets.Scripts.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static SaveLoader saveLoader;

    private void Awake()
    {
    }

    public void NewGame()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void LoadSave()
    {
        SaveLoader.Instance().IsNeedToLoad = true;
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void Close()
    {
        Application.Quit();
    }
}