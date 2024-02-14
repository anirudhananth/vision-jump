using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCam : MonoBehaviour
{
    private float CamSpeed => Game.obj.CamSpeed;

    private void FixedUpdate()
    {
        if (!Game.obj.GameHasStarted) return;

        transform.Translate(CamSpeed * Time.fixedDeltaTime * Vector3.up);
    }
}
