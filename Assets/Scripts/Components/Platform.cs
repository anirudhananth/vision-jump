using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public bool IsPlaced = false;
    public Color PreviewColor;
    public Color OverlapColor;
    public Color NormalColor;

    public bool HasOverlap => Col.IsTouchingLayers(Game.obj.Placer.OverlappingLayers);

    public SpriteRenderer Renderer { get; private set; }
    public Collider2D Col { get; private set; }

    public bool TryPlace()
    {
        if (IsPlaced == true) {
            Debug.LogWarning("Repeated placement of platform");
            return false;
        }

        if (HasOverlap) return false;

        IsPlaced = true;
        Col.isTrigger = false;
        Renderer.color = NormalColor;
        return true;
    }

    public void OnDestroy()
    {
        Game.obj.Placer.CurPlatformCount -= 1;
    }

    private void Start()
    {
        Renderer = GetComponentInChildren<SpriteRenderer>();
        Col = GetComponentInChildren<Collider2D>();
        if (!IsPlaced)
        {
            Renderer.color = PreviewColor;
            Col.isTrigger = true;
        }
    }

    private void Update()
    {
        if (IsPlaced) return;

        if (HasOverlap || !Game.obj.Placer.CanPlace)
        {
            Renderer.color = OverlapColor;
        }
        else
        {
            Renderer.color = PreviewColor;
        }
    }
}
