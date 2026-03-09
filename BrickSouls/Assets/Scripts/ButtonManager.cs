using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    // Start is called before the first frame update
    public void StartGame()
    {
        SceneManager.LoadScene("Nivel_1");
    }

    public void GoToLevel2()
    {
        SceneManager.LoadScene("Nivel_2");
    }
    public void GoToLevel3()
    {
        SceneManager.LoadScene("Nivel_3");
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void OnPauseClick()
{
    if (GameManager.instance != null)
    {
        GameManager.instance.TogglePause();
    }
}

public void ResumeGame()
{
    if (GameManager.instance != null)
    {
        GameManager.instance.TogglePause();
    }
}
}
