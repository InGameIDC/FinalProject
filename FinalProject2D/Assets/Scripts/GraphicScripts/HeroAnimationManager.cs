using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAnimationManager : MonoBehaviour
{
    public Animator animator;
    private GameObject _rotator;
    private Movment2D _movment2d;
    private int state;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        // movment
        _movment2d = GetComponentInChildren<Movment2D>();
        _movment2d.OnStartMovmentAnimation += Moving;
        _movment2d.OnFinishMovmentAnimation += FinishedMoving;
        animator.SetBool("IsMoving", false);

        _rotator = transform.Find("Rotator").gameObject;
        state = 0;
    }

    // Update is called once per frame
    void Update()
    {
        setDirection();
        //Debug.Log("Direction: " + animator.GetInteger("Direction"));
    }

    private void setDirection()
    {
        float rotation = _rotator.transform.rotation.eulerAngles.z;

        
        if (rotation % 360 >= 45 && rotation % 360 < 135) // Left
            animator.SetInteger("Direction", 3);
        else if (rotation % 360 >= 135 && rotation % 360 < 225) // Down
            animator.SetInteger("Direction", 0);
        else if (rotation % 360 >= 225 && rotation % 360 < 315) // Right
            animator.SetInteger("Direction", 1);
        else // UP
            animator.SetInteger("Direction", 2);

        //Debug.Log("Rotation: " + rotation + " Direction: " + animator.GetInteger("Direction"));
    }

    private void Moving()
    {
        animator.SetBool("IsMoving", true);
    }
    private void FinishedMoving()
    {
        animator.SetBool("IsMoving", false);
    }
}
