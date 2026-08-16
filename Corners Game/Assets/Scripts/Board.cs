using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] GameObject pawnBlackPrefab;
    [SerializeField] GameObject pawnWhitePrefab;
    [SerializeField] Transform pawnsContainer;

    GameObject[,] squares;
	public List<Pawn> BotPawns = new List<Pawn>();

	int width = 8;
    int height = 8;

	public static Board Instance { get; private set; }
	private void Awake()
	{
		if (Instance == null) Instance = this;
		else Destroy(gameObject);
	}

	void Start()
    {
        squares = new GameObject[width, height];
    }

    public void SetBoard()
    {
		ClearBoard();
		PlacePawns();
    }

	void ClearBoard()
	{
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				if (squares[x, y] != null)
				{
					Destroy(squares[x, y]); // Удаляем объект со сцены
					squares[x, y] = null;   // Очищаем ссылку в массиве
				}
			}
		}
		BotPawns.Clear();
	}

	private void PlacePawns()
    {
        // пешки первого игока
        for (int x = 5; x <= 7; x++)
        {
            for (int y = 0; y <= 2; y++)
            {
                Vector2 pawnPos = new Vector2(x, y);
                GameObject pawn = Instantiate(pawnWhitePrefab, pawnPos, Quaternion.identity);
                pawn.transform.parent = pawnsContainer;
                pawn.name = "white";
                squares[x, y] = pawn;
            }
        }

        // пешки второго игрока
        for (int x = 0; x <= 2; x++)
        {
            for (int y = 5; y <= 7; y++)
            {
                Vector2 pawnPos = new Vector2(x, y);
                GameObject pawn = Instantiate(pawnBlackPrefab, pawnPos, Quaternion.identity);
                pawn.transform.parent = pawnsContainer;
                pawn.name = "black";
                squares[x, y] = pawn;

				BotPawns.Add(pawn.GetComponent<Pawn>());
			}
        }
    }

    public void SetPositionEmpty(int x, int y)  
    {
        squares[x, y] = null;
    }

    public GameObject GetPosition(int x, int y)
    {
        return squares[x, y];
    }
       
    public bool PositionOnBoardExists(int x, int y) 
    {
        if (x < 0 || y < 0 || x >= squares.GetLength(0) || y >= squares.GetLength(1))
        {
            return false;
        }
        else { return true; }
    }

    public void SetPosition(GameObject obj) // отметить, что квадрат занят определенной пешкой
    {
        Pawn pawn = obj.GetComponent<Pawn>();
        squares[pawn.XPos, pawn.YPos] = obj;
    }

    public void CheckWinner()               
    {
        if (GetPosition(0, 5) && GetPosition(0, 5).name == "white" &&
            GetPosition(0, 6) && GetPosition(0, 6).name == "white" &&
            GetPosition(0, 7) && GetPosition(0, 7).name == "white" &&
            GetPosition(1, 5) && GetPosition(1, 5).name == "white" &&
            GetPosition(1, 6) && GetPosition(1, 6).name == "white" &&
            GetPosition(1, 7) && GetPosition(1, 7).name == "white" &&
            GetPosition(2, 5) && GetPosition(2, 5).name == "white" &&
            GetPosition(2, 6) && GetPosition(2, 6).name == "white" &&
            GetPosition(2, 7) && GetPosition(2, 7).name == "white")
        {
			GameController.Instance.Winner(1); 
		}
        if (GetPosition(5, 0) && GetPosition(5, 0).name == "black" &&
            GetPosition(5, 1) && GetPosition(5, 1).name == "black" &&
            GetPosition(5, 2) && GetPosition(5, 2).name == "black" &&
            GetPosition(6, 0) && GetPosition(6, 0).name == "black" &&
            GetPosition(6, 1) && GetPosition(6, 1).name == "black" &&
            GetPosition(6, 2) && GetPosition(6, 2).name == "black" &&
            GetPosition(7, 0) && GetPosition(7, 0).name == "black" &&
            GetPosition(7, 1) && GetPosition(7, 1).name == "black" &&
            GetPosition(7, 2) && GetPosition(7, 2).name == "black")
        {
			GameController.Instance.Winner(2); 
        }
    }

	// Метод возвращает список всех возможных ходов для пешки на заданных координатах
	public List<Vector2Int> GetAvailableMoves(int xPos, int yPos)
	{
		List<Vector2Int> moves = new List<Vector2Int>();

		bool canMoveOne = GameController.Instance.CanMoveOneSquare;
		bool canJumpLine = GameController.Instance.CanJumpLine;
		bool canJumpDiag = GameController.Instance.CanJumpDiagonal;

		// 1. Обычные шаги на соседние клетки
		if (canMoveOne)
		{
			AddMoveIfValid(moves, xPos, yPos + 1);
			AddMoveIfValid(moves, xPos, yPos - 1);
			AddMoveIfValid(moves, xPos + 1, yPos);
			AddMoveIfValid(moves, xPos - 1, yPos);
			AddMoveIfValid(moves, xPos + 1, yPos + 1);
			AddMoveIfValid(moves, xPos - 1, yPos - 1);
			AddMoveIfValid(moves, xPos + 1, yPos - 1);
			AddMoveIfValid(moves, xPos - 1, yPos + 1);
		}

		// 2. Прыжки по вертикали и горизонтали
		if (canJumpLine)
		{
			if (PositionOnBoardExists(xPos + 1, yPos) && GetPosition(xPos + 1, yPos))
				AddMoveIfValid(moves, xPos + 2, yPos);

			if (PositionOnBoardExists(xPos, yPos + 1) && GetPosition(xPos, yPos + 1))
				AddMoveIfValid(moves, xPos, yPos + 2);

			if (PositionOnBoardExists(xPos - 1, yPos) && GetPosition(xPos - 1, yPos))
				AddMoveIfValid(moves, xPos - 2, yPos);

			if (PositionOnBoardExists(xPos, yPos - 1) && GetPosition(xPos, yPos - 1))
				AddMoveIfValid(moves, xPos, yPos - 2);
		}

		// 3. Прыжки по диагонали
		if (canJumpDiag)
		{
			if (PositionOnBoardExists(xPos + 1, yPos + 1) && GetPosition(xPos + 1, yPos + 1))
				AddMoveIfValid(moves, xPos + 2, yPos + 2);

			if (PositionOnBoardExists(xPos - 1, yPos + 1) && GetPosition(xPos - 1, yPos + 1))
				AddMoveIfValid(moves, xPos - 2, yPos + 2);

			if (PositionOnBoardExists(xPos + 1, yPos - 1) && GetPosition(xPos + 1, yPos - 1))
				AddMoveIfValid(moves, xPos + 2, yPos - 2);

			if (PositionOnBoardExists(xPos - 1, yPos - 1) && GetPosition(xPos - 1, yPos - 1))
				AddMoveIfValid(moves, xPos - 2, yPos - 2);
		}

		return moves;
	}

	// Вспомогательный метод для добавления координат в список
	private void AddMoveIfValid(List<Vector2Int> moves, int targetX, int targetY)
	{
		// Проверяем, существует ли клетка на доске и свободна ли она
		if (PositionOnBoardExists(targetX, targetY))
		{
			if (GetPosition(targetX, targetY) == null)
			{
				moves.Add(new Vector2Int(targetX, targetY));
			}
		}
	}
}
