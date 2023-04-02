//This script determines the game's rules and creates the pieces
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Game : MonoBehaviour
{
    public GameObject controller;
    public static bool didAttack = false;
    public static bool canAttack = false;

    //Reference from Unity IDE
    public GameObject chesspiece;

    //Matrices needed, positions of each of the GameObjects
    //Also separate arrays for the players in order to easily keep track of them all
    //Keep in mind that the same objects are going to be in "positions" and "playerBlack"/"playerWhite"
    private GameObject[,] positions = new GameObject[8, 8];
    private GameObject[] playerBlack = new GameObject[16];
    private GameObject[] playerWhite = new GameObject[16];

    //current turn
    private string currentPlayer = "white";

    //Game Ending
    private bool gameOver = false;

    public int count = 0;

    //Unity calls this right when the game starts, there are a few built in functions
    //that Unity can call for you
    public void Start()
    {
        playerWhite = new GameObject[] { Create("white_rook",0, 0), Create("white_knight",1,0),
            Create("white_bishop",2,0), Create("white_queen",3, 0), Create("white_king", 4, 0),
            Create("white_bishop",5,0), Create("white_knight",6,0), Create("white_rook", 7, 0),
            Create("white_pawn", 0, 1), Create("white_pawn", 1, 1), Create("white_pawn", 2, 1),
            Create("white_pawn", 3, 1), Create("white_pawn", 4, 1), Create("white_pawn", 5, 1),
            Create("white_pawn", 6, 1), Create("white_pawn", 7, 1) };
        playerBlack = new GameObject[] { Create("black_rook",0, 7), Create("black_knight",1,7),
            Create("black_bishop",2,7), Create("black_queen",3, 7), Create("black_king", 4, 7),
            Create("black_bishop",5,7), Create("black_knight",6,7), Create("black_rook", 7, 7),
            Create("black_pawn", 0, 6), Create("black_pawn", 1, 6), Create("black_pawn", 2, 6),
            Create("black_pawn", 3, 6), Create("black_pawn", 4, 6), Create("black_pawn", 5, 6),
            Create("black_pawn", 6, 6), Create("black_pawn", 7, 6) };

        //Set all piece positions on the positions board
        for (int i = 0; i < playerBlack.Length; i++)
        {
            SetPosition(playerBlack[i]);
            SetPosition(playerWhite[i]);
        }
    }

    public GameObject Create(string name, int x, int y)
    {
        //Used to create the pieces at the start of the game
        GameObject obj = Instantiate(chesspiece, new Vector3(0, 0, -1), Quaternion.identity);
        Chessman cm = obj.GetComponent<Chessman>(); //We have access to the GameObject, we need the script
        cm.name = name; //This is a built in variable that Unity has, so we did not have to declare it before
        cm.SetXBoard(x);
        cm.SetYBoard(y);
        cm.Activate(); //It has everything set up so it can now Activate()
        return obj;
    }

    public void SetPosition(GameObject obj)
    {
        Chessman cm = obj.GetComponent<Chessman>();

        //Overwrites either empty space or whatever was there
        positions[cm.GetXBoard(), cm.GetYBoard()] = obj;
    }

    public void SetPosition2(GameObject obj, int xnew, int ynew)
    {
        //Used for AI
        Chessman cm = obj.GetComponent<Chessman>();
        positions[xnew, ynew] = obj;
    }

    public void SetPositionEmpty(int x, int y)
    {
        positions[x, y] = null;
    }

    public GameObject GetPosition(int x, int y)
    {
        //Accesses the piece in the named position
        return positions[x, y];
    }

    public GameObject[,] GetPositions()
    {
        //Returns the list of positions
        return positions;
    }

    public bool PositionOnBoard(int x, int y)
    {
        //Returns true if the position is on the board and false if not
        if (x < 0 || y < 0 || x >= positions.GetLength(0) || y >= positions.GetLength(1)) return false;
        return true;
    }

    public string GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    //public bool CanAttack()
    //{
        
    //}

    public void NextTurn()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        //if (controller.GetComponent<Game>().GetCount() % 8 >= 4 && didAttack) 
        //{
            //if (CanAttack())
            //{

            //}
            //else
            //{
                //count += 1;

                //if (count % 2 == 1)
                //{
                    //currentPlayer = "black";
                //}
                //else { currentPlayer = "white"; }
            //}
        //}
        //else
        //{
            count += 1;

            if (count % 2 == 1)
            {
                currentPlayer = "black";
            }
            else { currentPlayer = "white"; }
        //}
    }

    public void Update()
    {
        //Restarts the game by loading the scene over again
        if (gameOver == true && Input.GetMouseButtonDown(0))
        {
            gameOver = false;

            SceneManager.LoadScene("Game");
        }

        for (int i = 0; i < 8; i++)
        {
            if (positions[i, 7] != null && positions[i, 7].name == "white_pawn")
            {
                Destroy(positions[i, 7]);
                Create("white_knight", i, 7);
            }
        }
    }

    public void Winner(string playerWinner)
    {
        gameOver = true;

        if (currentPlayer == "white")
        {
            GameObject winner = GameObject.FindGameObjectWithTag("WhiteWinner");

            winner.transform.position = new Vector3(0, 0.7f, -2);
        }
        else
        {
            GameObject winner = GameObject.FindGameObjectWithTag("BlackWinner");

            winner.transform.position = new Vector3(0, 0.7f, -2);
        }
    }

    public int GetCount()
    {
        return count;
    }
}
