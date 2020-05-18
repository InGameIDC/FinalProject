using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class userGenerator : MonoBehaviour
{

    private static string dataPath = string.Empty;
    
    void Start()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer){
            dataPath = System.IO.Path.Combine(Application.persistentDataPath,"/user.xml");
        }
        else  dataPath = System.IO.Path.Combine(Application.dataPath,"/user.xml");

        

        playerStats user = new playerStats("dor",1,450);
        XMLOp.Serialize(user,"/user.xml");
        Debug.Log("creatKKKKKKKkkkkkked");
    }
    public static void generate (){
        playerStats user = new playerStats("dor",1,450);
        XMLOp.Serialize(user,"/user.xml");
        Debug.Log("creatKKKKKKKkkkkkked");
    }
    void update(){
        Debug.Log("hellp");
    }

  
}
