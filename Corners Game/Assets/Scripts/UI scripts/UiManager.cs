using UnityEngine;

public class UiManager : MonoBehaviour
{
	[SerializeField] RulesPopup rulesPopup;

	public static UiManager Instance { get; private set; }
	private void Awake()
	{
		if (Instance == null) Instance = this;
		else Destroy(gameObject);
	}

	public void OpenRules()
	{
		if (rulesPopup != null)
		{
			rulesPopup.gameObject.SetActive(true);
		}
	}
}
