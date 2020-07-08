using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{

    public Animator animator;
    public int levelToLoad;

    void Update()
    {
    }

    public void FadeToLevel(int levelIndex)
    {
        levelToLoad = levelIndex;
        animator.SetTrigger("FadeOut");
    }

    public void FadeToNextLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.buildIndex + 1 <= SceneManager.sceneCountInBuildSettings)
        {
            FadeToLevel(currentScene.buildIndex + 1);
        }
    }

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
