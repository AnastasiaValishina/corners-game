using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovePlate : MonoBehaviour, IPointerClickHandler
{
    GameObject reference = null;
	private List<Vector2Int> pathToHere;

	int plateX;
    int plateY;

	public void OnPointerClick(PointerEventData eventData)
	{
		AudioPlayer.Instance.PlaySlideSound();
		Pawn pawnScript = reference.GetComponent<Pawn>();

		// 1. СРАЗУ освобождаем старую клетку, пока пешка не улетела
		Board.Instance.SetPositionEmpty(pawnScript.XPos, pawnScript.YPos);

		// 2. Убираем маркеры ходов
		pawnScript.DestroyMovePlates();

		// 3. Даем пешке команду прыгать. 
		// Все остальные действия она сделает сама в конце пути!
		pawnScript.MoveInSteps(pathToHere);
	}

	public void SetCoords(int x, int y)
    {
        plateX = x;
        plateY = y;
    }

    public void SetReference (GameObject obj)
    {
        reference = obj;
    }

	public void SetPath(List<Vector2Int> path)
	{
		pathToHere = path;
	}
}
