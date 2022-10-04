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

    public void SetSelected()
    {
        SelectionRing.SetActive(true);
    }

    public void SetUnselected()
    {
        SelectionRing.SetActive(false);
    }

    public void SetRed()
    {
        GetComponent<Renderer>().material = Red;
    }

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
            //Debug.Log("Counter enter cell " + other.transform.name);
        }
    }
}
