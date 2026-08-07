using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	[Header("Difficulty Toggles")]
	[SerializeField] Toggle jumpDiagonal;
	[SerializeField] Toggle jumpLine;
	[SerializeField] Toggle moveOneSquare;
	[SerializeField] Toggle isBot;

	public void OnStartClicked()
	{
		GameController.Instance.StartGame(jumpDiagonal.isOn,jumpLine.isOn, moveOneSquare.isOn, isBot.isOn);
		gameObject.SetActive(false);
	}
}
