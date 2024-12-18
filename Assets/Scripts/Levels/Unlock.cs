using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unlock : MonoBehaviour
{
    public Button button;
    public GameObject bg;

    public string scene;
    void Update()
    {
        if (scene == "Level Select")
        {
            if (button.interactable == false)
            {
                bg.SetActive(false);
            }
            else if (button.interactable == true)
            {
                bg.SetActive(true);
            }
        }

    }
}
