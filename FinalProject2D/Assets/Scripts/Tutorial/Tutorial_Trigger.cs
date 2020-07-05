using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Trigger : Tutorial
{
    private bool m_IsCurrentTutorial = false;

    // This is the object we want the player to get to. (the area to collide)
    public Transform HitTransform;

    [SerializeField]
    public bool IsCurrentTutorial { get; set; }

    public override void CheckIfHappening()
    {
        IsCurrentTutorial = true;
    }

    public void OnTriggerEnter(Collider i_Other)
    {
        if (!IsCurrentTutorial)
        {
            return;
        }
        else
        {
            if (i_Other.transform == HitTransform)
            {
                TutorialManager.Instance.CompletedTutorial();
                IsCurrentTutorial = false;
            }
        }
    }
}
