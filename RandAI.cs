//This script is an AI that acts randomly, which we used temporarily, before the actual AI was finished
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using System.Threading;

public class RandAI : MonoBehaviour
{
    public List<List<object>> BlackPieces()
    {
        GameObject[,] reference = GetComponent<Game>().GetPositions();
        var blackPieces = new List<List<object>>();

        for (int x = 0; x < reference.GetLength(0); x++)
        {
            for (int y = 0; y < reference.GetLength(1); y++)
            {
                if (reference[x, y] != null)
                {
                    if (reference[x, y].name == "black_pawn" || reference[x, y].name == "black_rook" || reference[x, y].name == "black_knight" ||
                        reference[x, y].name == "black_bishop" || reference[x, y].name == "black_queen" || reference[x, y].name == "black_king")
                    {
                        var piece = new List<object>();
                        piece.Add(reference[x, y].name); piece.Add(x); piece.Add(y);
                        blackPieces.Add(piece);
                    }
                    if (blackPieces.Count == 16) break;
                }
            }
        }
        return blackPieces;
    }

    public List<int> MoveAdd(int x1, int y1, int x2, int y2)
    {
        var move = new List<int>();
        move.Add(x1); move.Add(y1); move.Add(x2); move.Add(y2);
        return (move);
    }

    public List<List<int>> LineAdd(int a, int b, int x, int y)
    {
        GameObject controller = GameObject.FindGameObjectWithTag("GameController");
        var moves = new List<List<int>>();
        for (int i = 1; i < 8; i++)
        {
            if (controller.GetComponent<Game>().PositionOnBoard(x + i * a, y + i * b))
            {
                if (controller.GetComponent<Game>().GetPosition(x + i * a, y + i * b) != null)
                {
                    if (controller.GetComponent<Game>().GetPosition(x + i * a, y + i * b).name.Split('_')[0] == "white")
                    {
                        moves.Add(MoveAdd(x, y, x + i * a, y + i * b));
                        break;
                    }
                    else { break; }
                }
                else { moves.Add(MoveAdd(x, y, x + i * a, y + i * b)); }
            }
            else { break; }
        }
        return (moves);
    }

    public List<int> KnightAdd(int a, int b, int x, int y)
    {
        GameObject controller = GameObject.FindGameObjectWithTag("GameController");

        if (controller.GetComponent<Game>().PositionOnBoard(x + a, y + b))
        {
            if (controller.GetComponent<Game>().GetPosition(x + a, y + b) == null)
            {
                return (MoveAdd(x, y, x + a, y + b));
            }
            else if (controller.GetComponent<Game>().GetPosition(x + a, y + b).name.Split('_')[0] == "white")
            {
                return (MoveAdd(x, y, x + a, y + b));
            }
        }
        return (null);
    }

    public List<int> KingAdd(int a, int b, int x, int y)
    {
        GameObject controller = GameObject.FindGameObjectWithTag("GameController");

        if (controller.GetComponent<Game>().PositionOnBoard(x + a, y + b))
        {
            if (controller.GetComponent<Game>().GetPosition(x + a, y + b) == null)
            {
                return (MoveAdd(x, y, x + a, y + b));
            }
            else if (controller.GetComponent<Game>().GetPosition(x + a, y + b).name.Split('_')[0] == "white")
            {
                return (MoveAdd(x, y, x + a, y + b));
            }
        }
        return (null);
    }

    public List<List<int>> BlackMoves()
    {
        GameObject[,] reference = GetComponent<Game>().GetPositions();
        var moves = new List<List<int>>();
        var move = new List<int>();
        List<List<object>> bp = BlackPieces();

        foreach (List<object> piece in bp)
        {
            GameObject controller = GameObject.FindGameObjectWithTag("GameController");
            var name = piece[0] as string;
            int x = Convert.ToInt32(piece[1]); int y = Convert.ToInt32(piece[2]);

            if (controller.GetComponent<Game>().GetCount() % 8 < 4)
            {
                if (name == "black_pawn")
                {
                    if (y == 6 && controller.GetComponent<Game>().PositionOnBoard(x, y - 2) && reference[x, y - 1] == null && reference[x, y - 2] == null)
                    {
                        moves.Add(MoveAdd(x, y, x, y - 2));
                    }
                    if (controller.GetComponent<Game>().PositionOnBoard(x, y - 1) && reference[x, y - 1] == null)
                    {
                        moves.Add(MoveAdd(x, y, x, y - 1));
                    }
                    if (controller.GetComponent<Game>().PositionOnBoard(x + 1, y - 1) && reference[x + 1, y - 1] != null && reference[x + 1, y - 1].name.Split('_')[0] == "white")
                    {
                        moves.Add(MoveAdd(x, y, x + 1, y - 1));
                    }
                    if (controller.GetComponent<Game>().PositionOnBoard(x - 1, y - 1) && reference[x - 1, y - 1] != null && reference[x - 1, y - 1].name.Split('_')[0] == "white")
                    {
                        moves.Add(MoveAdd(x, y, x - 1, y - 1));
                    }
                }
                if (name == "black_rook" || name == "black_queen")
                {
                    foreach (List<int> mov in LineAdd(1, 0, x, y)) { moves.Add(mov); }
                    foreach (List<int> mov in LineAdd(0, 1, x, y)) { moves.Add(mov); }
                    foreach (List<int> mov in LineAdd(-1, 0, x, y)) { moves.Add(mov); }
                    foreach (List<int> mov in LineAdd(0, -1, x, y)) { moves.Add(mov); }
                }
                if (name == "black_bishop" || name == "black_queen")
                {
                    foreach (List<int> mov in LineAdd(1, 1, x, y)) { moves.Add(mov); }
                    foreach (List<int> mov in LineAdd(-1, 1, x, y)) { moves.Add(mov); }
                    foreach (List<int> mov in LineAdd(1, -1, x, y)) { moves.Add(mov); }
                    foreach (List<int> mov in LineAdd(-1, -1, x, y)) { moves.Add(mov); }
                }
                if (name == "black_knight")
                {
                    if (KnightAdd(1, 2, x, y) != null) { moves.Add(KnightAdd(1, 2, x, y)); }
                    if (KnightAdd(2, 1, x, y) != null) { moves.Add(KnightAdd(2, 1, x, y)); }
                    if (KnightAdd(-1, 2, x, y) != null) { moves.Add(KnightAdd(-1, 2, x, y)); }
                    if (KnightAdd(1, -2, x, y) != null) { moves.Add(KnightAdd(1, -2, x, y)); }
                    if (KnightAdd(2, -1, x, y) != null) { moves.Add(KnightAdd(2, -1, x, y)); }
                    if (KnightAdd(-2, 1, x, y) != null) { moves.Add(KnightAdd(-2, 1, x, y)); }
                    if (KnightAdd(-1, -2, x, y) != null) { moves.Add(KnightAdd(-1, -2, x, y)); }
                    if (KnightAdd(-2, -1, x, y) != null) { moves.Add(KnightAdd(-2, -1, x, y)); }
                }
                if (name == "black_king")
                {
                    if (KingAdd(1, 1, x, y) != null) { moves.Add(KingAdd(1, 1, x, y)); }
                    if (KingAdd(1, 0, x, y) != null) { moves.Add(KingAdd(1, 0, x, y)); }
                    if (KingAdd(1, -1, x, y) != null) { moves.Add(KingAdd(1, -1, x, y)); }
                    if (KingAdd(0, 1, x, y) != null) { moves.Add(KingAdd(0, 1, x, y)); }
                    if (KingAdd(0, -1, x, y) != null) { moves.Add(KingAdd(0, -1, x, y)); }
                    if (KingAdd(-1, 1, x, y) != null) { moves.Add(KingAdd(-1, 1, x, y)); }
                    if (KingAdd(-1, 0, x, y) != null) { moves.Add(KingAdd(-1, 0, x, y)); }
                    if (KingAdd(-1, -1, x, y) != null) { moves.Add(KingAdd(-1, -1, x, y)); }
                }
            }
            else
            {
                Game sc = controller.GetComponent<Game>();

                if (sc.PositionOnBoard(x + 1, y - 1) && sc.GetPosition(x + 1, y - 1) == null) { moves.Add(MoveAdd(x, y, x + 1, y - 1)); }
                else if (sc.PositionOnBoard(x + 2, y - 2) && sc.GetPosition(x + 1, y - 1) != null && sc.GetPosition(x + 2, y - 2) == null && sc.GetPosition(x + 1, y - 1).name.Split('_')[0] == "white")
                {
                    moves.Add(MoveAdd(x, y, x + 2, y - 2));
                }

                if (sc.PositionOnBoard(x - 1, y - 1) && sc.GetPosition(x - 1, y - 1) == null) { moves.Add(MoveAdd(x, y, x - 1, y - 1)); }
                else if (sc.PositionOnBoard(x - 2, y - 2) && sc.GetPosition(x - 1, y - 1) != null && sc.GetPosition(x - 2, y - 2) == null && sc.GetPosition(x - 1, y - 1).name.Split('_')[0] == "white")
                {
                    moves.Add(MoveAdd(x, y, x - 2, y - 2));
                }

                if (name != "black_pawn")
                {
                    if (sc.PositionOnBoard(x + 1, y + 1) && sc.GetPosition(x + 1, y + 1) == null) { moves.Add(MoveAdd(x, y, x + 1, y + 1)); }
                    else if (sc.PositionOnBoard(x + 2, y + 2) && sc.GetPosition(x + 1, y + 1) != null && sc.GetPosition(x + 2, y + 2) == null && sc.GetPosition(x + 1, y + 1).name.Split('_')[0] == "white")
                    {
                        moves.Add(MoveAdd(x, y, x + 2, y + 2));
                    }

                    if (sc.PositionOnBoard(x - 1, y + 1) && sc.GetPosition(x - 1, y - 1) == null)
                    { moves.Add(MoveAdd(x, y, x - 1, y + 1)); }
                    else if (sc.PositionOnBoard(x - 2, y + 2) && sc.GetPosition(x - 1, y + 1) != null && sc.GetPosition(x - 2, y + 2) == null && sc.GetPosition(x - 1, y + 1).name.Split('_')[0] == "white")
                    {
                        moves.Add(MoveAdd(x, y, x - 2, y + 2));
                    }
                }
            }
        }
        return (moves);
    }

    public void Update()
    {
        GameObject controller = GameObject.FindGameObjectWithTag("GameController");
        GameObject[,] reference = GetComponent<Game>().GetPositions();
        string player = controller.GetComponent<Game>().GetCurrentPlayer();

        if (player == "black")
        {
            Thread.Sleep(200);

            List <List<int>> moves = BlackMoves();
            System.Random rnd = new System.Random();
            int r = rnd.Next(moves.Count);
            GameObject piece = reference[moves[r][0], moves[r][1]];

            controller = GameObject.FindGameObjectWithTag("GameController");

            //Destroy the victim Chesspiece

            // Doesn't work yet

            if (controller.GetComponent<Game>().GetCount() % 8 >= 4)
            {
                if (-2 < piece.GetComponent<Chessman>().GetXBoard() - moves[r][2] && 2 > piece.GetComponent<Chessman>().GetXBoard() - moves[r][2])
                {
                    int xx = (piece.GetComponent<Chessman>().GetXBoard() + moves[r][2]) / 2;
                    int yy = (piece.GetComponent<Chessman>().GetYBoard() + moves[r][3]) / 2;

                    if (controller.GetComponent<Game>().GetPosition(xx, yy) != null)
                    {

                        GameObject cp = controller.GetComponent<Game>().GetPosition(xx, yy);

                        if (cp.name == "white_king") controller.GetComponent<Game>().Winner("black");

                        Destroy(cp);
                    }
                }
            }
            else if (controller.GetComponent<Game>().GetPosition(moves[r][2], moves[r][3]) != null)
            {
                GameObject cp = controller.GetComponent<Game>().GetPosition(moves[r][2], moves[r][3]);

                if (cp.name == "white_king") controller.GetComponent<Game>().Winner("black");

                Destroy(cp);
            }

            //Set the Chesspiece's original location to be empty
            controller.GetComponent<Game>().SetPositionEmpty(piece.GetComponent<Chessman>().GetXBoard(), piece.GetComponent<Chessman>().GetYBoard());

            //Move reference chess piece to this position
            piece.GetComponent<Chessman>().SetXBoard(moves[r][2]);
            piece.GetComponent<Chessman>().SetYBoard(moves[r][3]);
            piece.GetComponent<Chessman>().SetCoords();

            //Update the matrix
            controller.GetComponent<Game>().SetPosition(piece);

            //Destroy the move plates including self
            piece.GetComponent<Chessman>().DestroyMovePlates();

            //Switch Current Player
            controller.GetComponent<Game>().NextTurn();
        }
    }
}
