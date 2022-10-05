using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : MonoBehaviour
{

    public Material Red;
    public Material Blue;

    GameObject SelectionRing;
    public GameObject CurrentCell;


    // Start is called before the first frame update
    void Awake()
    {
        SelectionRing = transform.GetChild(0).gameObject;
    }

    // Make the counter selected/unselected
    public void SetSelected(bool active)
    {
        if (active)
            SelectionRing.SetActive(true);
        else
            SelectionRing.SetActive(false);
    }

    // Make the counter red
    public void SetRed()
    {
        GetComponent<Renderer>().material = Red;
    }

    // Make the counter blue
    public void SetBlue()
    {
        GetComponent<Renderer>().material = Blue;
    }

    // When a counter collides with a cell, note that cell as the current cell
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Cell")
        {
            CurrentCell = other.gameObject;
        }
    }
}
