using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    public PlayerController Player;

    public float Angle;

    public float Pitch;

    public float Distance = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 rotation = new Vector3()
        //transform.position = Player.transform.position + ;
        //transform.position = Player.transform.position + (Angle.normalized * Distance);
        //transform.rotation = Quaternion.LookRotation(-Angle.normalized, Vector3.up);
    }
}
