using UnityEngine;
using UnityEngine.UI;
using static BotController;

public class DifficultyPopup : MonoBehaviour
{
	[SerializeField] Button easyBtn;
	[SerializeField] Button normBtn;
	[SerializeField] Button hardBtn;

	private void Start()
	{
		easyBtn.onClick.AddListener(OnEasyClicked);
		normBtn.onClick.AddListener(OnNormalClicked);
		hardBtn.onClick.AddListener(OnHardClicked);
	}

	private void OnEasyClicked()
	{
		AudioPlayer.Instance.PlayButtonClick();
		BotController.Instance.SetDifficulty(BotDifficulty.Easy);
		UiManager.Instance.ShowRules(true);
		gameObject.SetActive(false);
	}

	private void OnNormalClicked()
	{
		AudioPlayer.Instance.PlayButtonClick();
		BotController.Instance.SetDifficulty(BotDifficulty.Medium);
		UiManager.Instance.ShowRules(true);
		gameObject.SetActive(false);
	}

	private void OnHardClicked()
	{
		AudioPlayer.Instance.PlayButtonClick();
		BotController.Instance.SetDifficulty(BotDifficulty.Hard);
		UiManager.Instance.ShowRules(true);
		gameObject.SetActive(false);
	}

	private void OnDestroy()
	{
		if (easyBtn != null)
			easyBtn.onClick.RemoveListener(OnEasyClicked);

		if (normBtn != null)
			normBtn.onClick.RemoveListener(OnNormalClicked);

		if (hardBtn != null)
			hardBtn.onClick.RemoveListener(OnHardClicked);
	}
}
