using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour
{   
    public static GameManager instance;

    Player player;

    int hatCounter;

    FinishingManager FinishingManager;

    //ending level related variables
    [Header("Ending variables")]
    public bool gameHasEnded = false;
    int totalScore;
    
    //----------------
    [SerializeField] GameObject hatPrefab;

    [Header("Game Hud")]
    public Canvas gameHud;

    [SerializeField] TMP_Text currentLevelText;
    [SerializeField] TMP_Text hatCounterText;
    public int scoreForNextLevel;


    [Header("End Hud")]
    public Canvas endHud;
    [SerializeField] int sceneLoadDelayTime;
    [SerializeField] int timeForCalculatingTotalScore;
    [SerializeField] TMP_Text scoreForNextLevelText;
    public TMP_Text totalScoreText;

    [Header("DoTween")]
    [SerializeField] float doTweenDelayTime;

    [SerializeField] float maxScale;
     
    ////
    ////   ***********<SUMMARY>*************
    //// Game manager script is alive every scene.
    /// this script manages all the scores and hat related stuff in the game
    /// and all the huds that player can and cant see.
    /// as you can see at the bottom this script also finishes the game.


    // assigning variables
    private void Awake() 
    {
        if(instance == null)
        {
            instance = this;
        } 
    }

    private void Start() 
    {
        player = FindObjectOfType<Player>();
        FinishingManager = FindObjectOfType<FinishingManager>();
        
        scoreForNextLevelText.text = "Collect " + scoreForNextLevel + " points for next level!";

        currentLevelText.text = SceneManager.GetActiveScene().name;
        
    }
    
    //Calling the function every time the hat counter variable changes.
    public void UpdateHatCounter()
    {
        hatCounter = player.collectedHatList.Count;

        hatCounterText.text = hatCounter.ToString();
    }


    // calculating the gates (+ -)
    
    public void AddHats(int randNumToProcess)
    {
        for (int i = 0; i < randNumToProcess; i++)
        {   
            SpawnHat(player.transform.position);
            UpdateHatCounter();
        }
    }
    public void SubtractHats(int randNumToProcess)
    {
        UpdateHatCounter();
        for (int i = 0; i < randNumToProcess; i++)
        {
            DespawnHat();
            UpdateHatCounter();
        }
    }

    //spawn and despawn fuctions for the hats
    void SpawnHat(Vector3 position)
    {
        Quaternion spawnRotation = Quaternion.Euler (0 , 90 ,0);
        GameObject spawnedHat = Instantiate(hatPrefab, position, spawnRotation);
    }
    private void DespawnHat()
    {
        if (player.collectedHatList.Count > 0)
        {
            GameObject objectToDespawn = player.collectedHatList[player.followingHatCounter];
            // deleting the last indexed obj form list
            player.collectedHatList.RemoveAt(player.followingHatCounter);
            --player.followingHatCounter;
            UpdateHatCounter();
            player.UpdateNextHatPos();
            // moving the object away in order to set active with chunk optimzer
            objectToDespawn.transform.position = new Vector3(0, -100, 0);
            objectToDespawn.SetActive(false);
        }
    }

    /// Ending functions << when player crosses the finishing line these functions occure. 

    public void GameWon()
    {
        if(gameHasEnded)
        {
            player.animator.SetBool("GameWon",true);
            
            StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1));   
        } 
    }

    public void GameLost()
    {
        if(gameHasEnded)
        {
            player.animator.SetBool("GameLost",  true);
            StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex));
        }
    }

    IEnumerator LoadScene(int sceneInt)
    {
        yield return new WaitForSecondsRealtime(sceneLoadDelayTime);
        SceneManager.LoadScene(sceneInt);
    }
    
    public void EndLevel()
    {

        if(totalScore >= scoreForNextLevel)
        {
            GameManager.instance.GameWon();
            scoreForNextLevelText.color = Color.green;
        }
        else
        {
            GameManager.instance.GameLost();
            scoreForNextLevelText.color =  Color.red;
        }
    }
    public void UpdateTotalScore()
    {   
        float scale = 1.2f;
        DOTween.To(() => totalScore , x=> totalScore = x,player.collectedHatList.Count, doTweenDelayTime)
            .OnUpdate(() => {
                totalScoreText.text = totalScore.ToString();
            });

        DOTween.To(() => scale, x => scale = x, maxScale,1f)
            .OnUpdate(() => {
                totalScoreText.rectTransform.localScale = new Vector3(scale, scale, scale);
            }); 
        totalScoreText.text = totalScore.ToString();
    }              

}
