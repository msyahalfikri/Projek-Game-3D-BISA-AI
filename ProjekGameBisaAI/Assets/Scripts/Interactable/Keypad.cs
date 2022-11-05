using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keypad : Interactable
{
    // Start is called before the first frame update
    [SerializeField] private GameObject door;
    private PlayerHealth playerHealth;
    private bool doorOpen;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    protected override void Interact()
    {
        doorOpen = !doorOpen;
        door.GetComponent<Animator>().SetBool("isOpen", doorOpen);


    }
}
