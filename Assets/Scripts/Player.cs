using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
	[SerializeField] GameObject playerSpriteObject;

	private NavMeshAgent playerAgent;
	private SpriteRenderer playerSprite;
	private Animator playerSpriteAnimator;
	private bool canMove = true;

	private StageEvent targetedStageEvent;

	void Awake()
	{
		// Setting values
		playerAgent = GetComponent<NavMeshAgent>();
		playerSprite = playerSpriteObject.GetComponent<SpriteRenderer>();
		playerSpriteAnimator = playerSprite.GetComponent<Animator>();
	}

	void Update()
	{
		DoInput();

		DoMovement();

		DoAnimation();
	}

	void DoInput()
	{
		if (!canMove) return;

		// On click, if raycast hits raycastable
		if (Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, 1 << 6))
		{
			// Hit stage event
			if (hit.transform.TryGetComponent(out StageEvent stageEvent))
			{
				// Walk to stage event
				playerAgent.destination = stageEvent.InteractPosition;
				targetedStageEvent = stageEvent;
			}
			// Hit floor
			else if (NavMesh.SamplePosition(hit.point, out NavMeshHit nmHit, 2, -1))
			{
				// Walk to click location
				playerAgent.SetDestination(nmHit.position);
				targetedStageEvent = null;
			}
		}
	}

	void DoMovement()
	{
		// Flip sprite when moving left
		playerSprite.flipX = playerAgent.velocity.x < 0;

		// Player still traversing path
		if (!playerAgent.hasPath || playerAgent.remainingDistance >= 0.2f) return;

		// Destination reached
		playerAgent.ResetPath();

		if (!targetedStageEvent) return;
		// Destination is stageEvent
		targetedStageEvent.Interact();
	}

	void DoAnimation()
	{
		playerSpriteAnimator.SetBool("isMoving", playerAgent.velocity.magnitude > 0);
	}

	public void SetCanMove(bool canMove)
	{
		this.canMove = canMove;
		playerAgent.isStopped = !canMove;

		if (!canMove && playerAgent.hasPath)
		{
			playerAgent.ResetPath();
		}
	}
}
