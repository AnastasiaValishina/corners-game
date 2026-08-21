using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	[SerializeField] Button hotSeatBtn;
	[SerializeField] Button botBtn;
	[SerializeField] Button statsBtn;

	private void Start()
	{
		botBtn.onClick.AddListener(OnBotClicked);
		hotSeatBtn.onClick.AddListener(OnHotSeatClicked);
		statsBtn.onClick.AddListener(OnStatsClicked);
	}

	private void OnStatsClicked()
	{
		AudioPlayer.Instance.PlayButtonClick();
		UiManager.Instance.ShowStats();
	}

	private void OnBotClicked()
	{
		AudioPlayer.Instance.PlayButtonClick();
		GameController.Instance.IsBotActive = true;
		UiManager.Instance.ShowDiffPopup(true);
		gameObject.SetActive(false);
	}

	private void OnHotSeatClicked()
	{
		AudioPlayer.Instance.PlayButtonClick();
		GameController.Instance.IsBotActive = false;
		UiManager.Instance.ShowRules(true);
		gameObject.SetActive(false);
	}

	private void OnDestroy()
	{
		if (botBtn != null)
			botBtn.onClick.RemoveListener(OnBotClicked);

		if (hotSeatBtn != null)
			hotSeatBtn.onClick.RemoveListener(OnHotSeatClicked);

		if (statsBtn != null)
			statsBtn.onClick.RemoveListener(OnStatsClicked);
	}
}
