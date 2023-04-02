//This script determines the AI which controls the opponent
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using UnityEngine;

public class D1AI : MonoBehaviour
{

    public List<List<object>> WhitePieces()
    {
        GameObject[,] reference = GetComponent<Game>().GetPositions();
        var whitePieces = new List<List<object>>();

        for (int x = 0; x < reference.GetLength(0); x++)
        {
            for (int y = 0; y < reference.GetLength(1); y++)
            {
                if (reference[x, y] != null)
                {
                    if (reference[x, y].name == "white_pawn" || reference[x, y].name == "white_rook" || reference[x, y].name == "white_knight" ||
                        reference[x, y].name == "white_bishop" || reference[x, y].name == "white_queen" || reference[x, y].name == "white_king")
                    {
                        var piece = new List<object>();
                        piece.Add(reference[x, y].name); piece.Add(x); piece.Add(y);
                        whitePieces.Add(piece);
                    }
                    if (whitePieces.Count == 16) break;
                }
            }
        }
        return whitePieces;
    }
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
    public List<List<int>> WhiteMoves()
    {
        GameObject[,] reference = GetComponent<Game>().GetPositions();
        var moves = new List<List<int>>();
        var move = new List<int>();
        List<List<object>> wp = WhitePieces();

        foreach (List<object> piece in wp)
        {
            GameObject controller = GameObject.FindGameObjectWithTag("GameController");
            var name = piece[0] as string;
            int x = Convert.ToInt32(piece[1]); int y = Convert.ToInt32(piece[2]);

            if (controller.GetComponent<Game>().GetCount() % 8 < 4)
            {
                if (name == "white_pawn")
                {
                    if (y == 6 && controller.GetComponent<Game>().PositionOnBoard(x, y + 2) && reference[x, y + 1] == null && reference[x, y + 2] == null)
                    {
                        moves.Add(MoveAdd(x, y, x, y + 2));
                    }
                    if (controller.GetComponent<Game>().PositionOnBoard(x, y + 1) && reference[x, y + 1] == null)
                    {
                        moves.Add(MoveAdd(x, y, x, y + 1));
                    }
                    if (controller.GetComponent<Game>().PositionOnBoard(x + 1, y + 1) && reference[x + 1, y + 1] != null && reference[x + 1, y + 1].name.Split('_')[0] == "white")
                    {
                        moves.Add(MoveAdd(x, y, x + 1, y + 1));
                    }
                    if (controller.GetComponent<Game>().PositionOnBoard(x - 1, y + 1) && reference[x - 1, y + 1] != null && reference[x - 1, y + 1].name.Split('_')[0] == "white")
                    {
                        moves.Add(MoveAdd(x, y, x - 1, y + 1));
                    }
                }
                if (name == "white_rook" || name == "white_queen")
                {
                    foreach (List<int> mov in LineAdd(1, 0, x, y)) { moves.Add(mov); }
                    foreach (List<int> mov in LineAdd(0, 1, x, y)) { moves.Add(mov); }
                    foreach (List<int> mov in LineAdd(-1, 0, x, y)) { moves.Add(mov); }
                    foreach (List<int> mov in LineAdd(0, -1, x, y)) { moves.Add(mov); }
                }
                if (name == "white_bishop" || name == "white_queen")
                {
                    foreach (List<int> mov in LineAdd(1, 1, x, y)) { moves.Add(mov); }
                    foreach (List<int> mov in LineAdd(-1, 1, x, y)) { moves.Add(mov); }
                    foreach (List<int> mov in LineAdd(1, -1, x, y)) { moves.Add(mov); }
                    foreach (List<int> mov in LineAdd(-1, -1, x, y)) { moves.Add(mov); }
                }
                if (name == "white_knight")
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
                if (name == "white_king")
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

                if (sc.PositionOnBoard(x + 1, y + 1) && sc.GetPosition(x + 1, y + 1) == null) { moves.Add(MoveAdd(x, y, x + 1, y + 1)); }
                else if (sc.PositionOnBoard(x + 2, y + 2) && sc.GetPosition(x + 1, y + 1) != null && sc.GetPosition(x + 2, y + 2) == null && sc.GetPosition(x + 1, y + 1).name.Split('_')[0] == "white")
                {
                    moves.Add(MoveAdd(x, y, x + 2, y + 2));
                }

                if (sc.PositionOnBoard(x - 1, y + 1) && sc.GetPosition(x - 1, y + 1) == null)
                { moves.Add(MoveAdd(x, y, x - 1, y + 1)); }
                else if (sc.PositionOnBoard(x - 2, y + 2) && sc.GetPosition(x - 1, y + 1) != null && sc.GetPosition(x - 2, y + 2) == null && sc.GetPosition(x - 1, y + 1).name.Split('_')[0] == "white")
                {
                    moves.Add(MoveAdd(x, y, x - 2, y + 2));
                }

                if (name != "white_pawn")
                {
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
            List<List<int>> moves = BlackMoves();

            int r = 0;
            int a = 0;
            int b = 0;
            int c = 0;
            int d = 0;
            int count1 = 0;
            bool check = false;
            bool Qcheck = false;

            // Check for checks
            foreach (List<object> p in BlackPieces())
            {
                var name = p[0] as string;
                var x = Convert.ToInt32(p[1]);
                var y = Convert.ToInt32(p[2]);
                // Check if checked
                if (name == "black_king")
                {
                    foreach (List<int> move in WhiteMoves())
                    {
                        if (move[2] == x && move[3] == y)
                        {
                            check = true;
                        }
                    }
                }
            } // Check for advanced pawns too

            foreach(List<int> move in moves)
            {
                if (controller.GetComponent<Game>().GetCount() % 8 >= 4)
                {
                    if (reference[move[2], move[3]] != null && reference[move[2], move[3]].name == "white_king") { r = count1; break; }
                }
                else
                {
                    if (((move[2] - move[0]) == 2 || (move[2] - move[0]) == -2) && reference[(move[2] + move[0]) / 2, (move[3] + move[1]) / 2] != null)
                    {
                        if (reference[move[2], move[3]] != null && reference[(move[2] + move[0]) / 2, (move[3] + move[1]) / 2].name == "white_king") { r = count1; break; }
                    }
                }
                count1 += 1;
            }

            count1 = 0;

            if (!check)
            {
                foreach (List<int> move in moves)
                {
                    if (controller.GetComponent<Game>().GetCount() % 8 >= 4)
                    {
                        if (reference[move[2], move[3]] != null && reference[move[2], move[3]].name == "white_queen" && reference[move[0], move[1]].name != "black_king") { a = count1; }
                        else if (reference[move[2], move[3]] != null && reference[move[2], move[3]].name == "white_rook" && (reference[move[0], move[1]].name == "black_pawn" || reference[move[0], move[1]].name == "black_bishop" || reference[move[0], move[1]].name == "black_knight"))
                        {
                            foreach (List<object> p in BlackPieces())
                            {
                                var name = p[0] as string;
                                var x = Convert.ToInt32(p[1]);
                                var y = Convert.ToInt32(p[2]);
                                if (name == "black_queen")
                                {
                                    foreach (List<int> mo in WhiteMoves())
                                    {
                                        if (mo[2] == x && mo[3] == y)
                                        {
                                            if (!(mo[1] == move[3] && mo[0] == move[2]))
                                            {
                                                Qcheck = true;
                                            }
                                        }
                                    }
                                }
                            }
                            if (!Qcheck) { b = count1; }
                        }
                        else if (reference[move[2], move[3]] != null && reference[move[2], move[3]].name.Split('_')[0] == "white" && reference[move[0], move[1]].name != "black_king" && reference[move[0], move[1]].name != "black_queen" && reference[move[0], move[1]].name != "black_rook")
                        {
                            if (reference[move[2], move[3]].name == "white_bishop") { c = count1; }
                            if (reference[move[2], move[3]].name == "white_knight") { d = count1; }
                        }
                    }
                    else
                    {
                        int xATK = (move[2] + move[0]) / 2;
                        int yATK = (move[3] + move[1]) / 2;
                        if (move[2] - move[0] == 2 || move[2] - move[0] == -2)
                        {
                            if (reference[xATK, yATK] != null && reference[xATK, yATK].name == "white_queen" && reference[move[0], move[1]].name != "black_king") { a = count1; }
                            else if (reference[xATK, yATK] != null && reference[xATK, yATK].name == "white_rook" && (reference[move[0], move[1]].name == "black_pawn" || reference[move[0], move[1]].name == "black_bishop" || reference[move[0], move[1]].name == "black_knight"))
                            {
                                foreach (List<object> p in BlackPieces())
                                {
                                    var name = p[0] as string;
                                    var x = Convert.ToInt32(p[1]);
                                    var y = Convert.ToInt32(p[2]);
                                    if (name == "black_queen")
                                    {
                                        foreach (List<int> mo in WhiteMoves())
                                        {
                                            if (mo[2] == x && mo[3] == y)
                                            {
                                                Qcheck = true;
                                            }
                                        }
                                    }
                                }
                                if (!Qcheck) { b = count1; }
                            }
                            else if (reference[xATK, yATK] != null && reference[xATK, yATK].name.Split('_')[0] == "white")
                            {
                                if (reference[move[2], move[3]].name == "white_bishop") { c = count1; }
                                if (reference[move[2], move[3]].name == "white_knight") { d = count1; }
                            }
                        }
                        count1 += 1;
                    }
                }
            }
            if (r == 0)
            {
                if (a > 0) { r = a; }
                else if (b > 0) { r = b; }
                else if (c > 0) { r = c; }
                else if (d > 0) { r = d; }
            }
            if (r == 0)
            {
                System.Random rnd = new System.Random();
                r = rnd.Next(moves.Count);
            }

            GameObject piece = reference[moves[r][0], moves[r][1]];

            controller = GameObject.FindGameObjectWithTag("GameController");

            //Destroy the victim Chesspiece

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

            //Switch Current Player
            controller.GetComponent<Game>().NextTurn();
        }
    }
}
