using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tests2D : MonoBehaviour
{
    [SerializeField] public GameObject testTarget;
    [SerializeField] public float testRange;
    private Skill _skill;
    // Start is called before the first frame update

    private void Awake()
    {
        _skill = GetComponent<Skill>();
    }
    void Start()
    {
        bool check = SpaceCalTool.AreObjectsViewableAndWhithinRange(gameObject, testTarget, testRange);
        Debug.Log("AreObjectsViewableAndWhithinRange: " + check);
    }

    private void Update()
    {
        _skill.attack();
        Debug.DrawRay(transform.position, SpaceCalTool.GetVectorDirectionTowardTarget(transform.position, testTarget.transform.position));
    }

}
