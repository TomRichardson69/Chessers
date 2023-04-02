using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chessman : MonoBehaviour
{
    //References to objects in our Unity Scene
    public GameObject controller;
    public GameObject movePlate;

    //Position for this Chesspiece on the Board
    //The correct position will be set later
    private int xBoard = -1;
    private int yBoard = -1;

    //Variable for keeping track of the player it belongs to "black" or "white"
    private string player;

    //References to all the possible Sprites that this Chesspiece could be
    public Sprite black_queen, black_knight, black_bishop, black_king, black_rook, black_pawn;
    public Sprite white_queen, white_knight, white_bishop, white_king, white_rook, white_pawn;

    public void Activate()
    {
        //Get the game controller
        controller = GameObject.FindGameObjectWithTag("GameController");

        //Take the instantiated location and adjust transform
        SetCoords();

        //Choose correct sprite based on piece's name
        switch (this.name)
        {
            case "black_queen": this.GetComponent<SpriteRenderer>().sprite = black_queen; player = "black"; break;
            case "black_knight": this.GetComponent<SpriteRenderer>().sprite = black_knight; player = "black"; break;
            case "black_bishop": this.GetComponent<SpriteRenderer>().sprite = black_bishop; player = "black"; break;
            case "black_king": this.GetComponent<SpriteRenderer>().sprite = black_king; player = "black"; break;
            case "black_rook": this.GetComponent<SpriteRenderer>().sprite = black_rook; player = "black"; break;
            case "black_pawn": this.GetComponent<SpriteRenderer>().sprite = black_pawn; player = "black"; break;
            case "white_queen": this.GetComponent<SpriteRenderer>().sprite = white_queen; player = "white"; break;
            case "white_knight": this.GetComponent<SpriteRenderer>().sprite = white_knight; player = "white"; break;
            case "white_bishop": this.GetComponent<SpriteRenderer>().sprite = white_bishop; player = "white"; break;
            case "white_king": this.GetComponent<SpriteRenderer>().sprite = white_king; player = "white"; break;
            case "white_rook": this.GetComponent<SpriteRenderer>().sprite = white_rook; player = "white"; break;
            case "white_pawn": this.GetComponent<SpriteRenderer>().sprite = white_pawn; player = "white"; break;
        }
    }

    public void SetCoords()
    {
        //Get the board value in order to convert to xy coords
        float x = xBoard;
        float y = yBoard;

        //Adjust by variable offset
        x *= 1.16f;
        y *= 1.16f;

        //Add constants (pos 0,0)
        x += -4.1f;
        y += -4.05f;

        //Set actual unity values
        this.transform.position = new Vector3(x, y, -1.0f);
    }

    public int GetXBoard()
    {
        return xBoard;
    }
    public int GetYBoard()
    {
        return yBoard;
    }
    public void SetXBoard(int x)
    {
        xBoard = x;
    }
    public void SetYBoard(int y)
    {
        yBoard = y;
    }

    void Update()
    {
        GameObject checkers = GameObject.FindGameObjectWithTag("CheckersMode");
        GameObject chess = GameObject.FindGameObjectWithTag("ChessMode");

        if (controller.GetComponent<Game>().GetCount() % 8 >= 4)
        {
            checkers.transform.position = new Vector3(-6.278f, 3.51f, 0);
            chess.transform.position = new Vector3(-6.278f, 10, 0);
        }
        else
        {
            checkers.transform.position = new Vector3(-6.278f, 10, 0);
            chess.transform.position = new Vector3(-6.278f, 3.51f, 0);
        }
    }

    private void OnMouseUp()
    {
        if (!controller.GetComponent<Game>().IsGameOver() && controller.GetComponent<Game>().GetCurrentPlayer() == player)
        {
            //Remove all moveplates relating to previously selected piece
            DestroyMovePlates();

            //Create new MovePlates
            InitiateMovePlates();
        }
    }
    public void DestroyMovePlates()
    {
        //Destroy old MovePlates
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        for (int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]); // Be careful with this function "Destroy" it is asynchronous
        }
    }
    public void InitiateMovePlates()
    {
        //Spawns necessary moveplates
        if (controller.GetComponent<Game>().GetCount() % 8 >= 4)
        {
            switch (this.name)
            {
                case "white_pawn":
                    WCheckersMovePlate(xBoard, yBoard);
                    break;
                case "black_pawn":
                    BCheckersMovePlate(xBoard, yBoard);
                    break;
                default:
                    WCheckersMovePlate(xBoard, yBoard);
                    BCheckersMovePlate(xBoard, yBoard);
                    break;
            }
        }
        else
        {
            switch (this.name)
            {
                case "black_queen":
                case "white_queen":
                    LineMovePlate(1, 0);
                    LineMovePlate(0, 1);
                    LineMovePlate(1, 1);
                    LineMovePlate(-1, 0);
                    LineMovePlate(0, -1);
                    LineMovePlate(-1, -1);
                    LineMovePlate(-1, 1);
                    LineMovePlate(1, -1);
                    break;
                case "black_knight":
                case "white_knight":
                    LMovePlate();
                    break;
                case "black_bishop":
                case "white_bishop":
                    LineMovePlate(1, 1);
                    LineMovePlate(1, -1);
                    LineMovePlate(-1, 1);
                    LineMovePlate(-1, -1);
                    break;
                case "black_king":
                case "white_king":
                    SurroundMovePlate();
                    CastleMovePlate(xBoard, yBoard);
                    break;
                case "black_rook":
                case "white_rook":
                    LineMovePlate(1, 0);
                    LineMovePlate(0, 1);
                    LineMovePlate(-1, 0);
                    LineMovePlate(0, -1);
                    break;
                case "black_pawn":
                    PawnMovePlate(xBoard, yBoard - 1);
                    if (yBoard == 6)
                    {
                        PawnMovePlate(xBoard, yBoard - 2, true);
                    }
                    break;
                case "white_pawn":
                    PawnMovePlate(xBoard, yBoard + 1);
                    if (yBoard == 1)
                    {
                        PawnMovePlate(xBoard, yBoard + 2, true);
                    }
                    break;
            }
        }
    }

    public void WCheckersMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();

        if (sc.PositionOnBoard(x + 1, y + 1) && sc.GetPosition(x + 1, y + 1) == null) { MovePlateSpawn(x + 1, y + 1); }
        else if (sc.PositionOnBoard(x + 2, y + 2) && sc.GetPosition(x + 1, y + 1) != null && sc.GetPosition(x + 2, y + 2) == null && sc.GetPosition(x + 1, y + 1).name.Split('_')[0] != player)
        {
            MovePlateSpawn(x + 2, y + 2, true);
        }

        if (sc.PositionOnBoard(x - 1, y + 1) && sc.GetPosition(x - 1, y + 1) == null) { MovePlateSpawn(x - 1, y + 1); }
        else if (sc.PositionOnBoard(x - 2, y + 2) && sc.GetPosition(x - 1, y + 1) != null && sc.GetPosition(x - 2, y + 2) == null && sc.GetPosition(x - 1, y + 1).name.Split('_')[0] != player)
        {
            MovePlateSpawn(x - 2, y + 2, true);
        }
    }
    public void BCheckersMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x + 1, y - 1) && sc.GetPosition(x + 1, y - 1) == null) { MovePlateSpawn(x + 1, y - 1); }
        else if (sc.PositionOnBoard(x + 2, y - 2) && sc.GetPosition(x + 1, y - 1) != null && sc.GetPosition(x + 2, y - 2) == null && sc.GetPosition(x + 1, y - 1).name.Split('_')[0] != player)
        {
            MovePlateSpawn(x + 2, y - 2, true);
        }

        if (sc.PositionOnBoard(x - 1, y - 1) && sc.GetPosition(x - 1, y - 1) == null) { MovePlateSpawn(x - 1, y - 1); }
        else if (sc.PositionOnBoard(x - 2, y - 2) && sc.GetPosition(x - 1, y - 1) != null && sc.GetPosition(x - 2, y - 2) == null && sc.GetPosition(x - 1, y - 1).name.Split('_')[0] != player)
        {
            MovePlateSpawn(x - 2, y - 2, true);
        }
    }

    public void LineMovePlate(int xIncrement, int yIncrement)
    {
        Game sc = controller.GetComponent<Game>();

        int x = xBoard + xIncrement;
        int y = yBoard + yIncrement;

        while (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) == null)
        {
            MovePlateSpawn(x, y);
            x += xIncrement;
            y += yIncrement;
        }

        if (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y).GetComponent<Chessman>().player != player)
        {
            MovePlateSpawn(x, y, true);
        }
    }
    public void LMovePlate()
    {
        PointMovePlate(xBoard + 1, yBoard + 2);
        PointMovePlate(xBoard - 1, yBoard + 2);
        PointMovePlate(xBoard + 2, yBoard + 1);
        PointMovePlate(xBoard + 2, yBoard - 1);
        PointMovePlate(xBoard + 1, yBoard - 2);
        PointMovePlate(xBoard - 1, yBoard - 2);
        PointMovePlate(xBoard - 2, yBoard + 1);
        PointMovePlate(xBoard - 2, yBoard - 1);
    }
    public void SurroundMovePlate()
    {
        PointMovePlate(xBoard, yBoard + 1);
        PointMovePlate(xBoard, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard);
        PointMovePlate(xBoard - 1, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard + 1);
        PointMovePlate(xBoard + 1, yBoard);
        PointMovePlate(xBoard + 1, yBoard - 1);
        PointMovePlate(xBoard + 1, yBoard + 1);
    }
    public void PointMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();

        if (sc.PositionOnBoard(x, y))
        {
            GameObject cp = sc.GetPosition(x, y);

            if (cp == null)
            {
                MovePlateSpawn(x, y);
            }
            else if (cp.GetComponent<Chessman>().player != player)
            {
                MovePlateSpawn(x, y, true);
            }
        }
    }
    public void PawnMovePlate(int x, int y, bool doub = false)
    {
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x, y))
        {
            if (sc.GetPosition(x, y) == null)
            {
                if (doub && sc.GetPosition(x, y - 1) == null) { MovePlateSpawn(x, y); }
                else { MovePlateSpawn(x, y); }
            }

            if (!doub && sc.PositionOnBoard(x + 1, y) && sc.GetPosition(x + 1, y) != null && sc.GetPosition(x + 1, y).GetComponent<Chessman>().player != player)
            {
                MovePlateSpawn(x + 1, y, true);
            }

            if (!doub && sc.PositionOnBoard(x - 1, y) && sc.GetPosition(x - 1, y) != null && sc.GetPosition(x - 1, y).GetComponent<Chessman>().player != player)
            {
                MovePlateSpawn(x - 1, y, true);
            }

            if (y == 5)
            {
                if (sc.GetPosition(x - 1, y - 1) != null && sc.GetPosition(x - 1, y - 1).name == "black_pawn")
                {
                    MovePlateSpawn(x - 1, y, true, false, true);
                }
                else if (sc.GetPosition(x + 1, y - 1) != null && sc.GetPosition(x - 1, y - 1).name == "black_pawn")
                {
                    MovePlateSpawn(x + 1, y, true, false, true);
                }
            }
        }
    }
    public void CastleMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();

        if (x == 4 && y == 0)
        {
            if (sc.GetPosition(0, 0) != null && sc.GetPosition(0, 0).name == "white_rook" && sc.GetPosition(1, 0) == null && sc.GetPosition(2, 0) == null && sc.GetPosition(3, 0) == null)
            { MovePlateSpawn(2, 0, false, true); }
            if (sc.GetPosition(7, 0) != null && sc.GetPosition(7, 0).name == "white_rook" && sc.GetPosition(5, 0) == null && sc.GetPosition(6, 0) == null)
            { MovePlateSpawn(6, 0, false, true); }
        }
    }

    public void MovePlateSpawn(int matrixX, int matrixY, bool isAttack = false, bool isCastle = false, bool isenPassent = false)
    {
        //Get the board value in order to convert to xy coords
        float x = matrixX;
        float y = matrixY;

        //Adjust by variable offset
        x *= 1.16f;
        y *= 1.16f;

        //Add constants (pos 0,0)
        x += -4.1f;
        y += -4.05f;

        //Set actual unity values
        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        if (isAttack) { mpScript.attack = true; }
        if (isCastle) { mpScript.castle = true; }
        if (isenPassent) { mpScript.enPassent = true; }
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }
}