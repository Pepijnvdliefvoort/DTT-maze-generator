using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Algorithm : MonoBehaviour {
	[SerializeField]
	protected float delay = 0.5f;

	/// <summary>
	/// A method that implements an algorithm which can solve a maze
	/// </summary>
	/// <param name="maze">A two-dimensional array of cells that is used to carve out a maze</param>
	/// <param name="width">Width of the maze</param>
	/// <param name="height">Height of the maze</param>
	/// <returns>IEnumerator for the coroutine</returns>
	public abstract IEnumerator Solve(Cell[,] maze, int width, int height);

	/// <summary>
	/// Create a grid of cells with the wall prefab
	/// </summary>
	/// <param name="width">Width of the maze</param>
	/// <param name="height">Height of the maze</param>
	/// <param name="size">Size of the cells</param>
	/// <param name="wallPrefab">The wall prefab that is used to generate walls</param>
	/// <returns>A two-dimensional array of <see cref="Cell"/> [x, y]</returns>
	public Cell[,] CreateEmptyMaze(int width, int height, float size, GameObject wallPrefab) {
		Cell[,] maze = new Cell[width, height];

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				Cell newCell = new Cell(x, y);
				maze.SetValue(newCell, x, y);
				Vector3 position = new Vector3(-width / 2 + x, 0, -height / 2 + y);

				// Top
				GameObject upWall = Instantiate(wallPrefab, transform);
				newCell.Walls[0] = upWall;
				upWall.transform.position = position + new Vector3(0, 0, size / 2);
				upWall.transform.localScale = new Vector3(size, upWall.transform.localScale.y, upWall.transform.localScale.z);

				// Left
				GameObject leftWall = Instantiate(wallPrefab, transform);
				newCell.Walls[1] = leftWall;
				leftWall.transform.position = position + new Vector3(-size / 2, 0, 0);
				leftWall.transform.localScale = new Vector3(size, leftWall.transform.localScale.y, leftWall.transform.localScale.z);
				leftWall.transform.eulerAngles = new Vector3(0, 90, 0);

				// First row
				if (y == 0) {
					// Bottom
					GameObject bottomWall = Instantiate(wallPrefab, transform);
					newCell.Walls[2] = bottomWall;
					bottomWall.transform.position = position + new Vector3(0, 0, -size / 2);
					bottomWall.transform.localScale = new Vector3(size, bottomWall.transform.localScale.y, bottomWall.transform.localScale.z);
				}

				// Last column
				if (x == width - 1) {
					// Right
					GameObject rightWall = Instantiate(wallPrefab, transform);
					newCell.Walls[3] = rightWall;
					rightWall.transform.position = position + new Vector3(+size / 2, 0, 0);
					rightWall.transform.localScale = new Vector3(size, rightWall.transform.localScale.y, rightWall.transform.localScale.z);
					rightWall.transform.eulerAngles = new Vector3(0, 90, 0);
				}
			}
		}

		return maze;
	}

	protected void CreateExitAndEntrance(Cell[,] maze, int width, int height) {
		Destroy(maze[0, height - 1].Walls[1]);
		Destroy(maze[width - 1, 0].Walls[3]);
	}

	public void ClearMaze(Cell[,] maze) {
		// Loop through entire grid
		for (int x = 0; x < maze.GetLength(0); x++) {
			for (int y = 0; y < maze.GetLength(1); y++) {

				// Loop through all walls on a cell
				for (int i = 0; i < maze[x, y].Walls.Length; i++) {
					Destroy(maze[x, y].Walls[i]);
				}
			}
		}

		GameObject[] marks = GameObject.FindGameObjectsWithTag("Respawn");
		foreach (GameObject mark in marks) {
			Destroy(mark);
		}
	}

	public struct Position {
		public int X;
		public int Y;
	}
}
