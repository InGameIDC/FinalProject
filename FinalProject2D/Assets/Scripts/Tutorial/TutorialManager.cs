using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.WSA.Persistence;

public class TutorialManager : MonoBehaviour
{
    public LevelChanger m_LevelChanger;
    private const string k_CompletedAllTutorials = "Level complete! Proceed to next level";
    [SerializeField]
    private List<Tutorial> m_TutorialList = new List<Tutorial>();
    
    [SerializeField]
    private Text m_ExplanationText;
    private static TutorialManager thisInstance;
    [SerializeField]
    private Tutorial m_CurrentTutorial;

    public Tutorial CurrentTutorial
    {
        get { return m_CurrentTutorial; }
        set { m_CurrentTutorial = value; }
    }

    public Text ExplanationText
    {
        get { return m_ExplanationText; }
        set { m_ExplanationText = value; }
    }

    public List<Tutorial> TutorialList
    {
        get { return m_TutorialList; }
    }
    public static TutorialManager Instance
    {
        get
        {
            if (thisInstance == null)
            {
                thisInstance = GameObject.FindObjectOfType<TutorialManager>();
            }

            // If it's still null - there is no tut manager.
            if (thisInstance == null)
            {
                Debug.Log("There is no TutManager!");
            }

            return thisInstance;
        }
    }

    public void SetNextTutorial(int i_CurrentOrder)
    {
        CurrentTutorial = GetTutorialById(i_CurrentOrder);
        if (CurrentTutorial)
        {
            ExplanationText.text = CurrentTutorial.ExplanationText;
            CurrentTutorial.CheckIfHappening();
        }
        else
        {
            CompletedAllTutorials();
        }
    }

    public void CompletedTutorial()
    {
        SetNextTutorial(CurrentTutorial.Id + 1);
    }

    public void CompletedAllTutorials()
    {
        Debug.Log("Finished everything");
        ExplanationText.text = k_CompletedAllTutorials;
        m_LevelChanger.FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public Tutorial GetTutorialById(int i_RequestedId)
    {
        Tutorial result = null;
        foreach (Tutorial tutorial in TutorialList)
        {
            if (tutorial.Id == i_RequestedId)
            {
                result = tutorial;
            }
        }

        return result;
    }

    void Start()
    {
        SetNextTutorial(0);
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    if (CurrentTutorial)
    //    {
    //        CurrentTutorial.CheckIfHappening();
    //    }
    //}
}
