using UnityEngine;
using UnityEngine.UI;

public class Stardew : MonoBehaviour, IStageEventCondition, IResettable
{
	enum MoveDir { xPos, yPos, xNeg, yNeg }
	float timer = 0;
	float maxDelta;
	float paddedDelta;

	public bool Complete { get; set; }

	[SerializeField] RectTransform bobber;
	[SerializeField] RectTransform fish;
	[SerializeField] RectTransform maxRange;
	[SerializeField] Image progress;
	[SerializeField] float timeRequired = 5f;
	[SerializeField] float moveSpeed = 20f;
	[SerializeField] bool movesUp = true;

	private void Awake()
	{
		maxDelta = (movesUp ? maxRange.sizeDelta.y : maxRange.sizeDelta.x) / 2;
		paddedDelta = maxDelta - ((movesUp? fish.sizeDelta.y : fish.sizeDelta.x) / 2);
	}

	private void Update()
	{
		if (!Complete)
		{
			// Move the "bobber"
			float moveAmount = moveSpeed * Time.deltaTime;
			if (Input.GetMouseButton(0))
			{
				bobber.localPosition = movesUp
					? new(bobber.localPosition.x, Mathf.Clamp(bobber.localPosition.y, -maxDelta, maxDelta) + moveAmount)
					: new(Mathf.Clamp(bobber.localPosition.x, -maxDelta, maxDelta) + moveAmount, bobber.localPosition.y);
			}
			else
			{
				bobber.localPosition = movesUp
					? new(bobber.localPosition.x, Mathf.Clamp(bobber.localPosition.y, -maxDelta, maxDelta) - moveAmount)
					: new(Mathf.Clamp(bobber.localPosition.x, -maxDelta, maxDelta) - moveAmount, bobber.localPosition.y);
			}

			bool inRange = Mathf.Abs(Vector3.Distance(fish.anchoredPosition, bobber.anchoredPosition)) < (movesUp ? fish.sizeDelta.y : fish.sizeDelta.x) / 2;
			if (inRange)
			{
				timer += Time.deltaTime;
			}
			else
			{
				timer -= Time.deltaTime;
			}
			timer = Mathf.Clamp(timer, 0f, timeRequired);
			progress.fillAmount = timer / timeRequired;

			if (timer == timeRequired)
			{
				Complete = true;
			}
		}
	}

	void IResettable.ResetObject()
	{
		Complete = false;
		bobber.localPosition = movesUp ? new(bobber.localPosition.x, -maxDelta) : new(-maxDelta, bobber.localPosition.y);
		fish.localPosition = movesUp ? new(fish.localPosition.x, Random.Range(-paddedDelta, paddedDelta)) : new(Random.Range(-paddedDelta, paddedDelta), fish.localPosition.y);
		timer = 0f;
	}
}
