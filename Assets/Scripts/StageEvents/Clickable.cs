using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clickable : MonoBehaviour, IStageEventCondition, IResettable
{
	private int clicks = 0;

	[SerializeField] Button b;
	[SerializeField] private List<Sprite> sprites;
	[SerializeField] private Image image;
	[SerializeField] private int requiredClicks = 1;

	public bool Complete { get; set; } = false;


	public void OnClick()
	{
		clicks++;
		if (sprites.Count > 0)
		{
			Debug.Log(Mathf.Min(clicks, sprites.Count));
			image.sprite = sprites[Mathf.Min(clicks, sprites.Count - 1)];
		}

		if (clicks >= requiredClicks)
		{
			Complete = true;
			b.enabled = false;
		}
	}

	public void ResetObject()
	{
		clicks = 0;
		Complete = false;
		b.enabled = true;
		if (sprites.Count > 0)
		{
			image.sprite = sprites[0];
		}
	}
}
