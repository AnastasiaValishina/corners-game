using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
	const int playerOne = 1;
	const int playerTwo = 2;

    bool _jumpDiagonal = true;
    bool _jumpLine = true;
    bool _moveOneSquare = true;
    bool _isGameOver = false;
	bool _isBotActive = false;
    int _currentPlayer;


	public bool CanJumpDiagonal { get => _jumpDiagonal; private set => _jumpDiagonal = value; }
	public bool CanJumpLine { get => _jumpLine; private set => _jumpLine = value; }
	public bool CanMoveOneSquare { get => _moveOneSquare; private set => _moveOneSquare = value; }
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

	public void StartGame(bool jumpDiagonal, bool jumpLine, bool moveOneSquare)
    {
		_jumpDiagonal = jumpDiagonal;
		_jumpLine = jumpLine;
		_moveOneSquare = moveOneSquare;
		
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
		UiManager.Instance.ShowWinPopup(playerWinner, _isBotActive);		
    }
}
