using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
	private NavMeshAgent playerAgent;
	private SpriteRenderer playerSprite;
	private Animator playerSpriteAnimator;
	private StageEvent targetedStageEvent = null;
	private bool canMove = true;

	[SerializeField] GameObject playerSpriteObject;

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
			// Hit active stage event
			if (hit.transform.TryGetComponent(out StageEvent stageEvent) && stageEvent.Timer.Active)
			{
				// Walk to stage event
				playerAgent.destination = stageEvent.InteractPosition;
				targetedStageEvent = stageEvent;
			}
			// Hit floor
			else if (NavMesh.SamplePosition(hit.point, out NavMeshHit nmHit, 1, -1))
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

		if (targetedStageEvent && !targetedStageEvent.Timer.Failed)
		{
			// Destination is stageEvent and not failed
			targetedStageEvent.Interact();
		}
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
