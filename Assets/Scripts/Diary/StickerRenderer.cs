using UnityEngine;
using UnityEngine.UI;

public class StickerRenderer : MonoBehaviour
{
    public Sprite[] Stickers;
    public Image image;

    public void SetSticker(int stickerId)
    {
        image.sprite = Stickers[stickerId - 1];
    }

    public void SetEmpty()
    {
        Color transparentColor = image.color;
        transparentColor.a = 0f;
        image.color = transparentColor;
    }
}
