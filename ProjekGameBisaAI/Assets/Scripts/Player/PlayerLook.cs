using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera cam;
    private float xRotation = 0f;
    public float xSens = 30f;
    public float ySens = 30f;

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;
        //Mengkalkulasi rotasi kamera untuk melihat keatas dan kebawah
        xRotation -= (mouseY * Time.deltaTime) * ySens;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        //Mengpalikasikan ke transform camera
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        //merotasi player untuk melihat ke kiri dan kekanan
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSens);
    }
    private void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
