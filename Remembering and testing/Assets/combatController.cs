using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class combatController : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isComboPossible;
    public Animator anim;
    public bool recievedInput;
    void Start()
    {
        anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isComboPossible)
        {
            attack(0);
        }
        else if (Input.GetMouseButtonDown(1) && isComboPossible)
        {
            attack(1);
        }
        Debug.Log(isComboPossible);
    }

    void comboPossible()
    {
        isComboPossible = true;
        Debug.Log("combo is possible");

    }

    void resetCombo()
    {
        isComboPossible = false;
        // anim.ResetTrigger("attackR");
        // anim.ResetTrigger("attackL");
        Debug.Log("combo is reset");

    }

    void attack(int mouseKey)
    {
        if (mouseKey == 0)
        {
            anim.SetTrigger("attackR");
            anim.ResetTrigger("attackL");
        }
        else
        {
            anim.SetTrigger("attackL");
            anim.ResetTrigger("attackR");
        }
    }



}
