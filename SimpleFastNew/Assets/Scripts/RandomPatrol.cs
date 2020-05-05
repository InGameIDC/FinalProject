using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RandomPatrol : MonoBehaviour
{
    private float minX = -8.2f;
    private float maxX = 8.2f;
    private float minY = -4.3f;
    private float maxY = 4.3f;

    public float minSpeed = 0.75f;
    public float maxSpeed = 3f;
    private float speed;



    public float secondsToMaxDifficulty;

    Vector2 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        targetPosition = GetRandomPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if((Vector2)transform.position != targetPosition)
        {
            speed = Mathf.Lerp(minSpeed, maxSpeed, GetDifficutlyPercent());
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
        else
        {
            targetPosition = GetRandomPosition();
        }
        
    }

    Vector2 GetRandomPosition()
    {
        float RandomX = Random.Range(minX, maxX);
        float RandomY = Random.Range(minY, maxY);
        return new Vector2(RandomX, RandomY);
    }



    float GetDifficutlyPercent()
    {
        return Mathf.Clamp01(Time.timeSinceLevelLoad / secondsToMaxDifficulty);
    }
}