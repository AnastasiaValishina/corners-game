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
	public CornersMode currentMode;

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

	public List<Vector2Int> GetAvailableMoves(int xPos, int yPos)
	{
		List<Vector2Int> moves = new List<Vector2Int>();

		// ==========================================
		// БАЗОВЫЕ ХОДЫ (Доступны в обоих режимах)
		// ==========================================

		// 1. Обычные шаги по вертикали и горизонтали
		AddMoveIfValid(moves, xPos, yPos + 1);
		AddMoveIfValid(moves, xPos, yPos - 1);
		AddMoveIfValid(moves, xPos + 1, yPos);
		AddMoveIfValid(moves, xPos - 1, yPos);


		// ==========================================
		// ПРЫЖКИ (Одиночные и серии)
		// ==========================================

		// Создаем список посещенных клеток, чтобы шашка не зациклилась, прыгая туда-сюда
		HashSet<Vector2Int> visitedSquares = new HashSet<Vector2Int>();
		visitedSquares.Add(new Vector2Int(xPos, yPos)); // Запрещаем прыгать обратно в стартовую точку

		if (currentMode == CornersMode.Classic)
		{
			// Запускаем рекурсивный поиск прыжков ТОЛЬКО по вертикали и горизонтали (false)
			FindChainJumps(xPos, yPos, visitedSquares, moves, false);
		}
		else if (currentMode == CornersMode.Diagonal)
		{
			// В диагональном режиме добавляем обычные диагональные шаги
			AddMoveIfValid(moves, xPos + 1, yPos + 1);
			AddMoveIfValid(moves, xPos - 1, yPos - 1);
			AddMoveIfValid(moves, xPos + 1, yPos - 1);
			AddMoveIfValid(moves, xPos - 1, yPos + 1);

			// Запускаем поиск прыжков ВО ВСЕХ направлениях (true)
			FindChainJumps(xPos, yPos, visitedSquares, moves, true);
		}

		return moves;
	}

	// НОВЫЙ МЕТОД: Рекурсивный поиск цепочки прыжков
	private void FindChainJumps(int currentX, int currentY, HashSet<Vector2Int> visited, List<Vector2Int> moves, bool includeDiagonal)
	{
		// Массив направлений: Вверх, Вниз, Вправо, Влево
		List<Vector2Int> directions = new List<Vector2Int>
	{
		new Vector2Int(0, 1), new Vector2Int(0, -1),
		new Vector2Int(1, 0), new Vector2Int(-1, 0)
	};

		// Если включен диагональный режим, добавляем диагонали для прыжков
		if (includeDiagonal)
		{
			directions.Add(new Vector2Int(1, 1)); directions.Add(new Vector2Int(-1, -1));
			directions.Add(new Vector2Int(1, -1)); directions.Add(new Vector2Int(-1, 1));
		}

		// Проверяем каждое направление
		foreach (Vector2Int dir in directions)
		{
			int jumpOverX = currentX + dir.x;
			int jumpOverY = currentY + dir.y;
			int landX = currentX + dir.x * 2;
			int landY = currentY + dir.y * 2;

			// 1. Проверяем, есть ли на соседней клетке шашка, через которую можно перепрыгнуть
			if (PositionOnBoardExists(jumpOverX, jumpOverY) && GetPosition(jumpOverX, jumpOverY) != null)
			{
				// 2. Проверяем, не выходит ли клетка ПРИЗЕМЛЕНИЯ за пределы доски
				if (PositionOnBoardExists(landX, landY))
				{
					Vector2Int landPos = new Vector2Int(landX, landY);

					// 3. Проверяем, свободна ли клетка приземления и не были ли мы там в текущем ходе
					if (GetPosition(landX, landY) == null && !visited.Contains(landPos))
					{
						// Запоминаем, что мы тут были
						visited.Add(landPos);

						// Добавляем эту клетку как доступный ход
						if (!moves.Contains(landPos))
						{
							moves.Add(landPos);
						}

						// САМАЯ ГЛАВНАЯ МАГИЯ (Рекурсия): 
						// Спрашиваем: "А могу ли я прыгнуть куда-то еще ИЗ ЭТОЙ НОВОЙ ТОЧКИ?"
						FindChainJumps(landX, landY, visited, moves, includeDiagonal);
					}
				}
			}
		}
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

public enum CornersMode
{
	Classic,
	Diagonal
}
