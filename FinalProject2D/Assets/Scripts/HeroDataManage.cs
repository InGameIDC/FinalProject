using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroDataManage : MonoBehaviour
{
    [SerializeField] HeroData data;

    public HeroData GetData() => data;
}
