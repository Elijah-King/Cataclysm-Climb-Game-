using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    // The purpose of this script is to handle general level switching when reaching end of level triggers


    public GameObject EndLevelPopUp;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("IntroLevelEndTrigger"))
        {
            EndLevelPopUp.SetActive(true);
            
            SceneManager.LoadScene(2);
        }
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
