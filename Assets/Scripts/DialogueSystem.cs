using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
	private readonly List<int> recentLineIndexes = new();
	[SerializeField] string[] lines =
	{
		"We need to find the magic ladybug",
		"You killed my brother!",
		"The poison acts quick but slow",
		"You're my best friend",
		"I can't live without you",
		"I will do... something!",
		"Oh look, a cat!",
		"I don't think you understand my genius",
		"I love you with all my heart",
		"I have powers beyond your belief",
		"I have to tell you something",
		"We can do this together!",
		"You cannot be allowed to live",
		"Don't let it break!",
		"Get over here!",
		"How dare you!"
	};

	[SerializeField] Vector2 timeBetweenLines = new(5, 8);
	[SerializeField] Vector2 timeLineHeld = new(3, 6);
	[SerializeField] TextMeshProUGUI dialogueLine;
	[SerializeField] RectTransform speechBubble;
	[SerializeField] RectTransform leftPersonArrow;
	[SerializeField] RectTransform rightPersonArrow;
	[SerializeField] Sprite leftPersonLowered, leftPersonRaised, rightPersonLowered, rightPersonRaised;
	[SerializeField] SpriteRenderer leftPersonRenderer;
	[SerializeField] SpriteRenderer rightPersonRenderer;
	float timer = 8;
	bool lineActive = false;

	bool leftSelected;

	void Update()
	{
		timer -= Time.deltaTime;
		if (timer <= 0)
		{
			if (lineActive)
			{
				// Disable line
				LeanTween.scale(speechBubble, Vector3.zero, 0.4f).setEaseInBack();
				timer = Random.Range(timeBetweenLines.x, timeBetweenLines.y);
				lineActive = false;
				if (leftSelected)
				{
					leftPersonRenderer.sprite = leftPersonLowered;
				}
				else
				{
					rightPersonRenderer.sprite = rightPersonLowered;
				}
			}
			else
			{
				// Show new line
				LeanTween.scale(speechBubble, Vector3.one, 0.4f).setEaseOutBack();
				dialogueLine.text = lines[GetNewDialogueIndex()];
				timer = Random.Range(timeLineHeld.x, timeLineHeld.y);
				lineActive = true;
				leftSelected = Random.value > 0.5;
				if (leftSelected)
				{
					leftPersonArrow.gameObject.SetActive(true);
					rightPersonArrow.gameObject.SetActive(false);
					leftPersonRenderer.sprite = leftPersonRaised;
				}
				else
				{
					leftPersonArrow.gameObject.SetActive(false);
					rightPersonArrow.gameObject.SetActive(true);
					rightPersonRenderer.sprite = rightPersonRaised;
				}
			}
		}
	}

	int GetNewDialogueIndex()
	{
		int index = Random.Range(0, lines.Length);
		while (recentLineIndexes.Contains(index))
		{
			index++;
			if (index == lines.Length)
			{
				index = 0;
			}
		}
		recentLineIndexes.Add(index);
		if (recentLineIndexes.Count > 5)
		{
			recentLineIndexes.RemoveAt(0);
		}
		return index;
	}
}
