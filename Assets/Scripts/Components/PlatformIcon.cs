using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlatformIcon : MonoBehaviour
{
    public float UnlockScore;
    public string Name;
    public PlatformType Type;
    public Sprite NotSelectedSprite;
    public Sprite SelectedSprite;

    public bool isUnlocked = false;
    private Image image;
    public Image ChildImage;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        ChildImage.enabled = !isUnlocked;
        if (!isUnlocked && Game.highScore > UnlockScore)
        {
            isUnlocked = true;
            ChildImage.enabled = false;
        }

        if (Game.obj.Placer.SelectedPlatformType == Type)
        {
            image.sprite = SelectedSprite;
        }
        else
        {
            image.sprite = NotSelectedSprite;
        }
    }
}
