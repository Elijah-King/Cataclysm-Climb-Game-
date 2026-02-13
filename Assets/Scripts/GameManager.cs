using JetBrains.Annotations;
using System.Collections;
using System.Runtime.CompilerServices;
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


            var LevelEnd = collision.GetComponent<EndLevelScript>();

            EndLevelPopUp.SetActive(true);


            if (LevelEnd != null)
            {
                StartCoroutine(VictoryBuffer(LevelEnd.GetSceneToLoad()));
            }
           
            
        }
    }



    IEnumerator VictoryBuffer(int sceneIndex)
    {
        yield return new WaitForSeconds(4);

      

        SceneManager.LoadScene(sceneIndex);
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
