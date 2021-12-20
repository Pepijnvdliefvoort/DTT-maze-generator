using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Algorithm;

public class Cell {
	public Position position;
	public GameObject[] Walls;
	public bool Visited { get; set; }

	public Cell(int x, int y) {
		Visited = false;
		position.X = x;
		position.Y = y;

		// Top left bottom right
		Walls = new GameObject[4];
	}
}
