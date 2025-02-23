using UnityEngine;

public class Holdable : MonoBehaviour, IStageEventCondition, IResettable
{
	enum MoveDir { xPos, yPos, xNeg, yNeg }
	Vector3 startPos;

	public bool Complete { get; set; }

	[SerializeField] RectTransform movingObject;
	[SerializeField] RectTransform matchObject;
	[SerializeField] float moveSpeed = 20f;
	[SerializeField] float matchSensitivity = 10f;
	[SerializeField] MoveDir moveDir = MoveDir.yNeg;
	private void Awake()
	{
		startPos = movingObject.anchoredPosition;
	}

	private void Update()
	{
		if (!Complete)
		{
			float moveAmount = moveSpeed * Time.deltaTime;
			if (Input.GetMouseButton(0))
			{
				movingObject.localPosition = moveDir switch
				{
					MoveDir.xPos => new(movingObject.localPosition.x + moveAmount, movingObject.localPosition.y),
					MoveDir.yPos => new(movingObject.localPosition.x, movingObject.localPosition.y + moveAmount),
					MoveDir.xNeg => new(movingObject.localPosition.x - moveAmount, movingObject.localPosition.y),
					MoveDir.yNeg => new(movingObject.localPosition.x, movingObject.localPosition.y - moveAmount),
					_ => throw new System.NotImplementedException()
				};
			}
			else if (Input.GetMouseButtonUp(0) && Vector3.Distance(movingObject.localPosition, matchObject.localPosition) < matchSensitivity)
			{
				Complete = true;
			}
			else
			{
				movingObject.localPosition = moveDir switch
				{
					MoveDir.xPos => new(movingObject.localPosition.x - moveAmount, movingObject.localPosition.y),
					MoveDir.yPos => new(movingObject.localPosition.x, movingObject.localPosition.y - moveAmount),
					MoveDir.xNeg => new(movingObject.localPosition.x + moveAmount, movingObject.localPosition.y),
					MoveDir.yNeg => new(movingObject.localPosition.x, movingObject.localPosition.y + moveAmount),
					_ => throw new System.NotImplementedException()
				};
			}
			// TODO - fix for horizontal
			movingObject.anchoredPosition = new(movingObject.anchoredPosition.x, Mathf.Clamp(movingObject.anchoredPosition.y, 10, 250));
		}
	}

	void IResettable.ResetObject()
	{
		Complete = false;
		movingObject.anchoredPosition = startPos;
	}
}
