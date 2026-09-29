using System;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    private const int STRAIGHT_MOVEMENT_COST = 10;
    private const int DIAGONAL_MOVEMENT_COST = 14;

    private Grid<PathNode> grid;
    private List<PathNode> openList;
    private List<PathNode> closedList;
    public Pathfinding(int width, int height)
    {

        grid = new Grid<PathNode>(width, height, 1, (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        PathNode startNode = grid.GetGridObject(startX, startY);
        PathNode endNode = grid.GetGridObject(endX, endY);

        openList = new List<PathNode> { startNode };
        closedList = new List<PathNode>();

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                PathNode node = grid.GetGridObject(x, y);
                node.gCost = int.MaxValue;
                node.CalculateFCost();
                node.cameFromNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode)
            {
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeihgbourList(currentNode))
            {
                if(closedList.Contains(neighbourNode)) continue;
                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                if (tentativeGCost < neighbourNode.gCost)
                {
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if(!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }

        // No nodes on open list
        return null;
    }

    private List<PathNode> GetNeihgbourList(PathNode node)
    {
        List<PathNode> neighbours = new List<PathNode>();

        if (node.X - 1 >= 0)
        {
            // Left
            neighbours.Add(GetNode(node.X - 1, node.Y));
            // Left Down
            if (node.Y - 1 >= 0) neighbours.Add(GetNode(node.X - 1, node.Y - 1));
            //Left Up
            if (node.Y + 1 < grid.GetHeight()) neighbours.Add(GetNode(node.X - 1, node.Y + 1));
        }
        if (node.X + 1 < grid.GetWidth())
        {
            // Right
            neighbours.Add(GetNode(node.X + 1, node.Y));
            // Right Down
            if (node.Y - 1 >= 0) neighbours.Add(GetNode(node.X + 1, node.Y - 1));
            // Right Up
            if (node.Y + 1 < grid.GetHeight()) neighbours.Add(GetNode(node.X + 1, node.Y + 1));
        }
        // Down
        if (node.Y - 1 >= 0) neighbours.Add(GetNode(node.X, node.Y - 1));
        // Up
        if (node.Y + 1 < grid.GetHeight()) neighbours.Add(GetNode(node.X, node.Y + 1));

        return neighbours;
    }

    private PathNode GetNode(int x, int y)
    {
        return grid.GetGridObject(x, y);
    }

    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;
        while(currentNode.cameFromNode!=null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse();
        return path;
    }

    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.X - b.X);
        int yDistance = Mathf.Abs(a.Y - b.Y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return DIAGONAL_MOVEMENT_COST * Mathf.Min(xDistance, yDistance) + STRAIGHT_MOVEMENT_COST * remaining;
    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodes)
    {
        PathNode lowestFCostNode = pathNodes[0];
        for (int i = 0; i < pathNodes.Count; i++)
        {
            if (pathNodes[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = pathNodes[i];
            }
        }
        return lowestFCostNode;
    }
}
