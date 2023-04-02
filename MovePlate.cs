//This script controls the movement and taking of pieces
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MovePlate : MonoBehaviour
{
    //Some functions will need reference to the controller
    public GameObject controller;

    public Game game;

    //The Chesspiece that was tapped to create this MovePlate
    GameObject reference = null;

    //Location on the board
    int matrixX;
    int matrixY;

    //false: movement, true: attacking
    public bool attack = false;
    public bool castle = false;
    public bool enPassent = false;

    public void Start()
    {
        Game.didAttack = false;
        Game.canAttack = false;

        if (attack)
        {
            //Set to red
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);

            Game.canAttack = true;
        }
        if (castle)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0.0f, 1.0f, 1.0f, 1.0f);
        }
    }

    public void OnMouseUp()
    {
        //Piece movement
        controller = GameObject.FindGameObjectWithTag("GameController");

        //Destroy the victim Chesspiece
        if (attack)
        {
            if (controller.GetComponent<Game>().GetCount() % 8 >= 4)
            {
                if (reference.GetComponent<Chessman>().GetXBoard() - matrixX == 2 || reference.GetComponent<Chessman>().GetXBoard() - matrixX == -2)
                {
                    int xx = (reference.GetComponent<Chessman>().GetXBoard() + matrixX) / 2;
                    int yy = (reference.GetComponent<Chessman>().GetYBoard() + matrixY) / 2;

                    GameObject cp = controller.GetComponent<Game>().GetPosition(xx, yy);

                    if (cp.name == "white_king") controller.GetComponent<Game>().Winner("black");
                    if (cp.name == "black_king") controller.GetComponent<Game>().Winner("white");

                    Destroy(cp);

                    Game.didAttack = true;
                }
            }
            else
            {
                if (enPassent)
                {
                    Destroy(controller.GetComponent<Game>().GetPosition(matrixX, matrixY - 1));
                }
                else
                {
                    GameObject cp = controller.GetComponent<Game>().GetPosition(matrixX, matrixY);

                    if (cp.name == "white_king") controller.GetComponent<Game>().Winner("black");
                    if (cp.name == "black_king") controller.GetComponent<Game>().Winner("white");

                    Destroy(cp);
                }
            }
        }

        if (castle)
        {
            if (reference.GetComponent<Chessman>().GetXBoard() - matrixX == -2)
            {
                GameObject piece = controller.GetComponent<Game>().GetPosition(7, 0);

                controller.GetComponent<Game>().GetPosition(7, 0).GetComponent<Chessman>().SetXBoard(5);
                controller.GetComponent<Game>().GetPosition(7, 0).GetComponent<Chessman>().SetYBoard(0);
                controller.GetComponent<Game>().GetPosition(7, 0).GetComponent<Chessman>().SetCoords();

                controller.GetComponent<Game>().SetPosition(controller.GetComponent<Game>().GetPosition(7, 0));

                controller.GetComponent<Game>().SetPositionEmpty(controller.GetComponent<Game>().GetPosition(7, 0).GetComponent<Chessman>().GetXBoard(), controller.GetComponent<Game>().GetPosition(7, 0).GetComponent<Chessman>().GetYBoard());
            }
            else if (reference.GetComponent<Chessman>().GetXBoard() - matrixX == 2)
            {
                GameObject piece = controller.GetComponent<Game>().GetPosition(0, 0);

                controller.GetComponent<Game>().GetPosition(0, 0).GetComponent<Chessman>().SetXBoard(3);
                controller.GetComponent<Game>().GetPosition(0, 0).GetComponent<Chessman>().SetYBoard(0);
                controller.GetComponent<Game>().GetPosition(0, 0).GetComponent<Chessman>().SetCoords();

                controller.GetComponent<Game>().SetPosition(controller.GetComponent<Game>().GetPosition(0, 0));

                controller.GetComponent<Game>().SetPositionEmpty(controller.GetComponent<Game>().GetPosition(0, 0).GetComponent<Chessman>().GetXBoard(), controller.GetComponent<Game>().GetPosition(0, 0).GetComponent<Chessman>().GetYBoard());
            }
        }

        //Set the Chesspiece's original location to be empty
        controller.GetComponent<Game>().SetPositionEmpty(reference.GetComponent<Chessman>().GetXBoard(), reference.GetComponent<Chessman>().GetYBoard());

        //Move reference chess piece to this position
        reference.GetComponent<Chessman>().SetXBoard(matrixX);
        reference.GetComponent<Chessman>().SetYBoard(matrixY);
        reference.GetComponent<Chessman>().SetCoords();

        //Update the matrix
        controller.GetComponent<Game>().SetPosition(reference);

        //Destroy the move plates including self
        reference.GetComponent<Chessman>().DestroyMovePlates();

        //Switch Current Player
        controller.GetComponent<Game>().NextTurn();
    }

    public void SetCoords(int x, int y)
    {
        matrixX = x;
        matrixY = y;
    }

    public void SetReference(GameObject obj)
    {
        reference = obj;
    }

    public GameObject GetReference()
    {
        return reference;
    }
}
