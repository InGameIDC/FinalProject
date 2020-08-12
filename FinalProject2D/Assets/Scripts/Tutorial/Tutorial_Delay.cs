using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Delay : Tutorial
{
    [SerializeField]
    private bool m_IsCurrentTutorial = true;

    public float m_DelayTime = 1f;
    public bool IsCurrentTutorial
    {
        get { return m_IsCurrentTutorial; }
        set { m_IsCurrentTutorial = value; }
    }

    public override void CheckIfHappening()
    {
        m_TextImage.SetActive(true);
        //StartCoroutine("WaitForSecondsRealtime", m_DelayTime);
        IsCurrentTutorial = true;
    }

    public void Update()
    {
        if (IsCurrentTutorial == false)
        {
            return;
        }
        else
        {
            IsCurrentTutorial = false;
            CompleteTutorial();
        }
    }
}
