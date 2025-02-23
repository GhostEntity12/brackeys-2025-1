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

	float timer;
	bool lineActive = false;

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
			}
			else
			{
				// Show new line
				LeanTween.scale(speechBubble, Vector3.one, 0.4f).setEaseOutBack();
				dialogueLine.text = lines[GetNewDialogueIndex()];
				timer = Random.Range(timeLineHeld.x, timeLineHeld.y);
				lineActive = true;
			}
		}
	}

	int GetNewDialogueIndex()
	{
		int index = Random.Range(0, lines.Length);
		while (recentLineIndexes.Contains(index))
		{
			index++;
		}
		recentLineIndexes.Add(index);
		if (recentLineIndexes.Count > 5)
		{
			recentLineIndexes.RemoveAt(0);
		}
		return index;
	}
}
