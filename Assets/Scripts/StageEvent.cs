using UnityEngine;
using UnityEngine.AI;

public class StageEvent : MonoBehaviour
{
	[SerializeField] float timerLength = 10f;

	public Vector3 InteractPosition { get; private set; }

	private Timer timer;

	private void Start()
	{
		if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 5, -1))
		{
			InteractPosition = hit.position;
		}
		timer = Instantiate(GameManager.Instance.TimerPrefab);
		timer.Setup(timerLength, transform);
	}

	public void Interact()
	{
		timer.Pause();
		// Open popup
	}
}
