using YG;

public static class Settings
{
	public static bool IsMusicOn
	{
		get { return YG2.saves.isMusicOn; }
		set
		{
			YG2.saves.isMusicOn = value;
			YG2.SaveProgress();
		}
	}

	public static bool IsSoundOn
	{
		get { return YG2.saves.isSoundOn; }
		set
		{
			YG2.saves.isSoundOn = value;
			YG2.SaveProgress();
		}
	}
}