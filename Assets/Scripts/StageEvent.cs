using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class StageEvent : MonoBehaviour
{
	private List<IStageEventCondition> conditionList;
	private bool complete = false;

	[SerializeField] private float timerLength = 10f;
	[SerializeField] private CanvasGroup popupCanvas;
	[SerializeField] private RectTransform popup;
	[SerializeField] private List<GameObject> itemsToReset;

	public bool InProgress { get; private set; } = false;
	public Timer Timer { get; private set; }
	public Vector3 InteractPosition { get; private set; }


	private void Start()
	{
		if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 5, -1))
		{
			InteractPosition = hit.position;
		}
		Timer = Instantiate(GameManager.Instance.TimerPrefab, GameManager.Instance.EventCanvas.transform);
		Timer.Setup(timerLength, transform);

		conditionList = transform.GetComponentsInChildren<IStageEventCondition>().ToList();

		if (conditionList.Count == 0)
		{
			Debug.LogError("No assigned conditions");
			this.enabled = false;
		}


		popupCanvas.blocksRaycasts = false;
		popupCanvas.interactable = false;
		popup.localScale = Vector3.zero;
	}
	private void Update()
	{
		if (!complete && conditionList.All(c => c.Complete))
		{
			Finish();
		}
	}

	public void Interact()
	{
		Timer.Pause();
		foreach (var item in itemsToReset)
		{
			if (item.TryGetComponent(out IResettable r))
			{
				r.ResetObject();
			}
		}
		InProgress = true;
		complete = false;
		GameManager.Instance.Player.SetCanMove(false);
		// Open popup
		popupCanvas.blocksRaycasts = true;
		popupCanvas.interactable = true;
		LeanTween.scale(popup, Vector3.one, 0.2f).setEaseOutBack();
	}

	public void Finish()
	{
		Timer.Finish();
		InProgress = false;
		complete = true;
		GameManager.Instance.Player.SetCanMove(true);
		popupCanvas.blocksRaycasts = false;
		popupCanvas.interactable = false;
		LeanTween.scale(popup, Vector3.zero, 0.1f).setEaseInBack();
	}
}
