using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class heroesToChoose : MonoBehaviour
{

    GameObject gs;
    public string levelToPlay;


    // Start is called before the first frame update
    void Start()
    {
        gs = GameObject.FindGameObjectWithTag("GameStatus");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToLevel()
    {
        SceneManager.LoadScene(gs.GetComponent<GameStatus>().lastLevelCosen);
    }
}
