
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    [Header("Health Settings")]
    [SerializeField]
    private int maxHealth = 100;


    private int currentHealth;


    [SerializeField]

    private Image healthBar;



    public GameObject DeathPopup;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
   

    }

 

    public void TakeDamage(int amount)
    {
     
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Death();

        }
    }


    public void UpdateHealthBar()
    {
        if(healthBar != null)
        {
            healthBar.fillAmount = (float)currentHealth / maxHealth;
        }
    }


    public void Death()
    {

       
        healthBar.fillAmount = 0;

        DeathPopup.SetActive(true);

        StartCoroutine(deathBuffer());
        
    
    }


    IEnumerator deathBuffer()
    {
        yield return new WaitForSeconds(4);

        Scene currentScene = SceneManager.GetActiveScene();

        SceneManager.LoadScene(currentScene.buildIndex, LoadSceneMode.Single);
    }




    private void OnTriggerEnter2D(Collider2D collision) // for when an object hits the player the player dies
    {
        if (collision.CompareTag("Throwable"))
        {

            var controller = GetComponent<PlayerController>();

            var objMovement = collision.GetComponentInParent<ObjectLadderRoll>();
      
         

            if(controller != null && objMovement != null)
            {

                objMovement.enabled = false;
                controller.enabled = false; // players movement stops
                
                
                Death();
            }
            
            
          


        }
    }










}
