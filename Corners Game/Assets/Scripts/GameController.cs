using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour
{
    [SerializeField] Text winnerText;
    [SerializeField] Text turnText;
    [SerializeField] GameObject restartButton;

	string playerOneName;
    string playerTwoName;

    bool _jumpDiagonal = false;
    bool _jumpLine = false;
    bool _moveOneSquare = true;

    string currentPlayer;
    bool gameOver = false;

	bool _isBotActive = false;

	public static GameController Instance { get; private set; }
	public bool CanJumpDiagonal { get => _jumpDiagonal; private set => _jumpDiagonal = value; }
	public bool CanJumpLine { get => _jumpLine; private set => _jumpLine = value; }
	public bool CanMoveOneSquare { get => _moveOneSquare; private set => _moveOneSquare = value; }

	private void Awake()
	{
		if (Instance == null) Instance = this;
		else Destroy(gameObject);
	}

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void ToggleJumpDiagonal(bool newValue)
    {
        _jumpDiagonal = newValue;
    }
    
    public void ToggleJumpLine(bool newValue)
    {
        _jumpLine = newValue;
    }

    public void ToggleMoveOneSquare(bool newValue)
    {
        _moveOneSquare = newValue;
    }

	public void ToggleBotMode(bool newValue)
	{
		_isBotActive = newValue;
	}

	public void StartGame(bool jumpDiagonal, bool jumpLine, bool moveOneSquare, bool isBotActive)
    {
		_jumpDiagonal = jumpDiagonal;
		_jumpLine = jumpLine;
		_moveOneSquare = moveOneSquare;
		_isBotActive = isBotActive;

		Board.Instance.StartGame();
        SetPlayersNames();
        currentPlayer = playerOneName;
        UpdatePlayerText();
    }

    private void SetPlayersNames()
    {
        playerOneName = "Игрок 1";

		if (_isBotActive)
		{
			playerTwoName = "Бот (Черные)";
		}
		else
		{
			playerTwoName = "Игрок 2";
		}
	}

    private void UpdatePlayerText()
    {
        turnText.text = currentPlayer + " ходит...";
    }

    public string GetCurrentPlayer()
    {
        return currentPlayer;
    }

	public void NextTurn()
	{
		if (currentPlayer == playerOneName)
		{
			currentPlayer = playerTwoName;
			UpdatePlayerText();

			// Если включен режим игры с ботом и игра не окончена - запускаем логику бота
			if (_isBotActive && !gameOver)
			{
				StartCoroutine(BotTurnCoroutine());
			}
		}
		else
		{
			currentPlayer = playerOneName;
			UpdatePlayerText();
		}
	}

	private IEnumerator BotTurnCoroutine()
	{
		// Пауза 1 секунда, чтобы игрок успел понять, что ход перешел к компьютеру
		yield return new WaitForSeconds(1.0f);

		BotController.Instance.MakeSmartMove();
	}

	public void Winner(string playerWinner)
    {
        gameOver = true;
		restartButton.SetActive(true);
		winnerText.enabled = true;
        winnerText.text = playerWinner + " победил!";
    }

    public bool IsGameOver()
    {
        return gameOver;
    }
   
    public string GetPlayerOneName()
    {
        return playerOneName;
    }
    
    public string GetPlayerTwoName()
    {
        return playerTwoName;
    }

	public bool IsBotActive()
	{
		return _isBotActive;
	}
}
