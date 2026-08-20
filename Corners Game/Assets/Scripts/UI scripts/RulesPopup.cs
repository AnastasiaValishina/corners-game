using UnityEngine;
using UnityEngine.UI;

public class RulesPopup : MonoBehaviour
{
	[SerializeField] private Toggle classicModeToggle;
	[SerializeField] private Toggle diagonalModeToggle;
	[SerializeField] private Button playButton;

	private void Start()
	{
		playButton.onClick.AddListener(OnPlayClicked);
	}

	public void OnPlayClicked()
	{
		AudioPlayer.Instance.PlayButtonClick();

		CornersMode selectedMode = diagonalModeToggle.isOn ? CornersMode.Diagonal : CornersMode.Classic;
		GameController.Instance.StartGame(selectedMode);

		gameObject.SetActive(false);
	}

	private void OnDestroy()
	{
		if (playButton != null)
			playButton.onClick.RemoveListener(OnPlayClicked);
	}
}
