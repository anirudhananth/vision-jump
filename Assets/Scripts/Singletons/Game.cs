using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Game : MonoBehaviour
{
    public static Game obj;

    [Header("Game Fixed Data")]
    public const int InitMaxPlatformCount = 4;
    public const float InitCamSpeed = 2f;
    public const float InitMaxCooldown = 4f;
    public const float CamAcceleration = 0.02f;
    public const float DeathYPosition = 0f;

    [Header("Game Dynamic Data")]
    public float CamSpeed;
    public bool GameIsOver;
    public bool GameIsReady;
    public int MaxPlatformCount;
    public float MaxCooldown;
    public Camera Camera;

    [Header("Game Pre-assigned Data")]
    public GameCam GameCam;
    public GameObject Player;
    public PlatformPlacer Placer;


    public void Reset()
    {
        obj = this;
        CamSpeed = InitCamSpeed;
        GameIsOver = false;
        GameIsReady = false;
        MaxPlatformCount = InitMaxPlatformCount;
        MaxCooldown = InitMaxCooldown;
    }

    private void Start()
    {
        Reset();
        Camera = obj.GameCam.GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
        CamSpeed += CamAcceleration * Time.fixedDeltaTime;
    }
}
