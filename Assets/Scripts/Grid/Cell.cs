using UnityEngine;

public class Cell : MonoBehaviour
{
    private int x;
    private int y;

    public int X { get { return x; } }
    public int Y { get { return y; } }

    public void SetCoordinates(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}
