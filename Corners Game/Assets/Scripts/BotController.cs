using System.Collections.Generic;
using UnityEngine;
public enum BotDifficulty { Easy, Medium, Hard }

public class BotController : MonoBehaviour
{
	int smart = 0;
	int notSmart = 0;

	public BotDifficulty currentDifficulty = BotDifficulty.Hard;

	Vector2Int targetCorner = new Vector2Int(7, 0);
	public static BotController Instance { get; private set; }

	private void Awake()
	{
		if (Instance == null) Instance = this;
		else Destroy(gameObject);
	}

	public void MakeSmartMove()
	{
		// 1. Собираем АБСОЛЮТНО ВСЕ возможные ходы для всех черных пешек
		List<BotMove> allAvailableMoves = new List<BotMove>();
		
		List<Pawn> myPawns = Board.Instance.BotPawns;

		foreach (Pawn pawn in myPawns)
		{
			// Теперь получаем список маршрутов
			List<List<Vector2Int>> paths = Board.Instance.GetAvailableMoves(pawn.XPos, pawn.YPos);

			foreach (List<Vector2Int> path in paths)
			{
				// Конечная цель хода — это всегда последняя точка в маршруте
				Vector2Int finalTarget = path[path.Count - 1];

				// Сохраняем и конечную точку (для расчетов), и весь маршрут (для анимации)
				allAvailableMoves.Add(new BotMove { pawn = pawn, targetPos = finalTarget, path = path });
			}
		}

		if (allAvailableMoves.Count == 0) return;

		bool isThinkingSmart = true;

		var v = Random.value;
		switch (currentDifficulty)
		{
			case BotDifficulty.Easy:
				isThinkingSmart = v > 0.6f; 
				break;
			case BotDifficulty.Medium:
				isThinkingSmart = v > 0.3f;
				break;
			case BotDifficulty.Hard:
				isThinkingSmart = true;
				break;
		}

		if (!isThinkingSmart)
		{
			notSmart++;
			List<BotMove> goodMoves = new List<BotMove>();
			List<BotMove> outsideMoves = new List<BotMove>();

			foreach (BotMove m in allAvailableMoves)
			{
				int currentX = m.pawn.XPos;
				int currentY = m.pawn.YPos;

				int currentDist = (targetCorner.x - currentX) + (currentY - targetCorner.y);
				int newDist = (targetCorner.x - m.targetPos.x) + (m.targetPos.y - targetCorner.y);

				bool isAlreadyInZone = (currentX >= 5 && currentY <= 2);
				bool willBeInZone = (m.targetPos.x >= 5 && m.targetPos.y <= 2);

				// 1. Строгий запрет: никогда не выходить из собранного дома
				if (isAlreadyInZone && !willBeInZone) continue;

				// 2. Разрешаем ход, если пешка идет вперед ИЛИ шевелится внутри дома (освобождая место)
				if (newDist <= currentDist || (isAlreadyInZone && willBeInZone))
				{
					goodMoves.Add(m);

					// Отдельно собираем ходы тех пешек, которые еще "на улице"
					if (!isAlreadyInZone)
					{
						outsideMoves.Add(m);
					}
				}
			}

			// ЛОГИКА ВЫБОРА: 
			// Пытаемся играть фигурами с улицы. Если они заблокированы - разрешаем 
			// перетасовку внутри дома, чтобы освободить проход.
			List<BotMove> poolToChooseFrom;

			if (outsideMoves.Count > 0)
			{
				poolToChooseFrom = outsideMoves;
			}
			else if (goodMoves.Count > 0)
			{
				poolToChooseFrom = goodMoves;
			}
			else
			{
				poolToChooseFrom = allAvailableMoves; // Экстренный запасной вариант
			}

			BotMove randomMove = poolToChooseFrom[Random.Range(0, poolToChooseFrom.Count)];
			ExecuteMove(randomMove.pawn, randomMove);
		}
		else
		{
			ExecuteSmartHeuristic(allAvailableMoves);
		}
		Debug.Log("smart = " + smart + ". Not smart = " + notSmart);
	}

	private void ExecuteSmartHeuristic(List<BotMove> allAvailableMoves)
	{
		smart++;
		Pawn bestPawn = null;
		Vector2Int bestMove = new Vector2Int(-1, -1);
		int bestScore = 999999;

		BotMove bestBotMove = new BotMove { pawn = null };

		foreach (BotMove botMove in allAvailableMoves)
		{
			Pawn pawn = botMove.pawn;
			Vector2Int move = botMove.targetPos;

			int currentX = pawn.XPos;
			int currentY = pawn.YPos;

			int currentCornerDist = (targetCorner.x - currentX) + (currentY - targetCorner.y);
			bool isAlreadyInZone = (currentX >= 5 && currentY <= 2);

			int newCornerDist = (targetCorner.x - move.x) + (move.y - targetCorner.y);
			int distanceDelta = newCornerDist - currentCornerDist;
			bool willBeInZone = (move.x >= 5 && move.y <= 2);

			int score = 0;

			if (!isAlreadyInZone && willBeInZone) score -= 2000;
			if (isAlreadyInZone && !willBeInZone) score += 5000;
			score += (distanceDelta * 100);
			if (!isAlreadyInZone) score -= 50;
			score -= currentCornerDist;

			if (score < bestScore)
			{
				bestScore = score;
				bestBotMove = botMove; 
				bestPawn = pawn;
			}
		}
		if (bestPawn != null && bestBotMove.targetPos.x != -1) // Проверяем, что ход найден
		{
			ExecuteMove(bestPawn, bestBotMove);
		}
	}

	private void ExecuteMove(Pawn pawn, BotMove botMove)
	{
		AudioPlayer.Instance.PlaySlideSound();
		Board.Instance.SetPositionEmpty(pawn.XPos, pawn.YPos);
		pawn.MoveInSteps(botMove.path);
	}

	public void SetDifficulty(BotDifficulty difficulty)
	{
		currentDifficulty = difficulty;
	}

	private struct BotMove
	{
		public Pawn pawn;
		public Vector2Int targetPos;
		public List<Vector2Int> path; 
	}
}