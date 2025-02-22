using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class Stardew : MonoBehaviour, IStageEventCondition, IResettable
{
	enum MoveDir { xPos, yPos, xNeg, yNeg }
	Vector3 startPos;
	float timer = 0;
	float timeRequired = 5f;

	public bool Complete { get; set; }

	[SerializeField] RectTransform bobber;
	[SerializeField] RectTransform fish;
	[SerializeField] float moveSpeed = 20f;
	[SerializeField] float matchSensitivity = 10f;
	[SerializeField] MoveDir moveDir = MoveDir.yNeg;
	private void Awake()
	{
		startPos = bobber.localPosition;
	}

	private void Update()
	{
		if (!Complete)
		{
			// Move the "bobber"
			float moveAmount = moveSpeed * Time.deltaTime;
			if (Input.GetMouseButton(0))
			{
				bobber.localPosition = moveDir switch
				{
					MoveDir.xPos => new(bobber.localPosition.x - moveAmount, bobber.localPosition.y),
					MoveDir.yPos => new(bobber.localPosition.x, bobber.localPosition.y - moveAmount),
					MoveDir.xNeg => new(bobber.localPosition.x + moveAmount, bobber.localPosition.y),
					MoveDir.yNeg => new(bobber.localPosition.x, bobber.localPosition.y + moveAmount),
					_ => throw new System.NotImplementedException()
				};
			}
			else
			{
				bobber.localPosition = moveDir switch
				{
					MoveDir.xPos => new(bobber.localPosition.x + moveAmount, bobber.localPosition.y),
					MoveDir.yPos => new(bobber.localPosition.x, bobber.localPosition.y + moveAmount),
					MoveDir.xNeg => new(bobber.localPosition.x - moveAmount, bobber.localPosition.y),
					MoveDir.yNeg => new(bobber.localPosition.x, bobber.localPosition.y - moveAmount),
					_ => throw new System.NotImplementedException()
				};
			}

			if (Mathf.Abs(Vector3.Distance(fish.anchoredPosition, bobber.anchoredPosition)) < (moveDir == MoveDir.xPos || moveDir == MoveDir.xNeg ? fish.sizeDelta.x : fish.sizeDelta.y) / 2)
			{
				timer += Time.deltaTime;
			}
			else
			{
				timer -= Time.deltaTime;
			}
			timer = Mathf.Clamp(timer, 0f, timeRequired);

			if (timer == timeRequired)
			{
				Complete = true;
			}
		}
	}

	void IResettable.ResetObject()
	{
		Complete = false;
		bobber.localPosition = startPos;
	}
}
