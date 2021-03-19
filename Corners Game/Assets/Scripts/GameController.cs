using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Pawn pawnBlackPrefab;
    public Pawn pawnWhitePrefab;
    public Pawn[,] allPawns;

    Pawn[] playerBlack = new Pawn[9];
    Pawn[] playerWhite = new Pawn[9];
    bool isGameOver = false;

    void Start()
    {
        allPawns = new Pawn[8, 8];
        PlacePawns();
    }

    private void PlacePawns()
    {
        // place player 1 pawns
        for (int x = 5; x <= 7; x++)
        {
            for (int y = 0; y <= 2; y++)
            {
                Vector2 pawnPos = new Vector2(x, y);
                Pawn pawn = Instantiate(pawnWhitePrefab, pawnPos, Quaternion.identity);
                pawn.transform.parent = transform;
                pawn.name = "( " + x + ", " + y + " )";
                allPawns[x, y] = pawn;
            }
        }

        // place player 2 pawns
        for (int x = 0; x <= 2; x++)
        {
            for (int y = 5; y <= 7; y++)
            {
                Vector2 pawnPos = new Vector2(x, y);
                Pawn pawn = Instantiate(pawnBlackPrefab, pawnPos, Quaternion.identity);
                pawn.transform.parent = transform;
                pawn.name = "( " + x + ", " + y + " )";
                allPawns[x, y] = pawn;
            }
        }
    }
}
