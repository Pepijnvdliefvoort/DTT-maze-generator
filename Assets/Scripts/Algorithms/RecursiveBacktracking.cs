using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class RecursiveBacktracking : MazeAlgorithm {
    [Header("Algorithm settings")]
    [SerializeField]
    [Tooltip("The amount of iteration steps the algorithm runs every second. Increasing this number will result in a lower frame rate. NOTE: This algorithm will always do atleast 1 iteration every frame.")]
    private int iterationsPerSeconds = 5000;
    [SerializeField]
    private GameObject stepIndicator;

    [Header("Cell settings")]
    [SerializeField]
    [Tooltip("This indicates the zero-based position of the cell to start the algorithm from.")]
    private Vector2Int initialCell;
    [SerializeField]
    [Tooltip("This indicates the zero-based y-position of the cell to start the maze at.")]
    private int startCellPosition;
    [SerializeField]
    [Tooltip("This indicates the zero-based y-position of the cell to end the maze at.")]
    private int endCellPosition;

    [Header("Prefab settings")]
    [SerializeField]
    private GameObject cellPrefab;
    [SerializeField]
    private GameObject wallPrefab;

    private MazeCell[,] cells;
    private MazeCell currentCell;
    private Stack<MazeCell> cellStack;

    public override void OnInitialize() {
        // Initializes the 2D array with the size of the maze
        cells = new MazeCell[MazeGenerator.Bounds.x, MazeGenerator.Bounds.y];
        // Initializes a new stack used by the algorithm
        cellStack = new Stack<MazeCell>();

        // Two-dimensional loop to initialize and draw all individual cells in the maze
        for (int y = 0; y < cells.GetLength(1); y++) {
            for (int x = 0; x < cells.GetLength(0); x++) {
                Transform cellTransform = Instantiate(cellPrefab, transform).transform;
                cellTransform.name += $" ({x + 1}, {y + 1})";

                cellTransform.localPosition = MazeGenerator.GetLocalCellPosition(x, y);

                MazeCell cell = cellTransform.GetComponent<MazeCell>();
                cell.Initialize(MazeGenerator, new Vector2Int(x, y), wallPrefab);
                cells[x, y] = cell;
            }
        }

        // Set an initial cell and mark it as visited
        MazeCell initialCell = cells[this.initialCell.x, this.initialCell.y];
        initialCell.Visited = true;
        cellStack.Push(initialCell);

        // Remove the walls of the start and end of the maze
        Destroy(cells[0, startCellPosition][Direction.LEFT]);
        Destroy(cells[cells.GetLength(0) - 1, endCellPosition][Direction.RIGHT]);

        // Create an indicator object on the initial cell to track the algorithm while it is running
        stepIndicator.transform.position = transform.position + MazeGenerator.GetLocalCellPosition(initialCell.Position.x, initialCell.Position.y);
        stepIndicator.SetActive(true);
        stepIndicator.transform.localScale = Vector3.one * MazeGenerator.CellSize;
    }

    public override void OnRun() {
        // Calculate the amount of iterations this frame has to run
        int iterations = (int) Mathf.Max(iterationsPerSeconds * Time.deltaTime, 1);

        // Loop through the amount of earlier caluclated iterations
        for (int iteration = 0; iteration < iterations; iteration++) {
            // Check whether the stack is empty, which indicates the maze generation is completed
            if(cellStack.Count == 0) {
                Stop();
                break;
            }

            // Retrieve all direction enum values as an array in a random order
            Direction[] directions = System.Enum.GetValues(typeof(Direction)).Cast<Direction>().OrderBy(x => Random.value).ToArray();

            // Retrieve the current cell in the algorithm from the stack
            currentCell = cellStack.Pop();
            // Set the new position of the indicator to track the algorithm
            stepIndicator.transform.position = transform.position + MazeGenerator.GetLocalCellPosition(currentCell.Position.x, currentCell.Position.y);

            Vector2Int currentCellPosition = currentCell.Position;

            // Check all directions if the adjecant cell has been visited yet to continue the algorithm
            MazeCell visitingCell = null;
            Direction? randomDirection = null;
            foreach(Direction directionIteration in directions) {
                Vector2Int randomDirectionVector = directionIteration.ToVector();

                // Check if the adjecant cell of the given direction is not out of bounds
                int cellX = currentCellPosition.x + randomDirectionVector.x, cellY = currentCellPosition.y + randomDirectionVector.y;
                if (cellX >= MazeGenerator.Bounds.x || cellX < 0 || cellY >= MazeGenerator.Bounds.y || cellY < 0) {
                    continue;
                }

                MazeCell randomVisitingCell = cells[cellX, cellY];

                // Check whether the adjecant cell has been visited yet
                if(randomVisitingCell.Visited) {
                    continue;
                }

                randomDirection = directionIteration;
                visitingCell = randomVisitingCell;
            }

            // Check if a valid direction is present
            if(!randomDirection.HasValue) {
                continue;
            }

            // Check if a valid adjecant cell is present
            if (visitingCell == null) {
                continue;
            }

            // Remove the walls between the current cell and the adjecant cell
            Destroy(currentCell[randomDirection.Value]);
            Destroy(visitingCell[randomDirection.Value.GetOpposite()]);

            // Mark the adjecant cell as visited
            visitingCell.Visited = true;

            // Update the stack to push both the current cell and the adjecant cell to the stack
            cellStack.Push(currentCell);
            cellStack.Push(visitingCell);
        }
    }

    public override void OnStop() {
        // Disable the indicator to track the algorithm
        stepIndicator.SetActive(false);
    }

    public override void ClearMaze() {
        // Remove all cells in the maze
        foreach(MazeCell cell in cells) {
            Destroy(cell.gameObject);
        }
    }
}