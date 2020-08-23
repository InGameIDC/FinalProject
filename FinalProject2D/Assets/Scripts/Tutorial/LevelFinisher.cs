using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFinisher : MonoBehaviour
{
    [SerializeField] GameObject part3;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent.tag == "HeroUnit")
        {
            part3.SetActive(false);
            PlayerPrefs.SetInt("FinishedTutorial", 1);
        }
        //StartCoroutine(loadSceneAfterDelay(2f));
    }

    private IEnumerator loadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("HomeMenu");
    }

}
