using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBlock : MonoBehaviour
{

    public static int PhysicsLayer() {
        return LayerMask.GetMask("Block");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
