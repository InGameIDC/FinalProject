using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    public float m_Delay = 3;
    public Animator animator;
    public int levelToLoad;
    public string key = null;

    void Update()
    {
    }

    public void FadeToLevel(int levelIndex)
    {
        levelToLoad = levelIndex;
        animator.SetTrigger("FadeOut");
    }

    /*
    public void FadeToNextLevel()
    {
        FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }
    */

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(levelToLoad);
    }
    public void makeAlive()
    {
        animator.SetBool("isDead", false);
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.UnloadScene(scene.name);
        SceneManager.LoadScene(scene.name);
    }
}
