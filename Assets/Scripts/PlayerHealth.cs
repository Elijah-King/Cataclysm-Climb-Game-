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

        StartCoroutine(deathBuffer());
        
    
    }


    IEnumerator deathBuffer()
    {
        yield return new WaitForSeconds(4);

        SceneManager.LoadScene(0);
    }





}
