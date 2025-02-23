using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TreeEditor;

public class Draggable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IResettable
{
	private Image sprite;
	private Vector3 snapPosition;
	private Vector3 initialPosition;
	private Vector2 deltaBounds;

	[SerializeField] RectTransform bounds;
	[SerializeField] bool lockX = false;
	[SerializeField] bool lockY = false;
	[SerializeField] bool snapToPosition = false;
	[SerializeField] bool isSnapped = false;

	[field: SerializeField] public string ID { get; private set; }

	private void Awake()
	{
		sprite = GetComponent<Image>();
		snapPosition = transform.localPosition;
		initialPosition = transform.localPosition;
		deltaBounds = bounds.sizeDelta / 2;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		sprite.raycastTarget = false;
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (lockX)
		{
			transform.position = new(transform.position.x, eventData.position.y);
		}
		else if (lockY)
		{
			transform.position = new(eventData.position.x, transform.position.y);
		}
		else
		{
			transform.position = eventData.position;
		}

		if (bounds)
		{
			transform.localPosition = new(Mathf.Clamp(transform.localPosition.x, -deltaBounds.x, deltaBounds.x), Mathf.Clamp(transform.localPosition.y, -deltaBounds.y, deltaBounds.y));
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		sprite.raycastTarget = true;
		if (snapToPosition || isSnapped)
		{
			transform.localPosition = snapPosition;
		}
	}

	public void SetSnapPosition(Vector3 position)
	{
		isSnapped = true;
		sprite.raycastTarget = false;
		snapPosition = position;
	}

	public void ResetSnapping()
	{
		isSnapped = false;
		sprite.raycastTarget = true;
	}

	public void ResetObject()
	{
		transform.localPosition = initialPosition;
	}
}
