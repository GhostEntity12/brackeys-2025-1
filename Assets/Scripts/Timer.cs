using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
	private float timerLength;
	private float timeRemaining;
	private RectTransform rectTransform;

	[SerializeField] Image timerFront;
	[SerializeField] Gradient colorRange;
	public bool Active { get; private set; } = false;
	public bool Failed { get; private set; } = false;

	private void Awake()
	{
		// Setting values
		rectTransform = GetComponent<RectTransform>();
	}

	private void Update()
	{
		if (!Active) return;

		// Decrement time and update graphic
		timeRemaining -= Time.deltaTime;
		float percentage = Mathf.Clamp01(timeRemaining / timerLength);
		timerFront.fillAmount = percentage;
		timerFront.color = colorRange.Evaluate(percentage);

		// Timer hits zero
		if (timeRemaining <= 0)
		{
			// Failed
			Failed = true;
			return;
		}
	}

	public void Setup(float time, Transform location)
	{
		timerLength = time;
		rectTransform.position = Camera.main.WorldToScreenPoint(location.position) + (Vector3.up * 40);
	}

	public void Trigger()
	{
		Active = true;
		timeRemaining = timerLength + 0.2f;
		// Play appear animation
		LeanTween.scale(rectTransform, Vector3.one, 0.2f).setEaseOutBack();
	}

	public void Pause()
	{
		Active = false;
	}

	public void Finish()
	{
		// Play disappear animation
		LeanTween.scale(rectTransform, Vector3.zero, 0.2f).setEaseInBack();
	}
}
