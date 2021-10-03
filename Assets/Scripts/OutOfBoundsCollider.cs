using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBoundsCollider : MonoBehaviour
{

    public GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        BuildingBlock block = other.GetComponent<BuildingBlock>();
        if (block) {
            gameController.GameOver();
        }
    }
}
