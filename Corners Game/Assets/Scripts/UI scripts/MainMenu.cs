using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	[SerializeField] Button hotSeatBtn;
	[SerializeField] Button botBtn;

	[Header("Difficulty Toggles")]
	[SerializeField] Toggle jumpDiagonal;
	[SerializeField] Toggle jumpLine;
	[SerializeField] Toggle moveOneSquare;
	[SerializeField] Toggle isBot;

	private void Start()
	{
		botBtn.onClick.AddListener(OnBotClicked);
		hotSeatBtn.onClick.AddListener(OnHotSeatClicked);
	}


	public void OnStartClicked()
	{
		GameController.Instance.StartGame(jumpDiagonal.isOn,jumpLine.isOn, moveOneSquare.isOn, isBot.isOn);
		gameObject.SetActive(false);
	}

	private void OnBotClicked()
	{
		GameController.Instance.SetBot(true);
		UiManager.Instance.OpenRules();
		gameObject.SetActive(false);
	}

	private void OnHotSeatClicked()
	{
		GameController.Instance.SetBot(false);
		UiManager.Instance.OpenRules();
		gameObject.SetActive(false);
	}
	private void OnDestroy()
	{
		if (botBtn != null)
			botBtn.onClick.RemoveListener(OnBotClicked);

		if (hotSeatBtn != null)
			hotSeatBtn.onClick.RemoveListener(OnHotSeatClicked);
	}
}
