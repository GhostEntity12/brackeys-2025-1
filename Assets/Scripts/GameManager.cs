using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	[SerializeField] Vector2 spawnTimerRange = new(10, 12);
	[SerializeField] Canvas eventCanvas;
	
	[field: SerializeField] public Timer TimerPrefab { get; private set; }

	private float spawnTimerCountdown;
	private readonly List<Timer> timerList = new();

	void Update()
	{
		spawnTimerCountdown -= Time.deltaTime;

		if (spawnTimerCountdown > 0) return;
		spawnTimerCountdown = Random.Range(spawnTimerRange.x, spawnTimerRange.y);
		if (GetInactiveTimer(out Timer timer))
		{
			timer.Trigger();
		}

	}

	private bool GetInactiveTimer(out Timer t)
	{
		t = null;
		if (timerList.Count == 0) return false;

		List<Timer> inactiveTimers = timerList.Where(t => !t.Active).ToList();
		t = inactiveTimers[Random.Range(0, inactiveTimers.Count)];
		return true;
	}

	public void RegisterTimer(Timer t)
	{
		timerList.Add(t);
		t.transform.SetParent(eventCanvas.transform);
	}
}
