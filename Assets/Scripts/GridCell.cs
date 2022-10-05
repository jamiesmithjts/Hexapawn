using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    // Materials for the grid colour
    public Material black;
    public Material white;

    // Point where counters should sit on this cell
    Vector3 Midpoint;

    // The counter on top of this cell
    Counter currentCounter;

    // Reference to the selection that appears when the grid is a valid move
    GameObject SelectionSquare;


    private void Awake()
    {
        Midpoint = new Vector3(transform.position.x, transform.position.y + 0.6f, transform.position.z);
        SelectionSquare = transform.GetChild(0).gameObject;
    }

    // Make this cell black
    public void BecomeBlack()
    {
        GetComponent<Renderer>().material = black;
    }

    // Make this cell white
    public void BecomeWhite()
    {
        GetComponent<Renderer>().material = white;
    }

    // Return the midpoint
    public Vector3 GetMidPoint()
    {
        return Midpoint;
    }

    // Show the selection square
    public void SetSelected(bool active)
    {
        if(active)
            SelectionSquare.SetActive(true);
        else
            SelectionSquare.SetActive(false);
    }

    // When a counter enters this gridcell, make note of it
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Red" || other.tag == "Blue")
        {
            currentCounter = other.gameObject.GetComponent<Counter>();
            Debug.Log(other.name + " entered cell " + name);
        }
    }

    // and record it leaving
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Red" || other.tag == "Blue")
        {
            currentCounter = null;
            Debug.Log(other.name + " left cell " + name);
        }
    }

    // Get the current counter
    public Counter GetCounter()
    {
        return currentCounter;
    }
}
