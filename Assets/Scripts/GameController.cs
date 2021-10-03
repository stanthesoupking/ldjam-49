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

    // Start is called before the first frame update
    void Start()
    {
        state = GameControllerState.Playing;

        InitialHeight = HeightTracker.transform.position.y;
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
        Debug.Log("Game Over!");

        
    }
}
