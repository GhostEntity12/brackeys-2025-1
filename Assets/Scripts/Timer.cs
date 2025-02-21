using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
	private bool active = false;
	private float timerLength;
	private float timeRemaining;
	private RectTransform rectTransform;

	[SerializeField] Image timerFront;
	[SerializeField] Gradient colorRange;

	private void Awake()
	{
		// Setting values
		rectTransform = GetComponent<RectTransform>();
	}

	private void Update()
	{
		if (!active) return;

		// Decrement time and update graphic
		timeRemaining -= Time.deltaTime;
		float percentage = Mathf.Clamp01(timeRemaining / timerLength);
		timerFront.fillAmount = percentage;
		timerFront.color = colorRange.Evaluate(percentage);

		// Timer hits zero
		if (timeRemaining <= 0)
		{
			// Failed
			return;
		}
	}

	public void Setup(float time, Transform location)
	{
		timerLength = time;
		rectTransform.position = Camera.main.WorldToScreenPoint(location.position) + (Vector3.up * 10);
	}

	public void Trigger()
	{
		active = true;
		timeRemaining = timerLength;
		// Play appear animation
	}

	public void Pause()
	{
		active = false;
	}

	public void Finish()
	{
		// Play disappear animation
	}
}
