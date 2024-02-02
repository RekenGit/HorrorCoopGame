// Przypisz do obiektu: Player
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkAnim : MonoBehaviour
{
    public Animator anim;
    public new Rigidbody rigidbody;
    public bool Idzie = false;
    public bool Cofa = false;

    private void FixedUpdate()
    {
        if (rigidbody.IsSleeping())
        {
            anim.SetInteger("idzie", 0);
        }
        else
        {
            anim.SetInteger("idzie", 1);
        }
        if (Idzie)
        {
            anim.SetInteger("idzie", 1);
            anim.SetInteger("cofa", 0);
        }
        else if (Cofa)
        {
            anim.SetInteger("idzie", 0);
            anim.SetInteger("cofa", 1);
        }
        else
        {
            anim.SetInteger("idzie", 0);
            anim.SetInteger("cofa", 0);
        }
    }
}
