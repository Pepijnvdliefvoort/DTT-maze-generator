using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MazeGenerator))]
public abstract class MazeAlgorithm : MonoBehaviour {
    private MazeGenerator m_mazeGenerator;
    protected MazeGenerator MazeGenerator => m_mazeGenerator;

    /// <summary>
    /// Whether the algorithm is/has to be running or not
    /// </summary>
    private bool running;
    /// <summary>
    /// The time variables used to keep track of the duration of a ran algorithm
    /// </summary>
    private float startTime, endTime;

    /// <summary>
    /// The name of the algorithm
    /// </summary>
    public string Name => GetType().Name;

    private void Awake() {
        m_mazeGenerator = GetComponent<MazeGenerator>();
    }

    private void Update() {
        // Checks if the algorithm is running
        if(!running) {
            return;
        }
        // Run the algorithm during this frame
        OnRun();
    }

    /// <summary>
    /// Initializes the maze algorithm
    /// </summary>
    public void Initialize() {
        Debug.Log($"Initializing '{GetType().Name}' algorithm...");
        OnInitialize();
    }

    /// <summary>
    /// Initiates the generation process of the algorithm
    /// </summary>
    public void Run() {
        Debug.Log($"Running '{GetType().Name}' algorithm...");
        running = true;
        startTime = Time.time;
    }

    /// <summary>
    /// Stops the generation process of the algorithm
    /// </summary>
    public void Stop() {
        running = false;
        endTime = Time.time;

        Debug.Log($"Stopping '{GetType().Name}' algorithm...");
        Debug.Log($"Time taken: {endTime - startTime:F6}s");

        OnStop();
    }

    /// <summary>
    /// The event fired once the maze initializes
    /// </summary>
    public abstract void OnInitialize();
    /// <summary>
    /// The event fired for every frame while the algorithm is running
    /// </summary>
    public abstract void OnRun();
    /// <summary>
    /// The event fired once the maze algorithm generation process stops
    /// </summary>
    public abstract void OnStop();
    /// <summary>
    /// The event fired for when the maze has to be cleared
    /// </summary>
    public abstract void ClearMaze();
}