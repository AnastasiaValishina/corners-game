using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour
{
    [SerializeField] Text winnerText;
    [SerializeField] Text turnText;
    [SerializeField] GameObject restartButton;

	const int playerOne = 1;
	const int playerTwo = 2;

    bool _jumpDiagonal = false;
    bool _jumpLine = false;
    bool _moveOneSquare = true;

    int currentPlayer;
    bool gameOver = false;

	bool _isBotActive = false;

	public bool CanJumpDiagonal { get => _jumpDiagonal; private set => _jumpDiagonal = value; }
	public bool CanJumpLine { get => _jumpLine; private set => _jumpLine = value; }
	public bool CanMoveOneSquare { get => _moveOneSquare; private set => _moveOneSquare = value; }
	public static GameController Instance { get; private set; }

	private void Awake()
	{
		if (Instance == null) Instance = this;
		else Destroy(gameObject);
	}

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

	public void StartGame(bool jumpDiagonal, bool jumpLine, bool moveOneSquare)
    {
		_jumpDiagonal = jumpDiagonal;
		_jumpLine = jumpLine;
		_moveOneSquare = moveOneSquare;

		Board.Instance.StartGame();
        currentPlayer = 1;
		UiManager.Instance.UpdateTurn(currentPlayer);
    }

    public int GetCurrentPlayer()
    {
        return currentPlayer;
    }

	public void NextTurn()
	{
		if (currentPlayer == playerOne)
		{
			currentPlayer = playerTwo;
			UiManager.Instance.UpdateTurn(currentPlayer);

			if (_isBotActive && !gameOver)
			{
				StartCoroutine(BotTurnCoroutine());
			}
		}
		else
		{
			currentPlayer = playerOne;
			UiManager.Instance.UpdateTurn(currentPlayer);
		}
	}

	private IEnumerator BotTurnCoroutine()
	{
		yield return new WaitForSeconds(1.0f);

		BotController.Instance.MakeSmartMove();
	}

	public void Winner(int playerWinner)
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
   
    public int GetPlayerOneName()
    {
        return playerOne;
    }
    
    public int GetPlayerTwoName()
    {
        return playerTwo;
    }

	public bool IsBotActive()
	{
		return _isBotActive;
	}

	public void SetBot(bool botGame)
	{
		_isBotActive = botGame;
	}
}
