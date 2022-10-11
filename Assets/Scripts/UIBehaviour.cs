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
    public Text gamemode;
    public Text helpPrompt;
    public GameObject winnerBacker;
    public GameObject startMenuBackground;

    // Controls whether the main menu is visible or not
    bool GameStarted = false;

    // Controls whether the help menu is open
    bool HelpMenu = false;
    // Update is called once per frame

    private void Awake()
    {
        theGame.SetUpBoard();
    }

    void Update()
    {
        // If game has not started, only wait for space to start
        if (!GameStarted)
        {

            // When they do, hide the main menu and let the other inputs in
            if(Input.GetKeyDown(KeyCode.Space))
            {
                theGame.currentGame = Game.Hexapawn;
                theGame.ResetBoard();
                theGame.GameOver = false;
                startMenuBackground.SetActive(false);
                GameStarted = true;
            }
            if(Input.GetKeyDown(KeyCode.Q))
            {
                theGame.currentGame = Game.Octopawn;
                theGame.ResetBoard();
                theGame.GameOver = false;
                startMenuBackground.SetActive(false);
                GameStarted = true;
            }
        }

        // If the game has started, listen for other inputs
        if (GameStarted)
        {
            gamemode.text = theGame.currentGame.ToString();

            // Help menu has priority, if its open, all other inputs are ignored until its resovled.

            if (!redWins.IsActive() && !blueWins.IsActive())
            {
                if (Input.GetKeyDown(KeyCode.H))
                {
                    ToggleHelpMenu();
                }
                else if (HelpMenu && Input.GetKeyDown(KeyCode.Space))
                {
                    theGame.currentGame = Game.Hexapawn;
                    theGame.ResetBoard();
                    ToggleHelpMenu();
                }
                else if (HelpMenu && Input.GetKeyDown(KeyCode.Q))
                {
                    theGame.currentGame = Game.Octopawn;
                    theGame.ResetBoard();
                    ToggleHelpMenu();
                }
            }

            if (!HelpMenu)
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

    void ToggleHelpMenu()
    {
        HelpMenu = !HelpMenu;
        winnerBacker.SetActive(HelpMenu);
        helpPrompt.gameObject.SetActive(HelpMenu);
        
    }
}
