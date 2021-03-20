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

    }

   
}
