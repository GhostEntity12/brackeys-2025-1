using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class StageEvent : MonoBehaviour
{
	[SerializeField] private float timerLength = 10f;
	[SerializeField] private CanvasGroup popupCanvas;
	[SerializeField] private RectTransform popup;

	List<IStageEventCondition> conditionList;

	public bool InProgress { get; private set; } = false;
	public Timer Timer { get; private set; }
	public Vector3 InteractPosition { get; private set; }


	private void Start()
	{
		if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 5, -1))
		{
			InteractPosition = hit.position;
		}
		Timer = Instantiate(GameManager.Instance.TimerPrefab);
		Timer.Setup(timerLength, transform);
		Timer.transform.SetParent(GameManager.Instance.EventCanvas.transform);
	}
	private void Update()
	{
		if (conditionList.All(c => c.Complete))
		{
			Finish();
		}
	}

	public void Interact()
	{
		Timer.Pause();
		InProgress = true;
		GameManager.Instance.Player.SetCanMove(false);
		// Open popup
	}

	public void Finish()
	{
		Timer.Finish();
		InProgress = false;
		GameManager.Instance.Player.SetCanMove(true);
	}
}
