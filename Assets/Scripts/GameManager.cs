using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    // The purpose of this script is to handle general level switching when reaching end of level triggers


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("IntroLevelEndTrigger"))
        {
            SceneManager.LoadScene(1);
        }
    }








}
