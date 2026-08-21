using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class WinPopup : MonoBehaviour
{
	[SerializeField] GameObject panel;
	[SerializeField] TextMeshProUGUI text;
	[SerializeField] Button restartBtn;
	[SerializeField] Button mainMenuBtn;
	[SerializeField] Button statsBtn;
	[SerializeField] Image[] pawnImage;
	[SerializeField] GameObject sun;


	private void Start()
	{
		restartBtn.onClick.AddListener(OnRestartClicked);
		statsBtn.onClick.AddListener(OnStatsClicked);
		mainMenuBtn.onClick.AddListener(OnMenuClicked);
	}

	private void OnMenuClicked()
	{
		AudioPlayer.Instance.PlayButtonClick();
		UiManager.Instance.ShowMainMenu(true);
		gameObject.SetActive(false);
	}

	public void ShowWin(int playerWinner, bool isBot)
	{
		statsBtn.gameObject.SetActive(isBot);

		string lang = YG2.envir.language;

		if (isBot)
		{
			if (playerWinner == 1)
			{
				text.text = lang == "ru" ? "онаедю!" : "VICTORY!";
				PlayWin();
			}
			else
			{
				sun.SetActive(false);
				text.text = lang == "ru" ? "онпюфемхе" : "DEFEAT"; ;
			}
		}
		else
		{
			if (playerWinner == 2)
			{
				text.text = lang == "ru" ? "онаедю аекшу" : "WHITE WINS!";
			}
			else
			{
				text.text = lang == "ru" ? "онаедю вепмшу" : "BLACK WINS!";
			}
			PlayWin();
		}

	}

	private void PlayWin()
	{
		AudioPlayer.Instance.PlayWinSound();
		panel.transform.localScale = Vector3.zero;
		LeanTween.scale(panel, Vector3.one, 0.8f).setEase(LeanTweenType.easeOutBack);
		PlayPawnsDance();
		PlaySunburstAnimation();
	}

	private void PlayPawnsDance()
	{
		float jumpHeight = 30f;
		float jumpDuration = 0.25f; 
		float delayBetweenJumps = 0.15f; 

		for (int i = 0; i < pawnImage.Length; i++)
		{
			GameObject pawnObj = pawnImage[i].gameObject;

			LeanTween.cancel(pawnObj);

			float startY = pawnImage[i].rectTransform.localPosition.y;

			LeanTween.moveLocalY(pawnObj, startY + jumpHeight, jumpDuration)
				.setEase(LeanTweenType.easeOutQuad)
				.setLoopPingPong()
				.setRepeat(-1) 
				.setDelay(i * delayBetweenJumps);
		}
	}
	private void PlaySunburstAnimation()
	{
		sun.SetActive(true);
		LeanTween.cancel(sun);
		LeanTween.rotateAroundLocal(sun, Vector3.forward, -360f, 10f).setLoopClamp();
	}
	void OnRestartClicked()
	{
		AudioPlayer.Instance.PlayButtonClick();
		GameController.Instance.RestartGame();
		gameObject.SetActive(false);
	}

	void OnStatsClicked()
	{
		AudioPlayer.Instance.PlayButtonClick();
		UiManager.Instance.ShowStats();
	}

	private void OnDestroy()
	{
		if (restartBtn != null)
			restartBtn.onClick.RemoveListener(OnRestartClicked);

		if (statsBtn != null)
			statsBtn.onClick.RemoveListener(OnStatsClicked);

		if (mainMenuBtn != null)
			mainMenuBtn.onClick.RemoveListener(OnMenuClicked);
	}
}