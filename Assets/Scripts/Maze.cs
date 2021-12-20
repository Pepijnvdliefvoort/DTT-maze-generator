using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Maze : MonoBehaviour {
	[SerializeField]
	[Range(10, 250)]
	private int width = 10;

	[SerializeField]
	private Text widthText;

	[SerializeField]
	[Range(10, 250)]
	private int height = 10;

	[SerializeField]
	private Text heightText;

	[SerializeField]
	private GameObject wallPrefab = null;

	private float size = 1;
	private Algorithm algorithm;
	private Cell[,] maze;

	public void SetWidth(float value) {
		width = (int)value;
		widthText.text = width.ToString();
	}

	public void SetHeight(float value) {
		height = (int)value;
		heightText.text = height.ToString();
	}

	private void Awake() {
		// Get maze algorithm currently added as a component to the maze
		algorithm = GetComponent<Algorithm>();
	}

	private void Start() {
		maze = algorithm.CreateEmptyMaze(width, height, size, wallPrefab);
		widthText.text = width.ToString();
		heightText.text = height.ToString();
	}

	public void SolveMaze() {
		// Call abstract method which runs the solve method
		algorithm.ClearMaze(maze);
		maze = algorithm.CreateEmptyMaze(width, height, size, wallPrefab);
		StartCoroutine(algorithm.Solve(maze, width, height));
	}
}
