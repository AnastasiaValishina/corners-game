using System.Collections.Generic;
using UnityEngine;

public class BotController : MonoBehaviour
{
	int smart = 0;
	int notSmart = 0;
	public enum BotDifficulty { Easy, Medium, Hard }

	[Header("Настройки бота")]
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
		Pawn[] allPawns = FindObjectsOfType<Pawn>();

		foreach (Pawn pawn in allPawns)
		{
			if (pawn.name == "black")
			{
				List<Vector2Int> moves = Board.Instance.GetAvailableMoves(pawn.XPos, pawn.YPos);
				foreach (Vector2Int move in moves)
				{
					allAvailableMoves.Add(new BotMove { pawn = pawn, targetPos = move });
				}
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
			ExecuteMove(randomMove.pawn, randomMove.targetPos);
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
				bestMove = move;
				bestPawn = pawn;
			}
		}

		if (bestPawn != null && bestMove.x != -1)
		{
			ExecuteMove(bestPawn, bestMove);
		}
	}

	private void ExecuteMove(Pawn pawn, Vector2Int move)
	{
		AudioPlayer.Instance.PlaySlideSound();
		Board.Instance.SetPositionEmpty(pawn.XPos, pawn.YPos);
		pawn.MoveTo(move.x, move.y);
		Board.Instance.SetPosition(pawn.gameObject);
		Board.Instance.CheckWinner();
		GameController.Instance.NextTurn();
	}

	private struct BotMove
	{
		public Pawn pawn;
		public Vector2Int targetPos;
	}
}