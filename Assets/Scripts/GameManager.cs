using JetBrains.Annotations;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    // The purpose of this script is to handle general level switching when reaching end of level triggers


    public GameObject EndLevelPopUp;

    [SerializeField] GameObject ScoreCard;



    public PlayerScoreScript playerscoreBonusPopup;

    public AudioSource ScoreCardSource;

    public AudioClip ScoreCardStamp;


    public ScoreCardScript scoreCardScript;

    private EndLevelScript endlevelScript;

   public PlayerController playerController;



   


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("IntroLevelEndTrigger"))
        {


            endlevelScript = collision.GetComponent<EndLevelScript>();

            int FinalScore = playerscoreBonusPopup.FinalScore;

            int ScorePossible = playerscoreBonusPopup.scorePossible;


            EndLevelPopUp.SetActive(true);


            playerController.isFrozen = true;
            playerController.enabled = false;

            StartCoroutine("VictoryPopup");

        
        
        
        
        }



    
    
    
    
    
    
    
    }



    IEnumerator VictoryPopup()
    {

        yield return new WaitForSeconds(5);

        EndLevelPopUp.SetActive(false);
        
        
        ScoreCard.SetActive(true);

    

        ScoreCardSource.PlayOneShot(ScoreCardStamp);

        scoreCardScript.OnScoreCardShow();



        // Check score AFTER the score card appears
        if (playerscoreBonusPopup.FinalScore >= playerscoreBonusPopup.scorePossible)
          {
                StartCoroutine(playerscoreBonusPopup.BonusCountdown());
          


        }



    }







    IEnumerator VictoryBuffer(int sceneIndex)
    {
        yield return new WaitForSeconds(4);


        GameDataManager.Instance.AddLevelDeaths(playerscoreBonusPopup.LevelDeaths);
        
        SceneManager.LoadScene(sceneIndex);

    }





    public void StartVictoryBuffer()
    {
        var levelEnd = endlevelScript.GetComponent<EndLevelScript>();
        int sceneIndex = endlevelScript.GetSceneToLoad();
        StartCoroutine(VictoryBuffer(sceneIndex));

    }







    









    public void playGame()
    {
        SceneManager.LoadScene(1);
    }


    public void QuitGame()
    {
        Application.Quit();
    }












}
