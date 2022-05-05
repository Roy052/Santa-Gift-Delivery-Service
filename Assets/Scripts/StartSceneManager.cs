using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    public void ToStart()
    {
        SceneManager.LoadScene(2);
    }

    public void ToTutorial()
    {
        SceneManager.LoadScene(1);
    }

    public void ToExit()
    {
        Application.Quit();
    }
}
