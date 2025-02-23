using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
	[SerializeField] RectTransform frontCurtain;
	[SerializeField] CanvasGroup blackOut;
	private void Start()
	{
		LeanTween.moveY(frontCurtain, 900f, 1.5f).setEaseInOutBack();
		LeanTween.alphaCanvas(blackOut, 0, 1f).setDelay(0.7f).setEaseOutSine();
	}
	public void StartGame()
	{
		SceneManager.LoadScene(1);
	}
}
