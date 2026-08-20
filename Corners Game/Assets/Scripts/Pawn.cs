using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pawn : MonoBehaviour, IPointerClickHandler
{
	[SerializeField] private MovePlate movePlate;

	[Header("Настройки анимации LeanTween")]
	[SerializeField] private float moveDuration = 0.3f;
	[SerializeField] private LeanTweenType moveEase = LeanTweenType.easeInOutQuad;

	int xPos;
	int yPos;

	int player;
	Board board;
	GameController gameController;

	public int XPos { get => xPos; set => xPos = value; }
	public int YPos { get => yPos; set => yPos = value; }

	private static List<MovePlate> activeMovePlates = new List<MovePlate>();

	void Start()
	{
		board = Board.Instance;
		gameController = GameController.Instance;
		xPos = (int)transform.position.x;
		yPos = (int)transform.position.y;

		switch (name)
		{
			case "white":
				player = 1;
				break;
			case "black":
				player = 2;
				break;
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		DestroyMovePlates();

		if (!gameController.IsGameOver && gameController.CurrentPlayer == player)
		{
			// БЛОКИРОВКА: Если это черная пешка, и сейчас играет бот — запрещаем ручной клик
			if (gameController.IsBotActive && name == "black")
			{
				return;
			}

			AudioPlayer.Instance.PlayPawnClick();

			ShowAvailableMoves();
		}
	}

	public void MoveTo(int x, int y)
	{
		LeanTween.cancel(gameObject);

		Vector3 finalPos = new Vector3(x, y, transform.position.z);

		LeanTween.move(gameObject, finalPos, moveDuration)
			.setEase(moveEase);

		xPos = x;
		yPos = y;
	}

	public void MoveInSteps(List<Vector2Int> path)
	{
		LeanTween.cancel(gameObject);

		// Создаем очередь анимаций (секвенцию)
		LTSeq sequence = LeanTween.sequence();

		foreach (Vector2Int step in path)
		{
			Vector3 targetPos = new Vector3(step.x, step.y, transform.position.z);

			// ВАЖНО: Кладем само движение ВНУТРЬ sequence.append()
			// Теперь LeanTween будет ждать окончания прыжка перед следующим шагом!
			sequence.append(LeanTween.move(gameObject, targetPos, 0.25f).setEase(LeanTweenType.easeInOutQuad));

			// Небольшая пауза на каждой клетке, чтобы прыжки читались четче
			sequence.append(0.05f);
		}

		// 4. Действия после полного завершения цепочки прыжков
		sequence.append(() =>
		{
			// Обновляем внутренние координаты пешки только когда она дошла до финиша
			xPos = path[path.Count - 1].x;
			yPos = path[path.Count - 1].y;

			Board.Instance.SetPosition(gameObject);

			Board.Instance.CheckWinner();
			GameController.Instance.NextTurn();
		});
	}

	private void ShowAvailableMoves()
	{
		// Получаем все МАРШРУТЫ
		List<List<Vector2Int>> allPaths = board.GetAvailableMoves(xPos, yPos);

		// Перебираем каждый маршрут и спавним для него плитку
		foreach (List<Vector2Int> path in allPaths)
		{
			SpawnMovePlate(path);
		}
	}

	// Теперь метод принимает маршрут целиком
	private void SpawnMovePlate(List<Vector2Int> path)
	{
		// Плитка должна появиться на ПОСЛЕДНЕЙ клетке маршрута
		Vector2Int finalPos = path[path.Count - 1];

		var mp = Instantiate(movePlate, new Vector3(finalPos.x, finalPos.y, 0f), Quaternion.identity);
		mp.SetReference(gameObject);

		// Передаем весь маршрут в плитку (этот метод мы сейчас создадим)
		mp.SetPath(path);

		activeMovePlates.Add(mp);
	}

	public void DestroyMovePlates()
	{
		for (int i = 0; i < activeMovePlates.Count; i++)
		{
			if (activeMovePlates[i] != null)
			{
				Destroy(activeMovePlates[i].gameObject);
			}
		}

		activeMovePlates.Clear();
	}

	private void OnDestroy()
	{
		DestroyMovePlates();
	}
}