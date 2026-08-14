using System;
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
	}
	private void OnMusicClicked()
	{
		throw new NotImplementedException();
	}

	private void OnSoundClicked()
	{
		throw new NotImplementedException();
	}


	private void OnMenuClicked()
	{
		UiManager.Instance.ShowMainMenu(true);
		gameObject.SetActive(false);
	}

	private void OnRestartClicked()
	{
		GameController.Instance.RestartGame();
		gameObject.SetActive(false);
	}

	private void OnContinueClicked()
	{
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
