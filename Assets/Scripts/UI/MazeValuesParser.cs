using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

[RequireComponent(typeof(MazeGenerator))]
public class MazeValuesParser : MonoBehaviour {
    [Header("Object references")]
    [SerializeField]
    private TMP_InputField widthInput;
    [SerializeField]
    private TMP_InputField heightInput;
    [SerializeField]
    private TMP_Dropdown dropdown;

    private MazeGenerator m_mazeGenerator;

    private void Awake() {
        m_mazeGenerator = GetComponent<MazeGenerator>();
    }

    private void Start() {
        // Initializes the width and height input fields of the initial bounds of the maze
        Vector2Int mazeBounds = m_mazeGenerator.Bounds;
        widthInput.text = mazeBounds.x.ToString();
        heightInput.text = mazeBounds.y.ToString();
    }

    public void LoadAlgorithms(MazeAlgorithm[] algorithms) {
        // Resets the list of algorithms in the dropdown in the UI
        dropdown.options.Clear();
        foreach (MazeAlgorithm algorithm in algorithms) {
            dropdown.options.Add(new TMP_Dropdown.OptionData(algorithm.Name));
        }
    }

    /// <summary>
    /// Attempts to parse the input of the user for the settings of the maze
    /// </summary>
    public void ProcessValues() {
        if(!int.TryParse(widthInput.text, out int width) || !int.TryParse(heightInput.text, out int height)) {
            Debug.LogError("Failed to parse input value to int.");
            return;
        }

        string algorithmName = dropdown.options[dropdown.value].text;

        m_mazeGenerator.SetCurrentMazeValues(width, height, algorithmName);
    }
}