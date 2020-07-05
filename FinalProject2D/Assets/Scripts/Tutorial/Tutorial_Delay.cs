using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial_Delay : Tutorial
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
        m_TextImage.SetActive(true);
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
            IsCurrentTutorial = false;
            //m_TextImage.SetActive(false);
            CompleteTutorial();
        }
    }
}
