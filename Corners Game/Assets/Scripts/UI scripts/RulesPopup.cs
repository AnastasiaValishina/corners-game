using UnityEngine;
using UnityEngine.UI;

public class RulesPopup : MonoBehaviour
{
	[SerializeField] private Toggle classicModeToggle;
	[SerializeField] private Toggle diagonalModeToggle;
	[SerializeField] private Button playButton;
	[SerializeField] private Button backButton;

	private void Start()
	{
		playButton.onClick.AddListener(OnPlayClicked);
		backButton.onClick.AddListener(OnBackClicked);
	}

	private void OnEnable()
	{
		if (classicModeToggle != null)
		{
			classicModeToggle.isOn = true;
		}
	}

	private void OnBackClicked()
	{
		AudioPlayer.Instance.PlayButtonClick();
		UiManager.Instance.ShowMainMenu(true);
		gameObject.SetActive(false);
	}

	public void PlaySound()
	{
		AudioPlayer.Instance.PlayButtonClick();
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

		if (backButton != null)
			backButton.onClick.RemoveListener(OnBackClicked);
	}
}
