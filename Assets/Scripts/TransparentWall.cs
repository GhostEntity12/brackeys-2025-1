using UnityEngine;

public class TransparentWall : MonoBehaviour
{
	[SerializeField] Player player;
	Material m;
	
	bool IsTransparent
	{
		get => isTransparent;
		set
		{
			if (isTransparent == value) return;

			isTransparent = value;
			if (isTransparent)
			{
				LeanTween.color(gameObject, new(m.color.r, m.color.g, m.color.b, 0.5f), 0.1f);
			}
			else
			{
				LeanTween.color(gameObject, new(m.color.r, m.color.g, m.color.b, 1f), 0.1f);
			}
		}
	}

	bool isTransparent = false;

	private void Awake()
	{
		m = GetComponent<MeshRenderer>().material;
	}

	private void Update()
	{
		IsTransparent = 
			(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit1) && hit1.transform.gameObject == gameObject) ||
			(Physics.Raycast(Camera.main.transform.position, (player.transform.position - Camera.main.transform.position), out RaycastHit hit2) && hit2.transform.gameObject == gameObject);
	}
}
