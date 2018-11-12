using UnityEngine;
using System.Collections;
using System;

public class Node {
    public static int currentId;
    public int Id { get; set; }
	public bool walkable;
	public Vector3 worldPosition;
	public int gridX;
	public int gridY;
    
	public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY) {
        Id = currentId;
        currentId++;
		walkable = _walkable;
		worldPosition = _worldPos;
		gridX = _gridX;
		gridY = _gridY;
	}
}
