using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    private bool gameIspaused;
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
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }
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
        Time.timeScale = 0;
        AudioListener.pause = true;
        pauseMenuUI.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    public void DeactivateMenu()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        pauseMenuUI.SetActive(false);
        gameIspaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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

}
