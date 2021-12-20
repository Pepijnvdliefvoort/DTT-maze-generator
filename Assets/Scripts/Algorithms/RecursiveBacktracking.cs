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

	public override IEnumerator Solve(Cell[,] maze, int width, int height) {
		Stack<Cell> stack = new Stack<Cell>();

		Cell current = maze[0, height - 1];
		current.Visited = true;
		stack.Push(current);

		while (stack.Count > 0) {
			GameObject marked = Instantiate(mark, transform);

			Vector3 position = new Vector3(-width / 2 + current.position.X, 0.1f, -height / 2 + current.position.Y);
			marked.transform.position = position;

			yield return new WaitForSeconds(delay);

			Cell next = GetUnvisitedNeighbor(current, maze, width, height);
			if (next != null) {
				RemoveWalls(current, next);

				GameObject tobeMarked = Instantiate(tobeMark, transform);

				Vector3 markedPosition = new Vector3(-width / 2 + next.position.X, 0, -height / 2 + next.position.Y);
				tobeMarked.transform.position = markedPosition;
				yield return new WaitForSeconds(delay);

				// Push the current cell to the stack
				stack.Push(current);

				next.Visited = true;
				current = next;
			}
			else if (stack.Count > 0) {
				current = stack.Pop();
			}
		}

		CreateExitAndEntrance(maze, width, height);
	}

	private void RemoveWalls(Cell current, Cell next) {
		int x = current.position.X - next.position.X;
		int y = current.position.Y - next.position.Y;

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

	private Cell GetUnvisitedNeighbor(Cell current, Cell[,] maze, int width, int height) {
		List<Cell> neighbors = new List<Cell>();

		Cell top;
		if (current.position.Y - 1 < 0) {
			top = null;
		}
		else {
			top = maze[current.position.X, current.position.Y - 1];
		}

		Cell left;
		if (current.position.X + 1 >= width) {
			left = null;
		}
		else {
			left = maze[current.position.X + 1, current.position.Y];
		}

		Cell bottom;
		if (current.position.Y + 1 >= height) {
			bottom = null;
		}
		else {
			bottom = maze[current.position.X, current.position.Y + 1];
		}

		Cell right;
		if (current.position.X - 1 < 0) {
			right = null;
		}
		else {
			right = maze[current.position.X - 1, current.position.Y];
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

	//private CellWalls GetOppositeWall(CellWalls wall) {
	//	switch (wall) {
	//		case CellWalls.RIGHT: return CellWalls.LEFT;
	//		case CellWalls.LEFT: return CellWalls.RIGHT;
	//		case CellWalls.UP: return CellWalls.DOWN;
	//		case CellWalls.DOWN: return CellWalls.UP;
	//		default: return CellWalls.LEFT;
	//	}
	//}

	//private List<Neighbour> GetUnvisitedNeighbours(Position position, CellWalls[,] maze, int width, int height) {
	//	List<Neighbour> list = new List<Neighbour>();

	//	// Left
	//	if (position.X > 0) {
	//		if (!maze[position.X - 1, position.Y].HasFlag(CellWalls.VISITED)) {
	//			list.Add(new Neighbour {
	//				Position = new Position {
	//					X = position.X - 1,
	//					Y = position.Y,
	//				},
	//				SharedWall = CellWalls.LEFT
	//			});
	//		}
	//	}

	//	// Down
	//	if (position.Y > 0) {
	//		if (!maze[position.X, position.Y - 1].HasFlag(CellWalls.VISITED)) {
	//			list.Add(new Neighbour {
	//				Position = new Position {
	//					X = position.X,
	//					Y = position.Y - 1,
	//				},
	//				SharedWall = CellWalls.DOWN
	//			});
	//		}
	//	}

	//	// Up
	//	if (position.Y < height - 1) {
	//		if (!maze[position.X, position.Y + 1].HasFlag(CellWalls.VISITED)) {
	//			list.Add(new Neighbour {
	//				Position = new Position {
	//					X = position.X,
	//					Y = position.Y + 1,
	//				},
	//				SharedWall = CellWalls.UP
	//			});
	//		}
	//	}

	//	// Right
	//	if (position.X < width - 1) {
	//		if (!maze[position.X + 1, position.Y].HasFlag(CellWalls.VISITED)) {
	//			list.Add(new Neighbour {
	//				Position = new Position {
	//					X = position.X + 1,
	//					Y = position.Y,
	//				},
	//				SharedWall = CellWalls.RIGHT
	//			});
	//		}
	//	}

	//	return list;
	//}
}
