using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBehaviour : MonoBehaviour
{
    // References to various bits of the UI
    public GridBehaviour theGame;
    public Text redWins;
    public Text blueWins;
    public Text turnTracker;
    public GameObject winnerBacker;
    public GameObject startMenuBackground;

    // Controls whether the main menu is visible or not
    bool GameStarted = false;

    // Update is called once per frame
    void Update()
    {
        // If game has not started, only wait for space to start
        if (!GameStarted)
        {

            // When they do, hide the main menu and let the other inputs in
            if(Input.GetKeyDown(KeyCode.Space))
            {
                theGame.SetUpBoard(Game.Hexapawn);
                theGame.GameOver = false;
                startMenuBackground.SetActive(false);
                GameStarted = true;
            }
            if(Input.GetKeyDown(KeyCode.Q))
            {
                theGame.SetUpBoard(Game.Octopawn);
                startMenuBackground.SetActive(false);
                GameStarted = true;
            }
        }

        // If the game has started, listen for other inputs
        if (GameStarted)
        {
            // If the game isn't over...
            if (!theGame.GameOver)
            {
                // If it's the red player's turn, update the UI and do the same for blue
                if (theGame.playerTurn)
                {
                    turnTracker.text = "Red's turn";
                    turnTracker.color = Color.red;
                }
                else
                {
                    turnTracker.text = "Blue's turn";
                    turnTracker.color = Color.blue;
                }
            }
            // If the game is over, show the winning banner and update turn tracker
            if (theGame.GameOver)
            {
                winnerBacker.SetActive(true);
                turnTracker.text = "Press space to restart";
                turnTracker.color = Color.black;

                // Show the right winning message based on whose turn it was last
                if (theGame.playerTurn)
                {
                    blueWins.gameObject.SetActive(true);
                }
                else
                {
                    redWins.gameObject.SetActive(true);
                }
            }

            // When space is pressed, reset the UI
            if (Input.GetKeyDown(KeyCode.Space))
            {
                blueWins.gameObject.SetActive(false);
                redWins.gameObject.SetActive(false);
                winnerBacker.SetActive(false);
            }
        }
    }
}
