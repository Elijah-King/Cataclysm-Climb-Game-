using UnityEngine;

public class TempEnemyDamage : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerhealth = collision.GetComponent<PlayerHealth>();

            if (playerhealth != null)
            {

                
                    playerhealth.TakeDamage(25);
                
            }

        }
    }

}
