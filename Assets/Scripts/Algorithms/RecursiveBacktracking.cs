using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RecursiveBacktracking : Algorithm {
	[SerializeField]
	private GameObject mark = null;

	[SerializeField]
	private GameObject tobeMark = null;

	/// <inheritdoc />
	public override IEnumerator Solve(Cell[,] maze, int width, int height) {
		Stack<Cell> stack = new Stack<Cell>();

		// 1. Given a current cell as a parameter
		Cell current = maze[0, height - 1];

		// 2. Mark the current cell as visited
		current.Visited = true;
		stack.Push(current);

		// 3. While the current cell has any unvisited neighbour cells
		while (stack.Count > 0) {
			GameObject marked = Instantiate(mark, transform);

			Vector3 position = new Vector3(-width / 2 + current.Position.X, 0.1f, -height / 2 + current.Position.Y);
			marked.transform.position = position;

			yield return new WaitForSeconds(delay);

			// 1. Choose one of the unvisited neighbours
			Cell next = GetUnvisitedNeighbor(current, maze, width, height);
			if (next != null) {
				// 2. Remove the wall between the current cell and the chosen cell
				RemoveWalls(current, next);

				GameObject tobeMarked = Instantiate(tobeMark, transform);

				Vector3 markedPosition = new Vector3(-width / 2 + next.Position.X, 0, -height / 2 + next.Position.Y);
				tobeMarked.transform.position = markedPosition;
				yield return new WaitForSeconds(delay);

				// Push the current cell to the stack
				stack.Push(current);

				next.Visited = true;
				current = next;
			}
			// 3. Invoke the routine recursively for a chosen cell
			else if (stack.Count > 0) {
				current = stack.Pop();
			}
		}

		// Create entrance and exit for the maze after it is done generating
		CreateExitAndEntrance(maze, width, height);
	}

	/// <summary>
	/// Remove walls between two cells
	/// </summary>
	/// <param name="current">The current <see cref="Cell"/></param>
	/// <param name="next">The next <see cref="Cell"/> that will be used to generate from</param>
	private void RemoveWalls(Cell current, Cell next) {
		int x = current.Position.X - next.Position.X;
		int y = current.Position.Y - next.Position.Y;

		// Left
		if (x == -1) {
			Destroy(current.Walls[3]);
			Destroy(next.Walls[1]);
		}
		// Right
		else if (x == 1) {
			Destroy(current.Walls[1]);
			Destroy(next.Walls[3]);
		}

		// Top
		if (y == -1) {
			Destroy(current.Walls[0]);
			Destroy(next.Walls[2]);
		}
		// Bottom
		else if (y == 1) {
			Destroy(current.Walls[2]);
			Destroy(next.Walls[0]);
		}
	}

	/// <summary>
	/// Get a single random unvisited neighbor
	/// </summary>
	/// <param name="current">The current <see cref="Cell"/></param>
	/// <param name="maze">A two-dimensional array of <see cref="Cell"/> to check for neighbors</param>
	/// <param name="width">Width of the maze</param>
	/// <param name="height">Height of the maze</param>
	/// <returns>A random single <see cref="Cell"/> neighbor</returns>
	private Cell GetUnvisitedNeighbor(Cell current, Cell[,] maze, int width, int height) {
		List<Cell> neighbors = new List<Cell>();

		Cell top = null;
		if (!(current.Position.Y - 1 < 0)) {
			top = maze[current.Position.X, current.Position.Y - 1];
		}

		Cell left = null;
		if (!(current.Position.X + 1 >= width)) {
			left = maze[current.Position.X + 1, current.Position.Y];
		}

		Cell bottom = null;
		if (!(current.Position.Y + 1 >= height)) {
			bottom = maze[current.Position.X, current.Position.Y + 1];
		}

		Cell right = null;
		if (!(current.Position.X - 1 < 0)) {
			right = maze[current.Position.X - 1, current.Position.Y];
		}

		// Top
		if (top != null && !top.Visited) {
			neighbors.Add(top);
		}

		// Left
		if (left != null && !left.Visited) {
			neighbors.Add(left);
		}

		// Bottom
		if (bottom != null && !bottom.Visited) {
			neighbors.Add(bottom);
		}

		// Right
		if (right != null && !right.Visited) {
			neighbors.Add(right);
		}

		if (neighbors.Count > 0) {
			Cell neighbor = neighbors[Random.Range(0, neighbors.Count)];
			return neighbor;
		}

		return null;
	}
}
