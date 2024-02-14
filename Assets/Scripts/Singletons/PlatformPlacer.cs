using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public enum PlatformType
{
    NORMAL, BOUNCY, SAFE, DEFENSIVE
}

public class PlatformPlacer : MonoBehaviour
{
    [Header("Placer Dynamic Data")]
    public int CurPlatformCount = 1;
    public float CurCooldown;
    public PlatformType SelectedPlatformType = PlatformType.NORMAL;

    [Header("Placer Pre-assigned Data")]
    public GameObject PlatformPrefab;
    public LayerMask OverlappingLayers;
    public AudioClip placeSuccessAudio;
    public AudioClip placeFailAudio;
    public TextMeshProUGUI PlatformText;

    public bool CanPlace => CurCooldown == 0 && CurPlatformCount < Game.obj.MaxPlatformCount;
    public PlatformIcon[] PlatformIcons;

    private Platform CandidatePlatform;
    private AudioSource audioSource;

    public void Reset()
    {
        CurCooldown = 0;
        CurPlatformCount = Game.obj.MaxPlatformCount;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        PlatformText.text = $"# platforms: {CurPlatformCount}/{Game.obj.MaxPlatformCount}";
        if (!Game.obj.GameHasStarted || Game.obj.GameIsPaused) return;

        CurCooldown = Mathf.Clamp(CurCooldown - Time.deltaTime, 0, Game.obj.MaxCooldown);
        Vector2 mousePos = Game.obj.Camera.ScreenToWorldPoint(Input.mousePosition);

        if (CandidatePlatform != null)
        {
            CandidatePlatform.gameObject.SetActive(CurCooldown == 0);
        }
        else
        {
            CandidatePlatform = Instantiate(PlatformPrefab, mousePos, Quaternion.identity).GetComponent<Platform>();
        }
        CandidatePlatform.transform.position = mousePos;

        if (CanPlace && Input.GetMouseButtonDown(0))
        {
            if (CandidatePlatform.TryPlace())
            {
                Utils.Play(audioSource, placeSuccessAudio);
                CurPlatformCount++;
                CandidatePlatform = null;
            }
            else
            {
                Utils.Play(audioSource, placeFailAudio);
            }
        }

        List<PlatformIcon> unlocked = PlatformIcons.Where((f) => f.isUnlocked).ToList();
        int selected = unlocked.FindIndex((f) => f.Type == SelectedPlatformType);

        if (Input.mouseScrollDelta.y > 0) {
            selected -= 1;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            selected += 1;
        }
        selected += unlocked.Count;

        SelectedPlatformType = unlocked.Count > 0 ? unlocked[selected % unlocked.Count].Type : PlatformType.NORMAL;
    }
}
