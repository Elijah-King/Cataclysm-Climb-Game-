using UnityEngine;

public class ObjAnimations : MonoBehaviour
{

    public Animator anim;


    void Start()
    {
        anim.SetBool("flameMoving", true);
    }

}
