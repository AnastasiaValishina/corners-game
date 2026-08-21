using UnityEngine;
using UnityEngine.UI;

public class OptionsPopup : MonoBehaviour
{
	[SerializeField] Button musicButton;
	[SerializeField] Button soundButton;
	[SerializeField] Button mainMenuButton;
	[SerializeField] Button restartButton;
	[SerializeField] Button continueButton;

	private void Start()
	{
		musicButton.onClick.AddListener(OnMusicClicked);
		soundButton.onClick.AddListener(OnSoundClicked);
		mainMenuButton.onClick.AddListener(OnMenuClicked);
		restartButton.onClick.AddListener(OnRestartClicked);
		continueButton.onClick.AddListener(OnContinueClicked);
		UpdateButtonVisuals();
	}

	private void OnMusicClicked()
	{
		if (AudioPlayer.Instance != null)
		{
			AudioPlayer.Instance.PlayButtonClick();
			AudioPlayer.Instance.ToggleMusic();
			UpdateButtonVisuals();
		}
	}

	private void OnSoundClicked()
	{
		if (AudioPlayer.Instance != null)
		{
			AudioPlayer.Instance.ToggleSound();
			AudioPlayer.Instance.PlayButtonClick();
			UpdateButtonVisuals();
		}
	}

	private void UpdateButtonVisuals()
	{
		if (Settings.IsMusicOn)		
			musicButton.GetComponent<ButtonSpriteSwap>().SetOn();		
		else		
			musicButton.GetComponent<ButtonSpriteSwap>().SetOff();
		
		if (Settings.IsSoundOn)
			soundButton.GetComponent<ButtonSpriteSwap>().SetOn();
		else
			soundButton.GetComponent<ButtonSpriteSwap>().SetOff();
	}

	private void OnMenuClicked()
	{
		AudioPlayer.Instance.PlayButtonClick();
		UiManager.Instance.ShowMainMenu(true);
		gameObject.SetActive(false);
	}

	private void OnRestartClicked()
	{
		AudioPlayer.Instance.PlayButtonClick();
		GameController.Instance.RestartGame();
		gameObject.SetActive(false);
	}

	private void OnContinueClicked()
	{
		AudioPlayer.Instance.PlayButtonClick();
		gameObject.SetActive(false);
	}

	private void OnDestroy()
	{
		if (musicButton != null)
			musicButton.onClick.RemoveListener(OnMusicClicked);

		if (soundButton != null)
			soundButton.onClick.RemoveListener(OnSoundClicked);

		if (mainMenuButton != null)
			mainMenuButton.onClick.RemoveListener(OnMenuClicked);

		if (restartButton != null)
			restartButton.onClick.RemoveListener(OnRestartClicked);

		if (continueButton != null)
			continueButton.onClick.RemoveListener(OnContinueClicked);
	}

}
