using UnityEngine;
using System.Collections.Generic;

public enum Game
{
    Hexapawn,
    Octopawn
}

public class GridBehaviour : MonoBehaviour
{
    // A reference to the grid prefab, dimensions of the grid and a list of all cells
    public GridCell gridCellPrefab;
    int gridY = 3;
    int gridX = 3;
    List<GridCell> GridList = new List<GridCell>();

    // Variables for the 2 special grid cells which will store dead counters
    GridCell RedGraveyard;
    GridCell BlueGraveyard;

    // A reference to the counter prefab, a list of all alive and dead counters and a reference to the current counter
    public Counter counterPrefab;
    List<Counter> CounterListAll = new List<Counter>();
    List<Counter> CounterListDead = new List<Counter>();
    Counter currentCounter = null;
    List<GridCell> currentValidMoves = null;

    // Additional references needed for Octopawn
    public Transform standPos;
    public Transform cameraPos;
    public Transform BlueGraveyardStand;
    public Transform RedGraveyardStand;

    // Set up for mouse control
    Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);

    // Tracks whether the game has ended
    public bool GameOver = true;
    // true for red, false for blue
    public bool playerTurn = true;

    public Game currentGame;

    void Start()
    {
        // SetUpBoard spawns grid cells, counters and graveyards
        //SetUpBoard();
    }


    // Update is called once per frame
    void Update()
    {
        
        // Space will reset the board
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ResetBoard();
        }


        if (!GameOver)
        {
            CheckWinner();
            // M1 will be used to either select a counter, unselect a counter or choose a grid slot to move to
            if (Input.GetMouseButtonDown(0))
            {
                if(HandleTouch())
                {                    
                    TogglePlayerTurn();
                }
                
            }

            // M2 is currently used to kill counters
            if (Input.GetMouseButtonDown(1))
            {
                HandleAltTouch();
            }

            
            if (Input.GetKeyDown(KeyCode.Q))
            {
                FindValidMoves(CounterListAll[0]);
            }
        }
        
    }

    bool HandleTouch()
    {
        RaycastHit hit;

        if (Physics.Raycast(TouchRay, out hit))
        {
            // If there is no current counter, the only thing the user should be able to click on is a counter
            if(currentCounter == null)
            {
                foreach (Counter c in CounterListAll)
                {
                    if (hit.transform.name == c.transform.name)
                    {
                        if(playerTurn && c.tag == "Red")
                        {
                            // Change the state of the counter to selected to spawn a ring, then set the current counter to this counter
                            SetCurrentCounter(c);
                            return false;
                        }
                        else if (!playerTurn && c.tag == "Blue")
                        {
                            SetCurrentCounter(c);
                            return false;
                        }
                        else
                        {
                            Debug.Log("Player tried to pick wrong counter");
                            return false;
                        }
                        
                        
                    }
                }
            }
            // If there is a counter however, they can either click the grid to move there or click a counter again to deselect
            else
            {
                if(hit.transform.tag == "Red" || hit.transform.tag == "Blue" )
                {
                    // Remove the ring and set the current counter to null
                    UnsetCurrentCounter(currentCounter);
                    return false;
                }
                else
                {
                    // If counter is not null, and the user clicked a cell, they want to move there
                    foreach (GridCell g in GridList)
                    {
                        if (hit.transform.name == g.transform.name)
                        {
                            // Move the counter, then remove the ring and set the current counter to null
                            MoveCounter(g, currentCounter);
                            UnsetCurrentCounter(currentCounter);
                            return true;
                        }
                    }
                }
                
            }
        }
        return false;
    }

    // If the user right clicks a counter, remove it
    void HandleAltTouch()
    {
        RaycastHit hit;

        if (Physics.Raycast(TouchRay, out hit))
        {

            foreach (Counter c in CounterListAll)
            {
                if (hit.transform.name == c.transform.name)
                {
                    KillCounter(c);
                    break;
                }
            }
        }
    }

    // Grab the point where counters move to and set the counters transform to that
    void MoveCounter(GridCell cell, Counter counter)
    {
        List<GridCell> validMoves = FindValidMoves(counter);
        bool validMove = false;

        foreach(GridCell g in validMoves)
        {
            if(cell == g)
            {
                counter.transform.position = cell.GetMidPoint();
                if(cell.GetCounter() != false)
                {
                    KillCounter(cell.GetCounter());
                }
                validMove = true;
                break;
            }
        }
        if(!validMove)
            Debug.Log("Invalid move");
    }

    // Move the counter to it's respective graveyard then add to dead/remove from alive lists
    void KillCounter(Counter counter)
    {
        if (counter.tag == "Red")
        {
            counter.transform.position = RedGraveyard.transform.position;
        }
        else if (counter.tag == "Blue")
        {
            counter.transform.position = BlueGraveyard.transform.position;
        }

        CounterListDead.Add(counter);
        CounterListAll.Remove(counter);

    }

    // Set up board sorts grids, counters and graveyards
    public void SetUpBoard(Game gamemode)
    {
        Debug.Log("Setting up a game of " + gamemode);
        RedGraveyard = Instantiate(gridCellPrefab);
        RedGraveyard.transform.eulerAngles = new Vector3(90, 0, 0);
        RedGraveyard.transform.name = "RedGraveyard";
        RedGraveyard.transform.SetParent(transform);

        BlueGraveyard = Instantiate(gridCellPrefab);
        BlueGraveyard.transform.eulerAngles = new Vector3(90, 0, 0);
        BlueGraveyard.transform.name = "BlueGraveyard";
        BlueGraveyard.transform.SetParent(transform);

        if (gamemode == Game.Hexapawn)
        {
            gridX = 3;
            gridY = 3;

            RedGraveyard.transform.position = new Vector3(-2, 0.7f, 3);
            BlueGraveyard.transform.position = new Vector3(8, 0.7f, 3);
            
            BlueGraveyardStand.position = new Vector3(8, 0.18f, 3);
        }
        else if(gamemode == Game.Octopawn)
        {
            gridX = 4;
            gridY = 4;

            standPos.position = new Vector3(4, standPos.position.y, 4);
            standPos.localScale = new Vector3(8.5f, 1, 8.5f);
            cameraPos.position = new Vector3(4.31f, 10.98f, 2.97f);

            RedGraveyard.transform.position = new Vector3(-2, 0.7f, 4);
            BlueGraveyard.transform.position = new Vector3(10, 0.7f, 4);

            BlueGraveyardStand.position = new Vector3(10, 0.18f, 4);
            RedGraveyardStand.position = new Vector3(-2, 0.18f, 4);
        }
        // black stores a bool that is flipped back and forth to get a chequerboard pattern
        bool black = true;
        // Variable that is used to sort Octopawns chequerboard
        int OctoColourCount = 0;

        // nested for statements to create a grid
        for (int x = 1; x < gridX * 2; x += 2)
        {
            for (int z = 1; z < gridY * 2; z += 2)
            {
                // spawn a cell at this x/y, set its name and parent
                GridCell cell = Instantiate(gridCellPrefab, new Vector3(x, transform.position.y, z), Quaternion.identity);
                cell.transform.eulerAngles = new Vector3(90, 0, 0);
                cell.transform.name = "Cell" + x + z;
                cell.transform.SetParent(transform);

                // give it the correct colour then flip black
                if (black)
                {
                    cell.BecomeBlack();
                }
                else
                {
                    cell.BecomeWhite();
                }

                OctoColourCount++;
                if (gamemode == Game.Octopawn)
                {
                    if(OctoColourCount % 4 == 0)
                    {
                        black = !black;
                    }
                }
                black = !black;

                // add the new cell to the list of gridcells
                GridList.Add(cell);
            }
        }

        // create red counters
        for (int x = 0; x < gridX; x++)
        {
            // spawn a counter at this x/y, set its name, colour and parent. add it to the counter list.
            Counter thisCounter = Instantiate(counterPrefab);
            thisCounter.transform.name = "CounterRed" + x;
            thisCounter.transform.tag = "Red";
            thisCounter.SetRed();
            thisCounter.transform.SetParent(transform);
            CounterListAll.Add(thisCounter);
        }

        // create blue counters
        for (int x = 0; x < gridX; x++)
        {
            // spawn a counter at this x/y, set its name, colour and parent. add it to the counter list.
            Counter thisCounter = Instantiate(counterPrefab);
            thisCounter.transform.name = "CounterBlue" + x;
            thisCounter.transform.tag = "Blue";
            thisCounter.SetBlue();
            thisCounter.transform.SetParent(transform);
            CounterListAll.Add(thisCounter);
        }
        // spawn the graveyarde on top of the cubes, and set them up correctly.

        int redPosX = 1;
        int bluePosX = 1;

        int bluePosZ = 5;
        if(gamemode == Game.Octopawn)   
            bluePosZ = 7;


        foreach (Counter c in CounterListAll)
        {
            if(c.tag == "Red")
            {
                c.transform.position = new Vector3(redPosX, 1.5f, 1);
                redPosX += 2;
            }
            else
            {
                c.transform.position = new Vector3(bluePosX, 1.5f, bluePosZ);
                bluePosX += 2;
            }
        }





    }

    // Destroy all grids/graveyards/counters then clear all lists for them
    void ResetBoard()
     {
        // iterate through all the lists and destroy the game objects in them, then destroy both graveyards
        foreach (GridCell g in GridList)
        {
            Destroy(g.gameObject);
        }
        foreach (Counter c in CounterListAll)
        {
            Destroy(c.gameObject);
        }
        foreach (Counter c in CounterListDead)
        {
            Destroy(c.gameObject);
        }
        Destroy(RedGraveyard.gameObject);
        Destroy(BlueGraveyard.gameObject);

        // Clear all lists, then set up the board again
        GridList.Clear();
        CounterListAll.Clear();
        CounterListDead.Clear();
        GameOver = false;
        playerTurn = true;
        Debug.ClearDeveloperConsole();
        SetUpBoard(currentGame);

    }

    List<GridCell> FindValidMoves(Counter counter)
    {
        List<GridCell> ValidMoves = new List<GridCell>();
        //List<GridCell> InvalidMoves = new List<GridCell>();
        //ValidMoves.AddRange(GridList);
        if (counter.tag == "Red")
        {
            foreach (GridCell g in GridList)
            {
                if(g.transform.position.z == counter.transform.position.z + 2)
                {
                    if (g.transform.position.x == counter.transform.position.x && g.GetCounter() == null)
                    {
                        ValidMoves.Add(g);
                    }


                    if(g.GetCounter() != null && g.GetCounter().tag == "Blue")
                    {
                        if (g.transform.position.x == counter.transform.position.x + 2)
                        {
                            ValidMoves.Add(g);
                        }

                        if (g.transform.position.x == counter.transform.position.x - 2)
                        {
                            ValidMoves.Add(g);
                        }
                    }
                    
                }
                
            }
        }
        else if (counter.tag == "Blue")
        {
            foreach (GridCell g in GridList)
            {
                if (g.transform.position.z == counter.transform.position.z - 2)
                {
                    if (g.transform.position.x == counter.transform.position.x && g.GetCounter() == null)
                    {
                        ValidMoves.Add(g);
                    }


                    if (g.GetCounter() != null && g.GetCounter().tag == "Red")
                    {
                        if (g.transform.position.x == counter.transform.position.x + 2)
                        {
                            ValidMoves.Add(g);
                        }

                        if (g.transform.position.x == counter.transform.position.x - 2)
                        {
                            ValidMoves.Add(g);
                        }
                    }

                }

            }
        }
        else
        {
            Debug.Log("Error: Counter is not tagged correctly");
        }
        return ValidMoves;
    }


    //
    void DisplayValidMoves()
    {
        if(currentValidMoves != null)
        {
            foreach (GridCell g in currentValidMoves)
            {
                g.SetSelected(true);
            }
        }
        else
        {
            foreach (GridCell g in GridList)
            {
                g.SetSelected(false);
            }
        }
        
    }

    void SetCurrentCounter(Counter c)
    {
        c.SetSelected(true);
        currentCounter = c;
        currentValidMoves = FindValidMoves(c);
        DisplayValidMoves();
    }

    void UnsetCurrentCounter(Counter c)
    {
        c.SetSelected(false);
        currentCounter = null;
        currentValidMoves = null;
        DisplayValidMoves();
    }

    public bool CheckRedWinner()
    {
        List<GridCell> ValidMoves = new List<GridCell>();
        foreach (Counter c in CounterListAll)
        {
            if(c.tag == "Blue")
            {
                ValidMoves.AddRange(FindValidMoves(c));
            }
            else
            {
                int winningRow = 5;
                if (currentGame == Game.Octopawn)
                    winningRow = 7;

                if(c.transform.position.z == winningRow)
                {
                    Debug.Log("Red won because they got a counter to the back row");
                    return true;
                }
            }
        }
        if(ValidMoves.Count == 0)
        {
            Debug.Log("Red won because Blue has no moves");
            return true;
        }
        else
        {
            return false;
        }
        
    }

    public bool CheckBlueWinner()
    {
        List<GridCell> ValidMoves = new List<GridCell>();
        foreach (Counter c in CounterListAll)
        {
            if (c.tag == "Red")
            {
                ValidMoves.AddRange(FindValidMoves(c));
            }
            else
            {
                if (c.transform.position.z == 1)
                {
                    Debug.Log("Blue won because they got a counter to the back row");
                    return true;

                }
            }
        }
        if (ValidMoves.Count == 0)
        {
            Debug.Log("Blue won because Red has no moves");
            return true;
        }
        else
        {
            return false;
        }
    }

    void TogglePlayerTurn()
    {
        if (playerTurn)
        {
            Debug.Log("End of red turn");
            /*
            if(CheckRedWinner())
            {
                Debug.Log("Red won!");
                GameOver = true;
            }
            */
        }
        else
        {
            Debug.Log("End of blue turn");
            /*
            if (CheckBlueWinner())
            {
                Debug.Log("Blue won!");
                GameOver = true;
            }
            */
        }
        playerTurn = !playerTurn;
        //CheckWinner();

    }

    void CheckWinner()
    {
        if(!playerTurn)
        {
            if (CheckRedWinner())
            {
                GameOver = true;
                //Debug.Log("Red won!");
            }
        }
        else
        {
            if (CheckBlueWinner())
            {
                GameOver = true;
                //Debug.Log("Blue won!");
            }
        }        
    }
}
