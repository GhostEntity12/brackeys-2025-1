using UnityEngine;

public class Billboard : MonoBehaviour
{
	private void Start()
	{
		transform.rotation = Camera.main.transform.rotation;
	}
}
