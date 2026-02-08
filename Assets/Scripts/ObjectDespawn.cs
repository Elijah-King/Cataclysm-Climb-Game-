using UnityEngine;

public class ObjectDespawn : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Despawner"))
        {
            gameObject.SetActive(false);
        }
    }




}
