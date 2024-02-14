using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public bool IsPlaced = false;
    public Color PreviewColor;
    public Color OverlapColor;
    public Color NormalColor;
    public GameObject NoMorePlatformSprite;

    public GameObject[] Platforms;
    public GameObject DefensiveBeam;
    public PlatformType PlatformType;

    private SpriteRenderer activePlatformRenderer;
    private Collider2D activePlatformCollider;

    public bool HasOverlap => activePlatformCollider.IsTouchingLayers(Game.obj.Placer.OverlappingLayers);

    public bool TryPlace()
    {
        if (IsPlaced == true) {
            Debug.LogWarning("Repeated placement of platform");
            return false;
        }

        if (HasOverlap) return false;

        IsPlaced = true;
        DefensiveBeam.SetActive(true);
        activePlatformCollider.isTrigger = false;
        activePlatformRenderer.color = NormalColor;
        GetComponentInChildren<ParticleSystem>().Play();
        return true;
    }

    private void Start()
    {
        activePlatformRenderer = Platforms[0].GetComponent<SpriteRenderer>();
        activePlatformCollider = Platforms[0].GetComponent<Collider2D>();
        if (IsPlaced) return;
        foreach (var p in Platforms)
        {
            p.GetComponent<Collider2D>().isTrigger = true;
        }
    }

    public void OnDestroy()
    {
        Game.obj.Placer.CurPlatformCount -= 1;
    }

    private void Update()
    {
        if (IsPlaced) return;
        var platformType = Game.obj.Placer.SelectedPlatformType;

        for (int i = 0; i < Platforms.Length; i++)
        {
            bool isActive = i == (int)platformType;
            Platforms[i].SetActive(isActive);
            if (isActive)
            {
                activePlatformRenderer = Platforms[i].GetComponent<SpriteRenderer>();
                activePlatformCollider = Platforms[i].GetComponent<Collider2D>();
            }
        }

        if (HasOverlap || !Game.obj.Placer.CanPlace)
        {
            activePlatformRenderer.color = OverlapColor;
            NoMorePlatformSprite.SetActive(!Game.obj.Placer.CanPlace);
        }
        else
        {
            NoMorePlatformSprite.SetActive(false);
            activePlatformRenderer.color = PreviewColor;
        }
    }
}
