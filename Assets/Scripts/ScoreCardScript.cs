using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreCardScript : MonoBehaviour
{

    [SerializeField] TMP_Text yourScore;
    //[SerializeField] TMP_Text yourTime;
    // [SerializeField] TMP_Text yourDeaths;


    [SerializeField] PlayerScoreScript playerScore;

    public GameManager gameManager;



    public TMP_Text BonusPopup;
    [SerializeField] TMP_Text BonusScore;

    public PlayerScoreScript playerScoreScript;

    public AudioSource ScoreCardAudio;

    public AudioClip CountUpSound;


    [SerializeField] float CountSpeed = 0.025f;

    public static ScoreCardScript Instance;




    private void Awake()
    {
        Instance = this;
    }




    private void OnEnable()
    {
        int ContinuousTotal = GameDataManager.Instance.TotalScore;

        yourScore.text = $"Your Score: {ContinuousTotal}";


    }




    public void BonusScorePointsCountUp()
    {
        StartCoroutine(StartCountUpWithDelay(CountSpeed));
    }

    private IEnumerator StartCountUpWithDelay(float CountSpeed)
    {

        BonusScore.gameObject.SetActive(false);




        yield return new WaitForSeconds(1f);


        BonusScore.text = "0";
        BonusScore.gameObject.SetActive(true);

        ScoreCardAudio.clip = CountUpSound;
        ScoreCardAudio.loop = true;
        ScoreCardAudio.Play();


        int targetScore = playerScoreScript.BonusScore;
        StartCoroutine(CountUpRoutine(targetScore, CountSpeed));
    }





    public IEnumerator CountUpRoutine(int targetScore, float CountSpeed)
    {



        int currentScoreCount = 0;

        BonusScore.text = "0";

        while (currentScoreCount < targetScore)
        {
            currentScoreCount++;
            BonusScore.text = currentScoreCount.ToString();

            yield return new WaitForSeconds(CountSpeed);

        }



        ScoreCardAudio.Stop();

        playerScoreScript.FinalScore += targetScore;

        GameDataManager.Instance.AddScore(targetScore);

        yourScore.text = $"Your Score: {GameDataManager.Instance.TotalScore}";

        gameManager.StartVictoryBuffer();


    }






    public void OnScoreCardShow()
    {
        // If player got a perfect score, BonusCountdown() will run
        if (playerScore.FinalScore >= playerScore.scorePossible)
        {
            return; // BonusCountdown will handle scene switching
        }

        gameManager.StartVictoryBuffer();
    }











}
