using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{

    [Header("Health Settings")]
    [SerializeField]
    private int EnemymaxHealth = 100;


    private int EnemycurrentHealth;


    [SerializeField]

    private Image EnemyhealthBar;

    public GameObject enemy;





    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EnemycurrentHealth = EnemymaxHealth;
        UpdateHealthBar();
    }

 

    public void TakeDamage(int amount)
    {
     
        EnemycurrentHealth -= amount;
        EnemycurrentHealth = Mathf.Clamp(EnemycurrentHealth, 0, EnemymaxHealth);
        UpdateHealthBar();

        if (EnemycurrentHealth <= 0)
        {
            Death();

        }
    }


    public void UpdateHealthBar()
    {
        if(EnemyhealthBar != null)
        {
            EnemyhealthBar.fillAmount = (float)EnemycurrentHealth / EnemymaxHealth;
        }
    }


    public void Death()
    {


        enemy.SetActive(false);
        
        
        
        EnemyhealthBar.fillAmount = 0;

       
        
    
    }








}
