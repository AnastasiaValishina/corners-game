using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinPopup : MonoBehaviour
{
	[SerializeField] GameObject panel;
	[SerializeField] TextMeshProUGUI text;
	[SerializeField] Button restartBtn;
	[SerializeField] Button statsBtn;
	[SerializeField] Image[] pawnImage;
	[SerializeField] GameObject sun;


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
				PlayWin();
			}
			else
			{
				sun.SetActive(false);
				text.text = "онпюфемхе";
			}
		}
		else
		{
			if (playerWinner == 2)
			{
				text.text = "онаедю аекшу";				
			}
			else
			{
				text.text = "онаедю вепмшу";				
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
	}

	private void OnDestroy()
	{
		if (restartBtn != null)
			restartBtn.onClick.RemoveListener(OnRestartClicked);

		if (statsBtn != null)
			statsBtn.onClick.RemoveListener(OnStatsClicked);
	}
}