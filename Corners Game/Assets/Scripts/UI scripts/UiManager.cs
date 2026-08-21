using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
	[SerializeField] RulesPopup rulesPopup;
	[SerializeField] MainMenu mainMenu;
	[SerializeField] OptionsPopup optionsPopup;
	[SerializeField] DifficultyPopup diffPopup;
	[SerializeField] WinPopup winPopup;
	[SerializeField] StatsPopup statsPopup;

	[Header("Hud")]
	[SerializeField] Image nextPlayerImage;
	[SerializeField] Sprite playerOne;
	[SerializeField] Sprite playerTwo;
	[SerializeField] Button optionsButton;

	public static UiManager Instance { get; private set; }
	private void Awake()
	{
		if (Instance == null) Instance = this;
		else Destroy(gameObject);
	}

	private void Start()
	{
		optionsButton.onClick.AddListener(ShowOptions);
		ShowRules(false);
		ShowMainMenu(true);
	}

	void ShowOptions()
	{
		AudioPlayer.Instance.PlayButtonClick();
		if (optionsPopup != null)
		{
			optionsPopup.gameObject.SetActive(true);
		}
	}

	public void ShowDiffPopup(bool isShown)
	{
		if (diffPopup != null)
		{
			diffPopup.gameObject.SetActive(isShown);
		}
	}

	public void ShowWinPopup(int playerWinner, bool isBot)
	{
		if (winPopup != null)
		{
			winPopup.gameObject.SetActive(true);
			winPopup.ShowWin(playerWinner, isBot);
		}
	}

	public void ShowStats()
	{
		if (statsPopup != null)
		{
			statsPopup.gameObject.SetActive(true);
		}
	}

	public void ShowRules(bool isShown)
	{
		if (rulesPopup != null)
		{
			rulesPopup.gameObject.SetActive(isShown);
		}
	}

	public void ShowMainMenu(bool isShown)
	{
		if (mainMenu != null)
		{
			mainMenu.gameObject.SetActive(isShown);
		}
	}

	public void UpdateTurn(int player)
	{
		if (player == 1)
		{
			nextPlayerImage.sprite = playerOne;
		}
		else if (player == 2)
		{
			nextPlayerImage.sprite = playerTwo;
		}
	}

	private void OnDestroy()
	{
		if (optionsButton != null)
			optionsButton.onClick.RemoveListener(ShowOptions);
	}
}
