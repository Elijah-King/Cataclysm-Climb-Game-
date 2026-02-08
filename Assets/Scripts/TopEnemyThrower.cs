using System.Collections;
using UnityEngine;

public class TopEnemyThrower : MonoBehaviour
{
    public Transform holdPoint;
    public Transform pileParent;
    public float throwForce = 5f;

    private GameObject heldObject;

    void Start()
    {
        StartCoroutine(ThrowLoop());
    }

    IEnumerator ThrowLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);

            if (!PickupFromPile())
                yield break;

            yield return new WaitForSeconds(1f);

            ThrowObject();
        }
    }

    bool PickupFromPile()
    {
        foreach (Transform child in pileParent)
        {
            if (!child.gameObject.activeInHierarchy)
            {
                heldObject = child.gameObject;
                heldObject.SetActive(true);
                heldObject.transform.position = holdPoint.position;
                heldObject.transform.SetParent(holdPoint);

                Rigidbody2D rb = heldObject.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.bodyType = RigidbodyType2D.Kinematic;
                    rb.linearVelocity = Vector2.zero;
                }

                return true;
            }
        }

        Debug.Log("Pile is empty!");
        return false;
    }

    void ThrowObject()
    {
        heldObject.transform.SetParent(null);

        Rigidbody2D rb = heldObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;

            // Always throw right
            Vector2 dir = Vector2.right;

            rb.AddForce(dir * throwForce, ForceMode2D.Impulse);
        }

        heldObject = null;
    }








}



