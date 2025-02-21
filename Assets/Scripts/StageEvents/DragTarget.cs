using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragTarget : MonoBehaviour, IDropHandler, IStageEventCondition
{
	private bool complete = false;
	private Draggable heldDraggable;

	[SerializeField] List<string> acceptedIDs;

	public bool Complete { get => complete; set => complete = value; }
	
	public void OnDrop(PointerEventData eventData)
	{
		if (eventData.pointerDrag.TryGetComponent(out Draggable d) && acceptedIDs.Contains(d.ID))
		{
			d.SetSnapPosition(transform.localPosition);

			if (heldDraggable && heldDraggable != d)
			{
				heldDraggable.ResetSnapping();
				heldDraggable.ResetPosition();
			}

			heldDraggable = d;
			complete = true;
		}
	}
}
