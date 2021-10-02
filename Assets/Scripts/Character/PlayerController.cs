using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private CharacterController Controller;
    public PlayerCamera Camera;

    public float Speed = 10.0f;

    void Start()
    {
        Controller = (CharacterController)GetComponent("CharacterController");
    }

    void Update()
    {
        Vector2 thumbstick = new Vector2(-Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Vector3 movement = new Vector3(thumbstick.y, 0, thumbstick.x) * Speed * Time.deltaTime;

        // Rotate movement to align with camera
        Vector3 r = Camera.transform.rotation.eulerAngles.z;
        Debug.Log("");

        transform.rotation = Quaternion.LookRotation(movement);

        Controller.Move(movement);
    }
}
