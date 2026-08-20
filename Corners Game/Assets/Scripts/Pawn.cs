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

	private void ShowAvailableMoves()
	{
		List<Vector2Int> moves = board.GetAvailableMoves(xPos, yPos);

		foreach (Vector2Int move in moves)
		{
			SpawnMovePlate(move.x, move.y);
		}
	}

	private void SpawnMovePlate(int x, int y)
	{
		var mp = Instantiate(movePlate, new Vector3(x, y, 0f), Quaternion.identity);
		mp.SetReference(gameObject);
		mp.SetCoords(x, y);

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