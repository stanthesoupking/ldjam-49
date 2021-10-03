using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameControllerState
{
    Playing,
    GameOver
}

public class GameController : MonoBehaviour
{

    private GameControllerState state;

    public HeightTracker HeightTracker;

    public Text CurrentHeightText;

    public Color MovingTextColor;
    public Color StableTextColor;

    private float InitialHeight;

    public Transform GameOverExplosionPosition;
    public float ExplosionForce;
    public float ExplosionRadius;

    public Transform SpawnLocation;
    public float SpawnRadius;

    public int BlockCount = 10;

    public BuildingBlock[] blocks;

    public GameObject TemporaryColliders;

    public GameObject MainCamera;
    public GameObject SpawnCamera1;
    public GameObject SpawnCamera2;

    // Start is called before the first frame update
    void Start()
    {
        state = GameControllerState.Playing;

        InitialHeight = HeightTracker.transform.position.y;
    }

    void FixedUpdate()
    {
        bool needMoreBlocks = true;
        foreach(BuildingBlock block in FindObjectsOfType<BuildingBlock>())
        {
            if (!block.IsPlaced)
            {
                needMoreBlocks = false;
            }
        }

        if (needMoreBlocks) {
            SpawnMoreBlocks();
        }
    }

    // Update is called once per frame
    void Update()
    {
        float height = HeightTracker.transform.position.y - InitialHeight;
        CurrentHeightText.text = string.Format("{0}m", (int)height);

        CurrentHeightText.color = HeightTracker.IsMoving() ? MovingTextColor : StableTextColor;

        

    }

    public void GameOver()
    {
        if (state == GameControllerState.GameOver) {
            return;
        }

        state = GameControllerState.GameOver;

        Debug.Log("Game Over!");

        foreach(BuildingBlock block in FindObjectsOfType<BuildingBlock>())
        {
            Rigidbody body = block.GetComponent<Rigidbody>();
            body.AddExplosionForce(ExplosionForce, GameOverExplosionPosition.transform.position, ExplosionRadius);
        }
        
    }

    private void SpawnPart2()
    {
        SpawnCamera1.SetActive(false);
        SpawnCamera2.SetActive(true);
        Invoke("SpawnPart3", 10.0f);
    }

    private void SpawnPart3()
    {
        TemporaryColliders.SetActive(false);
        MainCamera.SetActive(true);
    }

    public void SpawnMoreBlocks()
    {
        Debug.Log("Spawning more blocks...");

        TemporaryColliders.SetActive(true);

        MainCamera.SetActive(false);
        SpawnCamera1.SetActive(true);
        
        for (int i = 0; i < BlockCount; ++i) {
            int index = Random.Range(0, blocks.Length);
            BuildingBlock block = Instantiate(blocks[index], SpawnLocation.position, Quaternion.identity, null);
        }

        Invoke("SpawnPart2", 5.0f);
    }
}
