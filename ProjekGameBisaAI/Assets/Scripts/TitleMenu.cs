using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour
{

    public Animator transition;
    public float transitionTime;
    IEnumerator LevelLoaderFunc(int levelIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }

    public void LoadLevel()
    {
        StartCoroutine(LevelLoaderFunc(SceneManager.GetActiveScene().buildIndex + 1));
    }
    // Update is called once per frame
    private void Start()
    {

    }
    public void PlayGame()
    {
        LoadLevel();
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}
