using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{

    public float HookRange = 2.0f;

    public Light Light;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float closestDistance = float.PositiveInfinity;
        BuildingBlock closestBlock = null;
        foreach(BuildingBlock block in FindObjectsOfType<BuildingBlock>()) {
            float distance = Vector3.Distance(block.transform.position, transform.position);
            if (distance < closestDistance) {
                closestDistance = distance;
                closestBlock = block;
            }
        }

        bool isInRange = (closestDistance < HookRange);
        
        Debug.Log("Closest block = " + closestDistance);
        Light.color = isInRange ? Color.green : Color.red;
    }
}
