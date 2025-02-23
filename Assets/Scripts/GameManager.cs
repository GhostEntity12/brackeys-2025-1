using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
	private float spawnTimerCountdown = 5;
	private readonly List<StageEvent> StageEventList = new();
	private bool gameOver;
	private float gameTimer;
	private int act = 1;
	private Vector2 spawnTimerRange = new(10, 12);

	[Header("Acts")]
	[SerializeField] private float act1End;
	[SerializeField] private float act2End;
	[SerializeField] private float act3End;
	[SerializeField] private Vector2 spawnTimerRangeAct1 = new(15, 20);
	[SerializeField] private Vector2 spawnTimerRangeAct2 = new(12, 15);
	[SerializeField] private Vector2 spawnTimerRangeAct3 = new(8, 10);

	[Header("Game End")]
	[SerializeField] RectTransform gameWinScreen;
	[SerializeField] RectTransform gameOverScreen;
	[SerializeField] TextMeshProUGUI gameOverInfo;

	[Header("Audio")]
	[SerializeField] AudioSource musicSource;
	[SerializeField] AudioSource effectSource;
	[SerializeField] AudioClip loseAudio;
	[SerializeField] AudioClip winAudio;
	[SerializeField] AudioClip winAudio2;

	[field: SerializeField, Space(10f)] public Canvas EventCanvas { get; private set; }
	[field: SerializeField] public Timer TimerPrefab { get; private set; }
	[field: SerializeField] public Player Player { get; private set; }
	[SerializeField] RectTransform actProgress;
	[SerializeField] RectTransform actProgressMarker;

	private void Start()
	{
		// Get all StageEvents in the scene
		StageEventList.AddRange(FindObjectsByType<StageEvent>(FindObjectsSortMode.None));
		SetAct(1);
	}

	void Update()
	{
		if (gameOver) return;

		gameTimer += Time.deltaTime;
		if (gameTimer > act3End && act == 3)
		{
			OnGameWin();
		}
		else if (gameTimer > act2End && act == 2)
		{
			SetAct(3);
		}
		else if (gameTimer > act1End && act == 1)
		{
			SetAct(2);
		}
		actProgressMarker.anchoredPosition = new(gameTimer / act3End * actProgress.sizeDelta.x, 0);
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

	public void SetAct(int act)
	{
		this.act = act;
		switch (act)
		{
			case 1:
				spawnTimerRange = spawnTimerRangeAct1;
				break;
			case 2:
				spawnTimerRange = spawnTimerRangeAct2;
				break;
			case 3:
				spawnTimerRange = spawnTimerRangeAct3;
				break;
		}
	}

	public void OnGameOver(string failMessage)
	{
		if (gameOver) return;
		gameOver = true;

		LeanTween.value(musicSource.volume, 0, 0.7f).setOnComplete(musicSource.Stop);
		effectSource.PlayOneShot(loseAudio, 0.7f);
		Debug.Log(failMessage);
		LeanTween.moveY(gameOverScreen, 0, 1f).setEaseOutBounce();
		gameOverInfo.text = $"You made it to Act {act} before {failMessage}.";
	}

	public void OnGameWin()
	{
		if (gameOver) return;
		gameOver = true;

		effectSource.PlayOneShot(winAudio, 0.7f);
		effectSource.PlayOneShot(winAudio2, 0.7f);
		LeanTween.moveY(gameWinScreen, 0, 1f).setEaseOutCubic();

	}

	public void ToMenu()
	{
		SceneManager.LoadScene(0);
	}
	public void Restart()
	{
		SceneManager.LoadScene(1);
	}
}
