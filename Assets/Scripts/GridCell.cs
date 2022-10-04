using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    public Material black;
    public Material white;
    Vector3 Midpoint;

    Counter currentCounter;

    GameObject SelectionSquare;

    public bool Occupied = false;

    private void Awake()
    {
        Midpoint = new Vector3(transform.position.x, transform.position.y + 0.6f, transform.position.z);
        SelectionSquare = transform.GetChild(0).gameObject;
    }

    public void BecomeBlack()
    {
        GetComponent<Renderer>().material = black;
    }

    public void BecomeWhite()
    {
        GetComponent<Renderer>().material = white;
    }

    public Vector3 GetMidPoint()
    {
        return Midpoint;
    }

    public void SetSelected()
    {
        SelectionSquare.SetActive(true);
    }

    public void SetUnselected()
    {
        SelectionSquare.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Red" || other.tag == "Blue")
        {
            currentCounter = other.gameObject.GetComponent<Counter>();
            //Debug.Log(other.name + " entered cell " + name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Red" || other.tag == "Blue")
        {
            currentCounter = null;
            //Debug.Log(other.name + " left cell " + name);
        }
    }

    public Counter GetCounter()
    {
        return currentCounter;
    }
}
