using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneNavigator : MonoBehaviour
{
    public int sceneNumber;
    public string specificScene;
    public GameObject onObj;
    public GameObject offObj;

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void LoadSceneNumber() //Load scent using int
    {
        SceneManager.LoadScene(sceneNumber);
    }
    public void LoadSpecificScene()//Load scent using string
    {
        SceneManager.LoadScene(specificScene);
    }

    public void Object() 
    {
        if (onObj != null) 
        {
            onObj.SetActive(true);
        }
        if (offObj != null) 
        {
            offObj.SetActive(false);
        }
    }

}
