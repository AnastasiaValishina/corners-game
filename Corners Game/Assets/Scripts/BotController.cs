using System.Collections.Generic;
using UnityEngine;

public class BotController : MonoBehaviour
{
	// Целевая точка для черных пешек
	Vector2Int targetCorner = new Vector2Int(7, 0);

	public static BotController Instance { get; private set; }
	private void Awake()
	{
		if (Instance == null) Instance = this;
		else Destroy(gameObject);
	}

	public void MakeSmartMove()
	{
		Pawn bestPawn = null;
		Vector2Int bestMove = new Vector2Int(-1, -1);
		int bestScore = 999999; // Сильно увеличили стартовое значение

		// 1. Находим все черные пешки на сцене
		Pawn[] allPawns = FindObjectsOfType<Pawn>();

		foreach (Pawn pawn in allPawns)
		{
			if (pawn.name == "black")
			{
				// Получаем доступные ходы от доски
				List<Vector2Int> possibleMoves = Board.Instance.GetAvailableMoves(pawn.GetPositionX(), pawn.GetPositionY());

				// ТЕКУЩАЯ дистанция именно этой пешки до цели
				int currentDistance = (targetCorner.x - pawn.GetPositionX()) + (pawn.GetPositionY() - targetCorner.y);

				// Проверяем, находится ли пешка УЖЕ в победной зоне (дом черных)
				bool isAlreadyInZone = (pawn.GetPositionX() >= 5 && pawn.GetPositionY() <= 2);

				foreach (Vector2Int move in possibleMoves)
				{
					// НОВАЯ дистанция после предполагаемого хода
					int newDistance = (targetCorner.x - move.x) + (move.y - targetCorner.y);

					// Разница (отрицательное число = приблизились к цели, положительное = отдалились)
					int distanceDelta = newDistance - currentDistance;

					// Штраф, если пешка попытается выйти из победной зоны
					bool leavesZone = isAlreadyInZone && !(move.x >= 5 && move.y <= 2);
					int penalty = leavesZone ? 5000 : 0;

					// УМНАЯ ФОРМУЛА:
					// 1. distanceDelta * 100 -> Приоритет большим прыжкам вперед.
					// 2. - currentDistance -> При прочих равных приоритет пешке, которая дальше всего!
					// 3. + penalty -> Огромный штраф за выход из собранного дома.
					int score = (distanceDelta * 100) - currentDistance + penalty;

					// Ищем наименьший счет
					if (score < bestScore)
					{
						bestScore = score;
						bestMove = move;
						bestPawn = pawn;
					}
				}
			}
		}

		// Совершаем лучший найденный ход
		if (bestPawn != null && bestMove.x != -1)
		{
			ExecuteMove(bestPawn, bestMove);
		}
	}

	private void ExecuteMove(Pawn pawn, Vector2Int move)
	{
		// Программно перемещаем пешку и передаем ход человеку
		Board.Instance.SetPositionEmpty(pawn.GetPositionX(), pawn.GetPositionY());

		pawn.SetPositionX(move.x);
		pawn.SetPositionY(move.y);
		pawn.SetCoords();

		Board.Instance.SetPosition(pawn.gameObject);
		Board.Instance.CheckWinner();
		GameController.Instance.NextTurn();
	}
}