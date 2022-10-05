using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBehaviour : MonoBehaviour
{
    public GridBehaviour theGame;
    public Text redWins;
    public Text blueWins;
    public Text turnTracker;
    public GameObject winnerBacker;
    public GameObject startMenuBackground;

    bool GameStarted = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameStarted)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                startMenuBackground.SetActive(false);
                GameStarted = true;
            }
        }
        if (GameStarted)
        {
            if (!theGame.GameOver)
            {
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
            if (theGame.GameOver)
            {
                winnerBacker.SetActive(true);
                turnTracker.text = "Press space to restart";
                turnTracker.color = Color.black;
                if (theGame.playerTurn)
                {
                    blueWins.gameObject.SetActive(true);
                }
                else
                {
                    redWins.gameObject.SetActive(true);
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                blueWins.gameObject.SetActive(false);
                redWins.gameObject.SetActive(false);
                winnerBacker.SetActive(false);
            }
        }
    }
}
