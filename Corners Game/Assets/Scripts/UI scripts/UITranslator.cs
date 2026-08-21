using UnityEngine;
using TMPro; 
using YG;

public class UITranslator : MonoBehaviour
{
	public string ru;
	public string en;

	private TextMeshProUGUI textComponent;

	private void Awake()
	{
		textComponent = GetComponent<TextMeshProUGUI>();
	}

	private void OnEnable()
	{
		YG2.onSwitchLang += SwitchLanguage;

		SwitchLanguage(YG2.lang);
	}

	private void OnDisable()
	{
		YG2.onSwitchLang -= SwitchLanguage;
	}

	public void SwitchLanguage(string lang)
	{
		switch (lang)
		{
			case "ru":
				textComponent.text = ru;
				break;
			default:
				textComponent.text = en; 
				break;
		}
	}
}