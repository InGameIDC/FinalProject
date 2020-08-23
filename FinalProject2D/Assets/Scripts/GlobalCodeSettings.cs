using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalCodeSettings
{
    public const float FRAME_RATE = 0.0167f;
    // const float FRAME_RATE = 0.005f; // use for fast tasting
    public const float DESIRED_POS_MARGIN_OF_ERROR = 0.1f;
    public const float AI_Refresh_Time = 0.5f;
    public const float RESPAWN_TIME = 10f;
    public const float Y_RELATIVE_TO_X = 0.75f;
    public const float ACCELARATION = -6f;
    public const float ACCELARATION_TOWARD_MOVING_ENEMY = -6f;
    public const float Minumum_Movment_To_Count = 0.2f;
    public static readonly string[] Layers_To_RayCast_Ignore = { "Terrain", "Ignore Raycast" };
    public static readonly string[] Layers_To_RayCast_Relate = { "Obstacle"};
    public static readonly string[] Layers_Teams = {"HeroUnit", "EnemyUnit"};
    public static readonly string[] Layers_Mouse_RayCast_Relate = { "Terrain", "HeroUnit", "EnemyUnit", "UI"};

    public static float CaclTimeRelativeToFramRate(float secs)
    {
        return secs * 59.888f * GlobalCodeSettings.FRAME_RATE;
    }
}
