using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tutorial : MonoBehaviour
{
    [SerializeField]
    private int m_Id;

    [TextArea]
    public string m_ExplanationText;
    
    public int Id { get; set; }
    public string ExplanationText { get; set; }

    public virtual void CheckIfHappening()
    {
    }

    void Awake()
    {
        TutorialManager.Instance.TutorialList.Add(this);
    }
}
