using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Data", menuName = "Level Data", order = 51)]
public class LevelData : ScriptableObject
{
    [SerializeField]
    private int _levelId;

    [SerializeField]
    private int[] coinsReward = {400,600,800};

    public int getlevelId() => _levelId;
    public float getCoinsReward(int stars) => coinsReward[stars-1];


}
