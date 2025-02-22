using UnityEngine;
using UnityEngine.UI;

public class Timing : MonoBehaviour, IStageEventCondition, IResettable
{
	private bool movingForward = true;
	private bool active = false;
	private float delta = 0f;

	float maxDistance;
	[SerializeField] RectTransform maxRange;
	[SerializeField] RectTransform safeZone;
	[SerializeField] RectTransform bouncer;
	[SerializeField] private float moveSpeed = 10f;
	[SerializeField] private bool movingX = true;

	public bool Complete { get; set; } = false;

	public void ResetObject()
	{
		Complete = false;
		active = true;
		maxDistance = (movingX ? maxRange.sizeDelta.x : maxRange.sizeDelta.y) / 2;
		bouncer.anchoredPosition = movingX
			? new(Random.Range(-maxDistance, maxDistance), bouncer.anchoredPosition.y)
			: new(bouncer.anchoredPosition.x, Random.Range(-maxDistance, maxDistance));
		float buffer = (movingX ? safeZone.sizeDelta.x : safeZone.sizeDelta.y) / 2f;
		safeZone.anchoredPosition = movingX
			? new(Random.Range(-maxDistance + buffer, maxDistance - buffer), bouncer.anchoredPosition.y)
			: new(bouncer.anchoredPosition.x, Random.Range(--maxDistance + buffer, maxDistance - buffer));
	}

	void Update()
	{
		if (!Complete && active)
		{
			delta = movingX ? bouncer.anchoredPosition.x : bouncer.anchoredPosition.y;
			bouncer.anchoredPosition += (movingForward ? 1 : -1) * moveSpeed * Time.deltaTime * (movingX ? Vector2.right: Vector2.up);
			if (delta > maxDistance)
			{
				movingForward = false;
			}
			else if (delta < -maxDistance)
			{
				movingForward = true;
			}

			bouncer.GetComponent<Image>().color = (Mathf.Abs(Vector3.Distance(safeZone.anchoredPosition, bouncer.anchoredPosition)) < (movingX ? safeZone.sizeDelta.x : safeZone.sizeDelta.y) / 2) ? Color.green : Color.red;

			if (Input.GetMouseButtonDown(0) && Mathf.Abs(Vector3.Distance(safeZone.anchoredPosition, bouncer.anchoredPosition)) < (movingX ? safeZone.sizeDelta.x : safeZone.sizeDelta.y) / 2) 
			{
				Complete = true;
			}
		}
	}
}
