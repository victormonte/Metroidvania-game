using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string newGameScene;

    public void NewGame()
    {
        SceneManager.LoadScene(newGameScene);
    }

    public void Quit()
    {
        Application.Quit();

        Debug.Log("Game Quit");
    }
}
