using UnityEngine;
using UnityEngine.UI;

public class ButtonSpriteSwap : MonoBehaviour
{
	[SerializeField] Image image;
	[SerializeField] Sprite onSprite;
	[SerializeField] Sprite offSprite;

	public void SetOn()
	{
		image.sprite = onSprite;
	}

	public void SetOff()
	{
		image.sprite = offSprite;
	}
}
