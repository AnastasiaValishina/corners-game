using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class StatsPopup : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI winsEasyText;
	[SerializeField] TextMeshProUGUI losesEasyText;
	[SerializeField] TextMeshProUGUI winsNormalText;
	[SerializeField] TextMeshProUGUI losesNormalText;
	[SerializeField] TextMeshProUGUI winsHardText;
	[SerializeField] TextMeshProUGUI losesHardText;
	[SerializeField] Button bottonBack;

	private void Start()
	{
		bottonBack.onClick.AddListener(OnBackClicked);
	}

	private void OnBackClicked()
	{
		AudioPlayer.Instance.PlayButtonClick();
		gameObject.SetActive(false);
	}

	private void OnEnable()
	{
		UpdateStats();
	}

	private void UpdateStats()
	{
		winsEasyText.text = YG2.saves.winsEasy.ToString();
		losesEasyText.text = YG2.saves.losesEasy.ToString();

		winsNormalText.text = YG2.saves.winsNormal.ToString();
		losesNormalText.text = YG2.saves.losesNormal.ToString();

		winsHardText.text = YG2.saves.winsHard.ToString();
		losesHardText.text = YG2.saves.losesHard.ToString();
	}

	private void OnDestroy()
	{
		if (bottonBack != null)
			bottonBack.onClick.RemoveListener(OnBackClicked);
	}
}
