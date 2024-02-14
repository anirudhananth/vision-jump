using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Game : MonoBehaviour
{
    public static Game obj;
    public static float highScore;
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
    public bool GameCanRestart;
    public bool GameHasStarted;
    public bool GameIsPaused;
    public int MaxPlatformCount;
    public float MaxCooldown;
    public Camera Camera;
    public Animator MenuAnimator;
    public AudioClip ClickAudio;
    public TextMeshProUGUI GameOverText;
    [SerializeField] TMPro.TextMeshProUGUI scoreText;

    [Header("Game Pre-assigned Data")]
    public GameCam GameCam;
    public GameObject Player;
    public PlatformPlacer Placer;
    public AudioSource AudioSource;


    public void Reset()
    {
        score = 0;
        obj = this;
        CamSpeed = InitCamSpeed;
        GameIsOver = false;
        GameCanRestart = false;
        GameHasStarted = false;
        GameIsPaused = false;
        MaxPlatformCount = InitMaxPlatformCount;
        MaxCooldown = InitMaxCooldown;
    }

    private void Start()
    {
        Reset();
        Camera = obj.GameCam.GetComponent<Camera>();
        AudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!GameHasStarted && Input.GetKeyUp(KeyCode.Space))
        {
            GameHasStarted = true;
            Utils.Play(AudioSource, ClickAudio);
            MenuAnimator.SetTrigger("GameStart");
        }

        if (GameHasStarted && !GameIsOver && Input.GetKeyUp(KeyCode.Escape))
        {
            Utils.Play(AudioSource, ClickAudio);
            MenuAnimator.SetTrigger(GameIsPaused ? "Unpause" : "Pause");
            GameIsPaused = !GameIsPaused;
            Time.timeScale = GameIsPaused ? 0 : 1;
        }

        if (GameIsPaused) return;

        if (GameCanRestart && Input.GetKeyUp(KeyCode.Space))
        {
            Utils.Play(AudioSource, ClickAudio);
            MenuAnimator.SetTrigger("GameStart");
            LoadScene("MainScene");
        }

        if (!GameHasStarted) return;

        if (!GameIsOver)
        {
            CamSpeed += CamAcceleration * Time.fixedDeltaTime;
            score += ScoreMultiplier * Time.fixedDeltaTime;
        }
        else
        {
            CamSpeed -= CamDeceleration * Time.fixedDeltaTime;
            CamSpeed = Mathf.Max(CamSpeed, 0);
        }
        if (score > highScore) { highScore = score; }
        scoreText.text = $"Score {score.ToString("0")}\n(High {highScore.ToString("0")})";
    }

    public void EndGame()
    {
        MenuAnimator.SetTrigger("GameOver");
        GameIsOver = true;
        Time.timeScale = 1.0f;
        GameOverText.text = $"High\t{(int)highScore}\r\nScore\t{(int)score}\r\n\r\n[Space] Restart";

        StartCoroutine(EndScene());
    }

    private IEnumerator EndScene()
    {
        yield return new WaitForSeconds(0.5f);
        GameCanRestart = true;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
