using UnityEngine;
using UnityEngine.EventSystems;

public class Pawn : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private MovePlate movePlate;
    int xPos;
    int yPos;
    
    int player;
    Board board;
    GameController gameController;

    void Start()
    {
        board = Board.Instance              ;
        gameController = GameController.Instance;
        xPos = (int)transform.position.x;
        yPos = (int)transform.position.y;

        switch (name)
        {
            case "white": player = 1;
                break;
            case "black": player = 2;
                break;
        }
    }

    public void SetCoords()
    {
        transform.position = new Vector3(xPos, yPos, 0f);
    }

	public void OnPointerClick(PointerEventData eventData)
	{
		// Если игра не окончена И сейчас ход этого игрока
		if (!gameController.IsGameOver() && gameController.GetCurrentPlayer() == player)
		{
			// БЛОКИРОВКА: Если это черная пешка, и сейчас играет бот — запрещаем ручной клик
			if (gameController.IsBotActive() && name == "black")
			{
				return; // Прерываем выполнение метода
			}

			DestroyMovePlates();
			InitiateMovePlates();
		}
	}

	private void InitiateMovePlates()
    {
        if (gameController.CanMoveOneSquare)
        {
            ActivateMoveOneSquare();
        }

        if (gameController.CanJumpDiagonal)
        {
            ActivateJumpOverDiag();
        }

        if (gameController.CanJumpLine)
        {
            ActivateJumpOver();
        }
    }

    private void ActivateJumpOver()         // перепрыгнуть по вертикали по горизонтали
    {
        if (board.PositionOnBoardExists(xPos + 1, yPos) && board.GetPosition(xPos + 1, yPos))
        {
            SpawnMovePlate(xPos + 2, yPos);
        }
        if (board.PositionOnBoardExists(xPos, yPos + 1) && board.GetPosition(xPos, yPos + 1))
        {
            SpawnMovePlate(xPos, yPos + 2);
        }
        if (board.PositionOnBoardExists(xPos - 1, yPos) && board.GetPosition(xPos - 1, yPos))
        {
            SpawnMovePlate(xPos - 2, yPos);
        }
        if (board.PositionOnBoardExists(xPos, yPos - 1) && board.GetPosition(xPos, yPos - 1))
        {
            SpawnMovePlate(xPos, yPos - 2);
        }
    }

    private void ActivateJumpOverDiag()         // перепрыгнуть по диагонали
    {
        if (board.PositionOnBoardExists(xPos + 1, yPos + 1) && board.GetPosition(xPos + 1, yPos + 1))
        {
            SpawnMovePlate(xPos + 2, yPos + 2);
        }
        if (board.PositionOnBoardExists(xPos - 1, yPos + 1) && board.GetPosition(xPos - 1, yPos + 1))
        {
            SpawnMovePlate(xPos - 2, yPos + 2);
        }
        if (board.PositionOnBoardExists(xPos + 1, yPos - 1) && board.GetPosition(xPos + 1, yPos - 1))
        {
            SpawnMovePlate(xPos + 2, yPos - 2);
        }
        if (board.PositionOnBoardExists(xPos - 1, yPos - 1) && board.GetPosition(xPos - 1, yPos - 1))
        {
            SpawnMovePlate(xPos - 2, yPos - 2);
        }
    }

    private void ActivateMoveOneSquare()         // сделать шаг в любом направлении
    {
        SpawnMovePlate(xPos, yPos + 1);
        SpawnMovePlate(xPos, yPos - 1);
        SpawnMovePlate(xPos + 1, yPos);
        SpawnMovePlate(xPos - 1, yPos);
        SpawnMovePlate(xPos + 1, yPos + 1);
        SpawnMovePlate(xPos - 1, yPos - 1);
        SpawnMovePlate(xPos + 1, yPos - 1);
        SpawnMovePlate(xPos - 1, yPos + 1);
    }

    private void SpawnMovePlate(int x, int y)   // поместить на доске маркеры возможных ходов
    {
        if (board.PositionOnBoardExists(x, y))
        {
            if (board.GetPosition(x, y) == null)
            {
                var mp = Instantiate(movePlate, new Vector3(x, y, 0f), Quaternion.identity);
                mp.SetReference(gameObject);
                mp.SetCoords(x, y);
            }
        }
    }
    public void DestroyMovePlates()
    {
        MovePlate[] movePlates = FindObjectsOfType<MovePlate>();
        for (int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i].gameObject);
        }
    }

    public int GetPositionX()
    {
        return xPos;
    }
    public int GetPositionY()
    {
        return yPos;
    }
    public void SetPositionX(int x)
    {
        xPos = x;
    }
    public void SetPositionY(int y)
    {
        yPos = y;
    }
}
