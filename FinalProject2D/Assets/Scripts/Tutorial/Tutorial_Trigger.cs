using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Trigger : Tutorial
{
    [SerializeField]
    private bool m_IsCurrentTutorial = false;

    // This is the object we want the player to get to. (the area to collide)
    public Transform HitTransform;

    public bool IsCurrentTutorial
    {
        get { return m_IsCurrentTutorial; }
        set { IsCurrentTutorial = value; }
    }

    public override void CheckIfHappening()
    {
        HitTransform.gameObject.SetActive(true);
        IsCurrentTutorial = true;
    }

    public void OnTriggerEnter2D(Collider2D i_Other)
    {
        Debug.Log("yay: " + i_Other);
        if (!IsCurrentTutorial)
        {
            return;
        }
        else
        {
            if (i_Other.transform == HitTransform)
            {
                Debug.Log("Trigger with " + i_Other.gameObject);
                HitTransform.gameObject.SetActive(false);
                IsCurrentTutorial = false;
                CompleteTutorial();
            }
        }
    }
}
