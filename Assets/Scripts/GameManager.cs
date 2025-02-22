using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	private float spawnTimerCountdown;
	private readonly List<StageEvent> StageEventList = new();

	[SerializeField] private Vector2 spawnTimerRange = new(10, 12);

	[field: SerializeField] public Canvas EventCanvas { get; private set; }
	[field: SerializeField] public Timer TimerPrefab { get; private set; }
	[field: SerializeField] public Player Player { get; private set; }

	private void Start()
	{
		// Get all StageEvents in the scene
		StageEventList.AddRange(FindObjectsByType<StageEvent>(FindObjectsSortMode.None));
		Debug.Log(StageEventList.Count);
	}

	void Update()
	{
		spawnTimerCountdown -= Time.deltaTime;

		if (spawnTimerCountdown > 0) return;
		spawnTimerCountdown = Random.Range(spawnTimerRange.x, spawnTimerRange.y);
		if (GetInactiveEvent(out StageEvent stageEvent))
		{
			stageEvent.Timer.Trigger();
		}
	}

	private bool GetInactiveEvent(out StageEvent e)
	{
		e = null;

		List<StageEvent> inactiveEvents = StageEventList.Where(t => !t.InProgress && !t.Timer.Active).ToList();
		if (StageEventList.Count == 0 || inactiveEvents.Count == 0) return false;
		e = inactiveEvents[Random.Range(0, inactiveEvents.Count)];
		return true;
	}
}
