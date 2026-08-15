using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
	[SerializeField] RulesPopup rulesPopup;
	[SerializeField] MainMenu mainMenu;
	[SerializeField] OptionsPopup optionsPopup;

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
