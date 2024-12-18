using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Event : MonoBehaviour
{
    BattleManager bManager;

    public string sceneName;

    public GameObject Object;

    bool isTutorialDone= false;

    public Sequence punch, dodge, special;

    public bool punchCheck,dodgeCheck,staminaCheck,superCheck,staminaDrain;

    public List<GameObject> tutorialPanels;

    private void Start()
    {
        bManager = BattleManager.instance;

        StartCoroutine(TutorialCheck());
    }

    private void Update()
    {
        
        if (bManager.enemy.health <= 40)
        {
            if (isTutorialDone)
            {
                if (PlayerPrefs.GetInt("Level") < 1)
                {
                    PlayerPrefs.SetInt("Level", 2);
                }
                SceneManager.LoadScene(sceneName);
            }
            else
            {
                bManager.enemy.health = 60;
            }
        }
        if (bManager.player.health <= 10 && (!isTutorialDone || bManager.player.isTired) )
        {
            bManager.player.health = 30;
        }
    }

    IEnumerator TutorialCheck()
    {    
        bool stop = false;
        bool leftDodge = false;
        bool rightDodge = false;

        bManager.player.superPunch = 60;

        while (!isTutorialDone)
        {
            if (bManager.player.isTired) 
            {
                staminaDrain = true;
            }
            if (!bManager.player.isTired&& staminaDrain)
            {
                yield return new WaitForSeconds(0.5f);
                staminaDrain = false;
            }

            if (!punchCheck)
            {
                PunchTutorial();
            }
            if (!dodgeCheck && punchCheck && !bManager.player.isTired &&!staminaDrain)
            {

                if (bManager.enemyMove == Move.LPunch && !rightDodge)
                {
                    tutorialPanels[1].SetActive(true);

                    if (!stop)
                    {
                        yield return new WaitForSeconds(0.1f);
                        Time.timeScale = 0.5f;
                        yield return new WaitForSeconds(0.1f);
                        Time.timeScale = 0f;
                        stop = true;
                    }
                    if (Input.GetKeyDown(bManager.player.RDodgeKey))
                    {
                        tutorialPanels[1].SetActive(false);
                        Time.timeScale = 1f;
                        rightDodge = true;
                        stop = false;

                    }

                }


                if (bManager.enemyMove == Move.RPunch && !leftDodge)
                {
                    tutorialPanels[2].SetActive(true);

                    if (!stop)
                    {
                        yield return new WaitForSeconds(0.1f);
                        Time.timeScale = 0.5f;
                        yield return new WaitForSeconds(0.1f);
                        Time.timeScale = 0f;
                        stop = true;
                    }
                    if (Input.GetKeyDown(bManager.player.LDodgeKey))
                    {
                        tutorialPanels[2].SetActive(false);
                        Time.timeScale = 1f;
                        leftDodge = true;
                        stop = false;

                    }
                }
                

                if (leftDodge && rightDodge)
                {
                    dodgeCheck = true;
                }
            }

            if (bManager.player.isTired&& !staminaCheck)
            {
                tutorialPanels[3].SetActive(true);
                staminaCheck = true;
                    
            }
            if (!bManager.player.isTired && staminaCheck)
            {
                tutorialPanels[3].SetActive(false);
            }

            if (bManager.player.hasSuper && !superCheck)
            {
                tutorialPanels[4].SetActive(true);
                superCheck = true;
            }
            if (!bManager.player.hasSuper && superCheck)
            {
                tutorialPanels[4].SetActive(false);

            }
            if (punchCheck && dodgeCheck && superCheck && staminaCheck)
            {
                isTutorialDone = true;
            }

            yield return null;

        }
        
    }


    void PunchTutorial() 
    {
        tutorialPanels[0].SetActive(true);

        if (bManager.playerMove == Move.RPunch || bManager.playerMove == Move.LPunch)
        {
            tutorialPanels[0].SetActive(false);
            punchCheck = true;

            bManager.enemy.ChangeMoveList(dodge.moveList);
            bManager.enemy.moveSequence.Add(dodge);
            bManager.enemy.moveSequence.Add(dodge);
            bManager.enemy.moveSequence.Add(punch);
            bManager.enemy.moveSequence.Add(punch);
            bManager.enemy.moveSequence.Add(special);
        }
    }

}
