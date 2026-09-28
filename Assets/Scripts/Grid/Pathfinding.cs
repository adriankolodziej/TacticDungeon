using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    private Grid<PathNode> grid;
    private List<PathNode> openList;
    private List<PathNode> closedList;
    public Pathfinding(int width, int height)
    {

        grid = new Grid<PathNode>(width, height, 1, (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));
    }

    private List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        PathNode startNode = grid.GetGridObject(startX, startY);

        openList = new List<PathNode> { startNode };
        closedList = new List<PathNode>();

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                PathNode node = grid.GetGridObject(x, y);
                node.gCost = int.MaxValue;
                node.CalculateFCost();
            }
        }

        return null;
    }
}
