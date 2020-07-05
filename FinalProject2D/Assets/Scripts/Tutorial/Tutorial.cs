using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tutorial : MonoBehaviour
{
    [SerializeField]
    private int m_Id;

    [TextArea]
    public string m_ExplanationText;

    public int Id
    {
        get { return m_Id; }
        set { m_Id = value; }
    }

    public string ExplanationText
    {
        get { return m_ExplanationText;}
        set { m_ExplanationText = value; }
    }

    public virtual void CheckIfHappening()
    {
    }

    public void CompleteTutorial()
    {
        if (this == TutorialManager.Instance.CurrentTutorial)
        {
            TutorialManager.Instance.CompletedTutorial();
        }
    }

    void Awake()
    {
        TutorialManager.Instance.TutorialList.Add(this);
    }
}
