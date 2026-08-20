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

	// Теперь мы возвращаем список маршрутов (каждый маршрут — это список точек)
	public List<List<Vector2Int>> GetAvailableMoves(int xPos, int yPos)
	{
		List<List<Vector2Int>> allPaths = new List<List<Vector2Int>>();

		// 1. Обычные шаги (длина маршрута = 1 шаг)
		AddSimpleMove(allPaths, xPos, yPos + 1);
		AddSimpleMove(allPaths, xPos, yPos - 1);
		AddSimpleMove(allPaths, xPos + 1, yPos);
		AddSimpleMove(allPaths, xPos - 1, yPos);

		bool includeDiag = (currentMode == CornersMode.Diagonal);

		if (includeDiag)
		{
			AddSimpleMove(allPaths, xPos + 1, yPos + 1);
			AddSimpleMove(allPaths, xPos - 1, yPos - 1);
			AddSimpleMove(allPaths, xPos + 1, yPos - 1);
			AddSimpleMove(allPaths, xPos - 1, yPos + 1);
		}

		// 2. Серии прыжков
		HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
		visited.Add(new Vector2Int(xPos, yPos)); // Стартовую точку отмечаем посещенной

		// Пустой маршрут для старта рекурсии
		List<Vector2Int> startingPath = new List<Vector2Int>();

		// Запускаем поиск прыжков
		FindChainJumps(xPos, yPos, visited, allPaths, startingPath, includeDiag);

		return allPaths;
	}

	// Вспомогательный метод для обычных шагов
	private void AddSimpleMove(List<List<Vector2Int>> allPaths, int x, int y)
	{
		if (PositionOnBoardExists(x, y) && GetPosition(x, y) == null)
		{
			// Создаем маршрут из одной точки и добавляем в общий список
			allPaths.Add(new List<Vector2Int> { new Vector2Int(x, y) });
		}
	}

	// Обновленная рекурсия, которая запоминает путь
	private void FindChainJumps(int currentX, int currentY, HashSet<Vector2Int> visited,
								List<List<Vector2Int>> allPaths, List<Vector2Int> currentPath, bool includeDiag)
	{
		List<Vector2Int> directions = new List<Vector2Int>
	{
		new Vector2Int(0, 1), new Vector2Int(0, -1),
		new Vector2Int(1, 0), new Vector2Int(-1, 0)
	};

		if (includeDiag)
		{
			directions.Add(new Vector2Int(1, 1)); directions.Add(new Vector2Int(-1, -1));
			directions.Add(new Vector2Int(1, -1)); directions.Add(new Vector2Int(-1, 1));
		}

		foreach (Vector2Int dir in directions)
		{
			int jumpOverX = currentX + dir.x;
			int jumpOverY = currentY + dir.y;
			int landX = currentX + dir.x * 2;
			int landY = currentY + dir.y * 2;

			if (PositionOnBoardExists(jumpOverX, jumpOverY) && GetPosition(jumpOverX, jumpOverY) != null)
			{
				if (PositionOnBoardExists(landX, landY))
				{
					Vector2Int landPos = new Vector2Int(landX, landY);

					if (GetPosition(landX, landY) == null && !visited.Contains(landPos))
					{
						visited.Add(landPos);

						// ВАЖНО: Создаем копию текущего пути и добавляем новую точку приземления
						List<Vector2Int> newPath = new List<Vector2Int>(currentPath);
						newPath.Add(landPos);

						// Сохраняем этот новый маршрут как доступный ход
						allPaths.Add(newPath);

						// Прыгаем дальше, передавая уже обновленный путь!
						FindChainJumps(landX, landY, visited, allPaths, newPath, includeDiag);
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
