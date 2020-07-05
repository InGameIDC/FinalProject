using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Tutorial_Trigger : Tutorial
{
    [SerializeField]
    private bool m_IsCurrentTutorial = true;

    public GameObject m_HiddenObject;
    // This is the object we want the player to get to. (the area to collide)
    public Transform HitTransform;

    public bool IsCurrentTutorial
    {
        get { return m_IsCurrentTutorial; }
        set { m_IsCurrentTutorial = value; }
    }

    public override void CheckIfHappening()
    {
        m_HiddenObject.SetActive(true);
        IsCurrentTutorial = true;
    }

    public void OnTriggerEnter2D(Collider2D i_Other)
    {
        if (IsCurrentTutorial == false)
        {
            Debug.Log("8====================D");
            return;
        }
        else
        {
            if (i_Other.transform == HitTransform)
            {
                Debug.Log("Trigger with " + i_Other.gameObject);
                m_HiddenObject.SetActive(false);
                IsCurrentTutorial = false;
                CompleteTutorial();
            }
            else
            {
                Debug.Log("Tag Doesn't match");
            }
        }
    }
}
