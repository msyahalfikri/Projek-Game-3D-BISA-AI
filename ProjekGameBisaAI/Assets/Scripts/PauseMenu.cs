using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIspaused;
    private InputAction menu;
    public GameObject pauseMenuUI;
    private PlayerInput playerInput;
    private void Awake()
    {
        playerInput = new PlayerInput();
    }
    private void Update()
    {

    }
    // public void Resume()
    // {
    //     pauseMenuUI.SetActive(false);
    //     Time.timeScale = 1f;
    // }
    void Pause(InputAction.CallbackContext context)
    {
        gameIspaused = !gameIspaused;
        if (gameIspaused)
        {
            ActivateMenu();
        }
        else
        {
            DeactivateMenu();
        }

    }

    void ActivateMenu()
    {
        GamePaused();
        pauseMenuUI.SetActive(true);
    }


    public void DeactivateMenu()
    {
        GameStart();
        pauseMenuUI.SetActive(false);
        gameIspaused = false;
    }

    private void OnEnable()
    {
        menu = playerInput.Menu.Pause;
        menu.Enable();

        menu.performed += Pause;
    }

    private void OnDisable()
    {
        menu.Disable();
    }

    public static void GamePaused()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
    }

    public static void GameStart()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
    }
}
