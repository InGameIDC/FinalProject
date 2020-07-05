using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Touch : Tutorial
{
    private bool m_IsCurrentTutorial = false;

    [SerializeField]
    public bool IsCurrentTutorial 
    {
        get { return m_IsCurrentTutorial; }
        set { m_IsCurrentTutorial = value; } 
    }

    public override void CheckIfHappening()
    {
        IsCurrentTutorial = true;
    }

    public void Update()
    {
        if (!IsCurrentTutorial)
        {
            return;
        }
        else
        {
            if (Input.touchCount == 1)
            {
                IsCurrentTutorial = false;
                CompleteTutorial();
            }
        }
    }
}
