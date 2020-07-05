using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tutorial : MonoBehaviour
{
    public gameObject popUp;
    [SerializeField]
    private int m_Id;

    [TextArea]
    public string m_ExplanationText;
    
    public int Id { get; set; }
    public string ExplanationText { get; set; }
    string first = "it looks like your friends are prisoned" 
    string second = "in order to free them you must fight the carrots";
    string third = "to attack a vegtable u must touch your hero and then touch the vegtalbe u want to to attack."
    string forth = "kill the carrots"

    gameObject text_box.text
    


    public virtual void CheckIfHappening()
    {
    }

    void Awake()
    {
        TutorialManager.Instance.TutorialList.Add(this);
    }
}
