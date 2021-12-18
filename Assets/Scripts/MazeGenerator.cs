using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class MazeGenerator : MonoBehaviour {
    [Header("Size settings")]
    [SerializeField]
    private Vector2Int initialBounds = new Vector2Int(10, 10);
    [SerializeField]
    private float cellSize = 1f;
    public float CellSize => cellSize;

    private Vector2Int? bounds;
    public Vector2Int Bounds {
        get {
            return bounds ?? initialBounds;
        }
        set {
            bounds = new Vector2Int(Mathf.Max(2, value.x), Mathf.Max(2, value.y));
        }
    }

    private List<MazeAlgorithm> algorithms;
    public MazeAlgorithm[] Algorithms => algorithms.ToArray();
    private MazeAlgorithm currentAlgorithm;
    public MazeAlgorithm CurrentAlgorithm {
        get {
            return currentAlgorithm;
        }
        set {
            currentAlgorithm?.Stop();
            currentAlgorithm?.ClearMaze();
            currentAlgorithm = value;
            currentAlgorithm?.Initialize();
        }
    }

    [Header("Events")]
    [SerializeField]
    private UnityEvent<MazeAlgorithm[]> onAlgorithmsLoaded;

    private void Awake() {
        Bounds = initialBounds;

        algorithms = new List<MazeAlgorithm>();
        RegisterAlgorithm<RecursiveBacktracking>();

        // Notify that the algorithms are done loading
        onAlgorithmsLoaded.Invoke(algorithms.ToArray());
    }

    /// <summary>
    /// Registers a new algorithm to be used by the maze generator
    /// </summary>
    /// <typeparam name="T">The type of component (derived from <see cref="MazeAlgorithm"/>) to register</typeparam>
    private void RegisterAlgorithm<T>() where T : MazeAlgorithm {
        if(!TryGetComponent(out T algorithmComponent)) {
            Debug.LogWarning("An attempt to register an algorithm has failed due to it not being present as a component on the maze generator object.");
            return;
        }

        algorithms.Add(algorithmComponent);
    }

    /// <summary>
    /// Sets the values used once reinitializing the maze
    /// </summary>
    /// <param name="width">The width of the maze</param>
    /// <param name="height">The height of the maze</param>
    /// <param name="algorithmName">The name of the algorithm. The correct algorithm is attempted to get parsed if it exists</param>
    public void SetCurrentMazeValues(int width, int height, string algorithmName) {
        MazeAlgorithm algorithm = algorithms.FirstOrDefault(ma => ma.Name.Equals(algorithmName));
        if(algorithm == null) {
            Debug.LogError($"Failed to parse algorithm from '{algorithmName}' to a valid algorithm instance");
            return;
        }

        Bounds = new Vector2Int(width, height);
        CurrentAlgorithm = algorithm;
    }

    /// <summary>
    /// Runs the algorithm
    /// </summary>
    public void Run() {
        CurrentAlgorithm?.Run();
    }

    /// <summary>
    /// Get the local position in world-space by the zero-based x and y coordinates of a cell
    /// </summary>
    /// <param name="x">The zero-based x coordinate</param>
    /// <param name="y">The zero-based y coordinate</param>
    /// <returns>The local position in world-space of the given cell</returns>
    public Vector3 GetLocalCellPosition(int x, int y) {
        float localX = -Bounds.x * cellSize / 2 + (x * cellSize) + (cellSize / 2);
        float localY = Bounds.y * cellSize / 2 - (y * cellSize) + (cellSize / 2);
        return  transform.position + new Vector3(localX, localY);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;

        // Draws a small sphere at the center of each cell
        for(int x = 0; x < Bounds.x; x++) {
            for (int y = 0; y < Bounds.y; y++) {
                Gizmos.DrawSphere(GetLocalCellPosition(x, y), 0.075f * cellSize);
            }
        }
    }
}