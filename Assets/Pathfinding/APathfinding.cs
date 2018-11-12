using System.Collections.Generic;
using UnityEngine;

public class APathfinding {
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
    private static int[] gCosts;
    private static int[] hCosts;

    public static List<Node> FindPath(Vector3 startPos, Vector3 targetPos) {

		Node startNode = Grid.NodeFromWorldPoint(startPos);
		Node targetNode = Grid.NodeFromWorldPoint(targetPos);

		var openSet = new Heap<Node>(Grid.MaxSize, Compare);
		var closedSet = new HashSet<Node>();
        gCosts = new int[Grid.MaxSize];
        hCosts = new int[Grid.MaxSize];
        parents = new Node[Grid.MaxSize];

		openSet.Add(startNode);

		while (openSet.Count > 0) {
			Node currentNode = openSet.RemoveFirst();
			closedSet.Add(currentNode);

			if (currentNode == targetNode) {
				return RetracePath(startNode,targetNode);
			}

			foreach (Node neighbour in Grid.GetNeighbours(currentNode)) {
				if (!neighbour.walkable || closedSet.Contains(neighbour)) {
					continue;
				}

                int newMovementCostToNeighbour = gCosts[currentNode.Id] + GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < gCosts[neighbour.Id] || !openSet.Contains(neighbour))
                {
                    gCosts[neighbour.Id] = newMovementCostToNeighbour;
                    hCosts[neighbour.Id] = GetDistance(neighbour, targetNode);
                    parents[neighbour.Id] = currentNode;

                    if (!openSet.Contains(neighbour)) openSet.Add(neighbour);
                    else openSet.UpdateItem(neighbour);
                }
			}
		}
        return null;
	}

    public static List<Node> RetracePath(Node startNode, Node endNode) {
		List<Node> path = new List<Node>();
		Node currentNode = endNode;

		while (currentNode != startNode) {
			path.Add(currentNode);
            currentNode = parents[currentNode.Id];
        }
		path.Reverse();
		return path;
	}

    private static int Compare(Node nodeA, Node nodeB) {
        int compare = (gCosts[nodeA.Id] + hCosts[nodeA.Id]).CompareTo(gCosts[nodeB.Id] + hCosts[nodeB.Id]);
        if (compare == 0)
        {
            compare = hCosts[nodeA.Id].CompareTo(hCosts[nodeB.Id]);
        }
        return -compare;
    }

    private static int GetDistance(Node nodeA, Node nodeB) {
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY) return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
	}
}
