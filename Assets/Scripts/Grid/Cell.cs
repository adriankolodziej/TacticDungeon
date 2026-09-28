using System;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    private Color originalColor;
    private int x;
    private int y;

    public int X { get { return x; } }
    public int Y { get { return y; } }

private void OnEnable()
    {
        GridEvents.OnCellChosen += ChooseCell;
    }

    private void OnDisable()
    {
        GridEvents.OnCellChosen -= ChooseCell;
    }
    public void SetCoordinates(int x, int y)
    {
        this.originalColor = this.GetComponent<Renderer>().material.color;
        this.x = x;
        this.y = y;
    }

    public void ChooseCell(object sender, (int x, int y) cellCoordinates)
    {
        var (x, y) = cellCoordinates;
        if(this.x == x && this.y == y)
        {
            this.GetComponent<Renderer>().material.color = Color.red;
            Debug.Log("Cell chosen at coordinates: " + x + ", " + y);
        }
        else
        {
            this.GetComponent<Renderer>().material.color = originalColor;
        }
    }
}
