using UnityEngine;

public class KillBoxScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Player hit the kill box");

        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerhealth = collision.GetComponent<PlayerHealth>();
            if (playerhealth != null)
            {
                playerhealth.TakeDamage(100);
                playerhealth.Death();
            }
          
        }
    }
}

