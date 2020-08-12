using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class red_fill_animation : MonoBehaviour
{
    private Animator a_Animator;
    void Start()
    {
        a_Animator = gameObject.GetComponent<Animator>();
        a_Animator.SetBool("loosing", false);
    }
    void update()
    {
        if (gameObject.GetComponentInChildren<Animator>().GetBool("loosing"))
        {
            a_Animator.SetBool("loosing", true);
        }
            a_Animator.SetBool("loosing", false);
    }
}
