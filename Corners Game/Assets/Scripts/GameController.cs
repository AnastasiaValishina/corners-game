using System;
using System.Collections;
using UnityEngine;
using YG;

public class GameController : MonoBehaviour
{
	const int playerOne = 1;
	const int playerTwo = 2;

    bool _isGameOver = false;
	bool _isBotActive = false;
    int _currentPlayer;

	public bool IsGameOver { get => _isGameOver; private set => _isGameOver = value; }
	public bool IsBotActive { get => _isBotActive; set => _isBotActive = value; }
	public int CurrentPlayer { get => _currentPlayer; private set => _currentPlayer = value; }

	public static GameController Instance { get; private set; }

	private void Awake()
	{
		if (Instance == null) Instance = this;
		else Destroy(gameObject);
	}

    public void RestartGame()
    {
		_isGameOver = false;
		Board.Instance.SetBoard();
		_currentPlayer = 1;
		UiManager.Instance.UpdateTurn(_currentPlayer);
	}

	public void StartGame(CornersMode selectedMode)
    {
		Board.Instance.currentMode = selectedMode;
		RestartGame();
	}

	public void NextTurn()
	{
		if (_currentPlayer == playerOne)
		{
			_currentPlayer = playerTwo;
			UiManager.Instance.UpdateTurn(_currentPlayer);

			if (_isBotActive && !IsGameOver)
			{
				StartCoroutine(BotTurnCoroutine());
			}
		}
		else
		{
			_currentPlayer = playerOne;
			UiManager.Instance.UpdateTurn(_currentPlayer);
		}
	}

	private IEnumerator BotTurnCoroutine()
	{
		yield return new WaitForSeconds(1.0f);

		BotController.Instance.MakeSmartMove();
	}

	public void Winner(int playerWinner)
    {
		_isGameOver = true;
		if (_isBotActive)
		{
			bool isPlayerWin = (playerWinner == 1);

			switch (BotController.Instance.currentDifficulty)
			{
				case BotDifficulty.Easy:
					if (isPlayerWin) YG2.saves.winsEasy++;
					else YG2.saves.losesEasy++;
					break;

				case BotDifficulty.Medium:
					if (isPlayerWin) YG2.saves.winsNormal++;
					else YG2.saves.losesNormal++;
					break;

				case BotDifficulty.Hard:
					if (isPlayerWin) YG2.saves.winsHard++;
					else YG2.saves.losesHard++;
					break;
			}

			YG2.SaveProgress();
		}
		UiManager.Instance.ShowWinPopup(playerWinner, _isBotActive);		
    }
}
