using UnityEngine;
using UnityEngine.UI;

public class RulesPopup : MonoBehaviour
{
	[Header("Difficulty Toggles")]
	[SerializeField] Toggle jumpDiagonal;
	[SerializeField] Toggle jumpLine;
	[SerializeField] Toggle moveOneSquare;
	[SerializeField] Button playButton;

	private void Start()
	{
		playButton.onClick.AddListener(OnPlayClicked);
	}

	public void OnPlayClicked()
	{
		GameController.Instance.StartGame(jumpDiagonal.isOn, jumpLine.isOn, moveOneSquare.isOn);
		gameObject.SetActive(false);
	}

	private void OnDestroy()
	{
		if (playButton != null)
			playButton.onClick.RemoveListener(OnPlayClicked);
	}
}
