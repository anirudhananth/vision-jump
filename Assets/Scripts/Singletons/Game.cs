using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Game : MonoBehaviour
{
    public static Game obj;
    private float score;

    [Header("Game Fixed Data")]
    public const int InitMaxPlatformCount = 4;
    public const float InitCamSpeed = 2f;
    public const float InitMaxCooldown = 4f;
    public const float CamAcceleration = 0.02f;
    public const float CamDeceleration = 2.5f;
    public const float ScoreMultiplier = 1.5f;
    public const float DeathYPosition = 0f;

    [Header("Game Dynamic Data")]
    public float CamSpeed;
    public bool GameIsOver;
    public bool GameIsReady;
    public int MaxPlatformCount;
    public float MaxCooldown;
    public Camera Camera;
    [SerializeField] TMPro.TextMeshProUGUI scoreText;

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
        if(!GameIsOver) {
            CamSpeed += CamAcceleration * Time.fixedDeltaTime;
            score += ScoreMultiplier * Time.fixedDeltaTime;
        } else {
            CamSpeed -= CamDeceleration * Time.fixedDeltaTime;
            CamSpeed = Mathf.Max(CamSpeed, 0);
        }
        scoreText.text = score.ToString("0");
    }

    public void EndGame() {
        GameIsOver = true;
        StartCoroutine(EndScene());
    }

    private IEnumerator EndScene() {
        yield return new WaitForSeconds(2);
        LoadScene("EndScene");
    }

    public void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
}
