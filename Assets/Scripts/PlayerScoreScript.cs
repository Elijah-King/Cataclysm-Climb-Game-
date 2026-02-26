using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScoreScript : MonoBehaviour
{


    public int FinalScore; // Score player will have at end of level

    public int BonusScore = 100;

    [SerializeField] int ScoreTriggersInLevel; // amount of score triggers in the level


    public int scorePossible; // Max amount of score achievable in level


    public ScoreCardScript scoreCard;


    public AudioSource BonusSource;

    public AudioClip PlayBonusPopupSound;


    private void Start()
    {
        scorePossible = ScoreTriggersInLevel * 10;


      


    }




  




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("ScoreTrigger"))
        {
            FinalScore += 10;











        }
    }





   public IEnumerator BonusCountdown()
    {
        yield return new WaitForSeconds(2);


        BonusSource.PlayOneShot(PlayBonusPopupSound);
        scoreCard.BonusPopup.gameObject.SetActive(true);

        scoreCard.BonusScorePointsCountUp();
    }








}
