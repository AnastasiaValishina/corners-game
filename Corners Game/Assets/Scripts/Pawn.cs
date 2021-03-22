using UnityEngine;

public class Pawn : MonoBehaviour
{
    [SerializeField] private MovePlate movePlate;
    int xPos;
    int yPos;
    
    string player;
    Board board;
    GameController gameController;

    void Start()
    {
        board = FindObjectOfType<Board>();
        gameController = FindObjectOfType<GameController>();
        xPos = (int)transform.position.x;
        yPos = (int)transform.position.y;

        switch (name)
        {
            case "white": player = gameController.GetPlayerOneName();
                break;
            case "black": player = gameController.GetPlayerTwoName();
                break;
        }
    }

    public void SetCoords()
    {
        transform.position = new Vector3(xPos, yPos, 0f);
    }

    private void OnMouseDown()
    {
        if (!gameController.IsGameOver() && gameController.GetCurrentPlayer() == player)
        {
            DestroyMovePlates();
            InitiateMovePlates();
        }
    }

    private void InitiateMovePlates()
    {
        if (gameController.CanMoveOneSquare())
        {
            ActivateMoveOneSquare();
        }

        if (gameController.CanJumpDiagonal())
        {
            ActivateJumpOverDiag();
        }

        if (gameController.CanJumpLine())
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
