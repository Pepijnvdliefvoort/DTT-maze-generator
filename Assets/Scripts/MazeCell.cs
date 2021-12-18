using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour {
    /// <summary>
    /// The child wall objects of the cell
    /// </summary>
    private GameObject[] walls;
    /// <summary>
    /// The zero-based position in the maze of where the cell is located at
    /// </summary>
    public Vector2Int Position { get; private set; }
    /// <summary>
    /// Whether the cell has been visited yet by the recursive backtracking algorithm
    /// </summary>
    public bool Visited { get; set; }

    /// <summary>
    /// Creates a cell at the given position with the given wall objects
    /// </summary>
    /// <param name="generator">The maze generator instance to retrieve its settings</param>
    /// <param name="position">The position in the maze to create the cell at</param>
    /// <param name="wallObject">The wall object to use when placing a wall in the cell</param>
    public void Initialize(MazeGenerator generator, Vector2Int position, GameObject wallObject) {
        Position = position;

        walls = new GameObject[4];

        // Loop for each instance of wall to create
        for(int i = 0; i < walls.Length; i++) {
            // Create a new wall object in the scene
            Transform wallTransform = Instantiate(wallObject, transform).transform;

            float cellSize = generator.CellSize;
            Vector3 wallPosition = Vector3.zero;
            float wallRotation = 0f;

            // Switch decision on how to position and rotate the wall on the cell
            switch(i) {
                // Left
                case 0:
                    wallPosition = new Vector3(-cellSize / 2, 0f);
                    break;
                // Top
                case 1:
                    wallPosition = new Vector3(0f, -cellSize / 2);
                    wallRotation = 90f;
                    break;
                // Right
                case 2:
                    wallPosition = new Vector3(cellSize / 2, 0f);
                    break;
                // Bottom
                case 3:
                    wallPosition = new Vector3(0f, cellSize / 2);
                    wallRotation = 90f;
                    break;
            }

            // Set the position, rotation and scale of the wall
            wallTransform.localPosition = wallPosition;
            wallTransform.localScale *= cellSize;
            wallTransform.Rotate(0f, 0f, wallRotation);

            // Add the newly created wall to the array of walls of the cell
            walls[i] = wallTransform.gameObject;
        }
    }

    public GameObject this[Direction direction] => walls[(int) direction];
}