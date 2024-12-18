using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public List<Button> levels;
    public int levelCap;
    public bool reset;

    void Start()
    {
        levelCap = 1;
        if (PlayerPrefs.GetInt("Level") > 1)
        {
            levelCap = PlayerPrefs.GetInt("Level");
        }
    }

    private void Awake()
    {
        Camera.main.aspect = 16f / 9f;
    }

    private void Update()
    {
        for (int l = 0; l < levels.Count; l++)
        {
            levels[l].interactable = true;
            if (l + 1 > levelCap)
            {
                levels[l].interactable = false;
            }
        }
        if (reset) 
        {
            ClearSaves();
        }
    }
    public void AddLevel()
    {
        levelCap += 1;
    }
    public void ClearSaves()
    {
        PlayerPrefs.DeleteKey("Level");
    }
}
