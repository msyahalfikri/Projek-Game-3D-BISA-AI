using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI promptText;

    public GameObject DeathScreenUI;
    public GameObject WinScreenUI;
    // Start is called before the first frame update
    void Start()
    {
    }
    private void Update()
    {
        showDeathScreen();
        showWinScreen();
        if (PauseMenu.gameIspaused || RespawnPlayer.isDead || DemoGameplayWin.isWin)
        {
            PauseMenu.GamePaused();
            LockPointer(false);
        }
        else
        {
            PauseMenu.GameStart();
            LockPointer(true);
        }
    }
    // Update is called once per frame
    public void UpdateText(string promptMessage)
    {
        promptText.text = promptMessage;
    }

    public void showDeathScreen()
    {
        if (RespawnPlayer.isDead)
        {
            DeathScreenUI.SetActive(true);
        }
        else
        {
            DeathScreenUI.SetActive(false);
        }
    }

    public static void LockPointer(bool state)
    {
        if (state == false)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (state == true)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void showWinScreen()
    {
        if (DemoGameplayWin.isWin)
        {
            WinScreenUI.SetActive(true);
        }
        else
        {
            WinScreenUI.SetActive(false);
        }
    }

}
