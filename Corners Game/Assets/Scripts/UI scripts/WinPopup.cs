using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinPopup : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI text;
	[SerializeField] Button restartBtn;
	[SerializeField] Button statsBtn;

	private void Start()
	{
		restartBtn.onClick.AddListener(OnRestartClicked);
		statsBtn.onClick.AddListener(OnStatsClicked);
	}

	public void ShowWin(int playerWinner, bool isBot)
	{
		statsBtn.gameObject.SetActive(isBot);

		if (isBot)
		{
			if (playerWinner == 1)
			{
				text.text = "онаедю!";
				AudioPlayer.Instance.PlayWinSound();
			}
			else
			{
				text.text = "онпюфемхе";
			}
		}
		else
		{
			if (playerWinner == 2)
			{
				text.text = "онаедю аекшу";
				AudioPlayer.Instance.PlayWinSound();
			}
			else
			{
				text.text = "онаедю вепмшу";
				AudioPlayer.Instance.PlayWinSound();
			}
		}
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
	}


	private void OnDestroy()
	{
		if (restartBtn != null)
			restartBtn.onClick.RemoveListener(OnRestartClicked);

		if (statsBtn != null)
			statsBtn.onClick.RemoveListener(OnStatsClicked);
	}
}
