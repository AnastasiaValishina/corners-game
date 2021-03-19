using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject squareBlackPrefab;
    public GameObject squareWhitePrefab;

    int width = 8;
    int height = 8;

    GameObject[,] squares;
    void Start()
    {
        squares = new GameObject[width, height];
        CreateBoard();
    }

    private void CreateBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 posBlack = new Vector2(x, y);
                GameObject squareBlack = Instantiate(squareBlackPrefab, posBlack, Quaternion.identity) as GameObject;
                squareBlack.transform.parent = transform;
                squareBlack.name = "( " + x + ", " + y + " )";
                y++;
                Vector2 posWhite = new Vector2(x, y);
                GameObject squareWhite = Instantiate(squareWhitePrefab, posWhite, Quaternion.identity) as GameObject;
                squareWhite.transform.parent = transform;
                squareWhite.name = "( " + x + ", " + y + " )";
            }
            x++;
            for (int y = 0; y < height; y++)
            {
                Vector2 posBlack = new Vector2(x, y);
                GameObject squareWhite = Instantiate(squareWhitePrefab, posBlack, Quaternion.identity) as GameObject;
                squareWhite.transform.parent = transform;
                squareWhite.name = "( " + x + ", " + y + " )";
                y++;
                Vector2 posWhite = new Vector2(x, y);
                GameObject squareBlack = Instantiate(squareBlackPrefab, posWhite, Quaternion.identity) as GameObject;
                squareBlack.transform.parent = transform;
                squareBlack.name = "( " + x + ", " + y + " )";
            }
        }
    }
}
