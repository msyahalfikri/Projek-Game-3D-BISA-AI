using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform respawnPoint;
    public static bool isDead = false;

    private void Start()
    {

    }
    private void Update()
    {
        // Respawn();
    }
    public void Respawn()
    {
        // if (isDead)
        // {
        player.SetActive(false);
        player.transform.position = respawnPoint.transform.position;
        player.SetActive(true);
        isDead = false;
        // }
    }
}
