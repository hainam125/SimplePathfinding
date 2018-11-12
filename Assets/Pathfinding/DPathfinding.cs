using System.Collections.Generic;
using UnityEngine;

public class DPathfinding {
    private static Grid grid;

    public static Grid Grid
    {
        get
        {
            if (grid == null) grid = GameObject.FindObjectOfType<Grid>();
            return grid;
        }
    }

    private static Node[] parents;
    private static int[] costs;

    public static List<Node> FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = Grid.NodeFromWorldPoint(startPos);
        Node targetNode = Grid.NodeFromWorldPoint(targetPos);

        var openSet = new Heap<Node>(Grid.MaxSize, Compare);
        var closedSet = new HashSet<Node>();
        parents = new Node[Grid.MaxSize];
        costs = new int[Grid.MaxSize];
        for (int i = 0; i < costs.Length; i++) costs[i] = int.MaxValue;
        costs[startNode.Id] = 0;

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet.RemoveFirst();
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }

            foreach (Node neighbour in Grid.GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newMovementCostToNeighbour = costs[currentNode.Id] + GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < costs[neighbour.Id] || !openSet.Contains(neighbour))
                {
                    costs[neighbour.Id] = newMovementCostToNeighbour;
                    parents[neighbour.Id] = currentNode;

                    if (!openSet.Contains(neighbour)) openSet.Add(neighbour);
                    else openSet.UpdateItem(neighbour);
                }
            }
        }
        return null;
    }

    public static List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = parents[currentNode.Id];
        }
        path.Reverse();
        return path;
    }

    private static int Compare(Node nodeA, Node nodeB)
    {
        int compare = (costs[nodeA.Id]).CompareTo(costs[nodeB.Id]);
        return -compare;
    }

    private static int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
