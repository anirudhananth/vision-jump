using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlatformPlacer : MonoBehaviour
{
    [Header("Placer Dynamic Data")]
    public int CurPlatformCount;
    public float CurCooldown;

    [Header("Placer Pre-assigned Data")]
    public GameObject PlatformPrefab;
    public LayerMask OverlappingLayers;

    public bool CanPlace => CurCooldown == 0 && CurPlatformCount < Game.obj.MaxPlatformCount;

    private Platform CandidatePlatform;

    public void Reset()
    {
        CurCooldown = 0;
        CurPlatformCount = Game.obj.MaxPlatformCount;
    }

    private void Update()
    {
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
                CurPlatformCount++;
                CandidatePlatform = null;
            }
        }
    }
}
