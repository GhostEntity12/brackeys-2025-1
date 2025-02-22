using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragTarget : MonoBehaviour, IDropHandler, IStageEventCondition, IResettable
{
	private Draggable heldDraggable;

	[SerializeField] List<string> acceptedIDs;

	public bool Complete { get; set; } = false;

	public void OnDrop(PointerEventData eventData)
	{
		if (!heldDraggable && eventData.pointerDrag.TryGetComponent(out Draggable d) && acceptedIDs.Contains(d.ID))
		{
			d.SetSnapPosition(transform.localPosition);
			heldDraggable = d;
			Complete = true;
		}
	}

	public void ResetObject()
	{
		heldDraggable = null;
		Complete = false;
	}
}
