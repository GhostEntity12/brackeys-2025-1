using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
	[SerializeField] Image timerFront;
	[SerializeField] Gradient colorRange;

	public bool Active { get; private set; } = false;

	private RectTransform rectTransform;
	private float timerLength;
	private float timeRemaining;

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
			return;
		}
	}

	public void Setup(float time, Transform location)
	{
		timerLength = time;
		GameManager.Instance.RegisterTimer(this);
		rectTransform.position = Camera.main.WorldToScreenPoint(location.position) + (Vector3.up * 10);
	}

	public void Trigger()
	{
		Active = true;
		timeRemaining = timerLength;
		// Play appear animation
	}

	public void Pause()
	{
		Active = false;
	}

	public void Finish()
	{
		Active = false;
		// Play disappear animation
	}
}
