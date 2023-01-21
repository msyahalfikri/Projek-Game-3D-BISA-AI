using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnPlayer : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform respawnPoint;
    private PlayerUI playerUI;
    public static bool isDead = false;

    private void Start()
    {
        playerUI = GetComponent<PlayerUI>();
    }
    private void Update()
    {
        // Respawn();
    }
    public void Respawn()
    {

        // player.SetActive(false);
        // player.transform.position = respawnPoint.transform.position;
        // player.SetActive(true);  
        isDead = false;
        DemoGameplayWin.isWin = false;
        SceneManager.LoadScene(1);
    }
}
