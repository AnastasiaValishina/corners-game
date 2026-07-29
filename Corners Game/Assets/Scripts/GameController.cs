using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject popupMenu;
    [SerializeField] Text winnerText;
    [SerializeField] Text turnText;
    [SerializeField] GameObject restartButton;

    [Header("Difficulty Toggles")]
    [SerializeField] Toggle toggleJumpDiagonal;
    [SerializeField] Toggle toggleJumpLine;
    [SerializeField] Toggle toggleMoveOneSquare;

    string playerOneName;
    string playerTwoName;

    bool jumpDiagonal = false;
    bool jumpLine = false;
    bool moveOneSquare = true;
    string currentPlayer;
    bool gameOver = false;

	public static GameController Instance { get; private set; }
	private void Awake()
	{
		if (Instance == null) Instance = this;
		else Destroy(gameObject);
	}

	void Start()
    {
        toggleJumpDiagonal.isOn = jumpDiagonal;
        toggleJumpLine.isOn = jumpLine;
        toggleMoveOneSquare.isOn = moveOneSquare;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void ToggleJumpDiagonal(bool newValue)
    {
        jumpDiagonal = newValue;
    }
    
    public void ToggleJumpLine(bool newValue)
    {
        jumpLine = newValue;
    }

    public void ToggleMoveOneSquare(bool newValue)
    {
        moveOneSquare = newValue;
    }

    public void OnStartClick()
    {
        Board.Instance.StartGame();
        popupMenu.SetActive(false);
        SetPlayersNames();
        currentPlayer = playerOneName;
        UpdatePlayerText();
    }

    private void SetPlayersNames()
    {
        playerOneName = "Игрок 1";
        playerTwoName = "Игрок 2";
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
        }
        else
        {
            currentPlayer = playerOneName;
            UpdatePlayerText();
        }
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
    
    public bool CanJumpDiagonal()
    {
        return jumpDiagonal;
    }
    
    public bool CanJumpLine()
    {
        return jumpLine;
    }
    
    public bool CanMoveOneSquare()
    {
        return moveOneSquare;
    }

    public string GetPlayerOneName()
    {
        return playerOneName;
    }
    
    public string GetPlayerTwoName()
    {
        return playerTwoName;
    }
}
