using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameControllerState
{
    Playing,
    GameOver
}

public class GameController : MonoBehaviour
{
    private bool WaitingToPlay;
    public int ResupplyIncrement;
    public int ResupplyCount;

    private GameControllerState state;

    public ParticleSystem Explosion;

    public CraneController Crane;

    public HeightTracker HeightTracker;

    public Text CurrentHeightText;
    public Text RemainingBlocksText;
    public Text FailText;

    public Text ResupplyText;

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

    public Material[] materials;

    public GameObject TemporaryColliders;

    public GameObject MainCamera;
    public GameObject SpawnCamera1;
    public GameObject SpawnCamera2;

    private Vector3 InitialHeightTrackerPosition;

    // Start is called before the first frame update
    void Start()
    {
        FailText.color = new Color(1.0f, 0, 0, 0);

        state = GameControllerState.Playing;

        InitialHeight = HeightTracker.transform.position.y;

        InitialHeightTrackerPosition = HeightTracker.transform.position;
    }

    void FixedUpdate()
    {
        float height = HeightTracker.transform.position.y - InitialHeight;
        CurrentHeightText.text = string.Format("{0}m", (int)height);

        CurrentHeightText.color = HeightTracker.IsMoving() ? MovingTextColor : StableTextColor;

        bool unstableBlocks = false;
        int notPlacedCount = 0;
        bool needMoreBlocks = true;
        foreach(BuildingBlock block in FindObjectsOfType<BuildingBlock>())
        {
            if (!block.IsPlaced) {
                notPlacedCount++;
            }

            if (!block.IsRoughlyStable()) {
                unstableBlocks = true;
            }

            if (!block.IsPlaced || (block.IsPlaced && !block.IsStable()))
            {
                needMoreBlocks = false;
            }
        }

        RemainingBlocksText.text = string.Format("{0} Rooms Until Resupply", notPlacedCount);

        ResupplyText.text = string.Format("Resupply #{0}", ResupplyCount);

        if (needMoreBlocks) {
            SpawnMoreBlocks();
        }

        if (WaitingToPlay) {
            if (!unstableBlocks) {
                SpawnPart3();
                WaitingToPlay = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        

    }

    private void ResetLevel()
    {
        ResupplyCount = 0;

        state = GameControllerState.Playing;

        foreach(BuildingBlock block in FindObjectsOfType<BuildingBlock>()) {
            Destroy(block.gameObject);
        }

        Crane.Reset();

        Explosion.gameObject.SetActive(false);
        Explosion.Stop();

        HeightTracker.transform.position = InitialHeightTrackerPosition;

        FailText.color = new Color(1.0f, 0, 0, 0.0f);
    }

    public void GameOver()
    {
        if (state == GameControllerState.GameOver) {
            return;
        }

        Crane.Enabled = false;

        state = GameControllerState.GameOver;

        Debug.Log("Game Over!");

        Explosion.gameObject.SetActive(true);
        Explosion.Play();

        FailText.color = new Color(1.0f, 0, 0, 1.0f);

        foreach(BuildingBlock block in FindObjectsOfType<BuildingBlock>())
        {
            Rigidbody body = block.GetComponent<Rigidbody>();
            body.AddExplosionForce(ExplosionForce, GameOverExplosionPosition.transform.position, ExplosionRadius);
        }
        
        Invoke("ResetLevel", 5.0f);
    }

    private void SpawnPart2()
    {
        WaitingToPlay = true;
        SpawnCamera1.SetActive(false);
        SpawnCamera2.SetActive(true);
    }

    private void SpawnPart3()
    {
        TemporaryColliders.SetActive(false);
        MainCamera.SetActive(true);
        SpawnCamera2.SetActive(false);
        Crane.Enabled = true;
    }

    public void SpawnMoreBlocks()
    {
        Debug.Log("Spawning more blocks...");

        ResupplyCount++;

        Crane.Enabled = false;

        TemporaryColliders.SetActive(true);

        MainCamera.SetActive(false);
        SpawnCamera1.SetActive(true);
        
        for (int i = 0; i < ResupplyCount * ResupplyIncrement; ++i) {
            int index = Random.Range(0, blocks.Length);
            BuildingBlock block = Instantiate(blocks[index], SpawnLocation.position, Quaternion.identity, null);
            block.GetComponent<MeshRenderer>().material = materials[Random.Range(0, materials.Length)];
        }

        Invoke("SpawnPart2", 5.0f);
    }
}
