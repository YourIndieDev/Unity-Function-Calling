using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MultiSelectionView
{
    private List<string> options;
    private Dictionary<string, bool> selectionStates;
    private string title;

    public MultiSelectionView(string title, List<string> options)
    {
        this.title = title;
        this.options = options;
        selectionStates = new Dictionary<string, bool>();

        // Initialize all options as unselected by default
        foreach (var option in options)
            selectionStates[option] = false;
    }


    public void Draw()
    {
        GUILayout.Space(10);

        GUILayout.Label(title, EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();

        foreach (var option in options)
        {
            bool currentState = selectionStates[option];

            // Toggle the state when the button is clicked
            if (GUILayout.Button(option))
            {
                selectionStates[option] = !currentState; // Toggle
            }
        }

        GUILayout.EndHorizontal();

        GUILayout.Space(10);
    }

    public bool IsSelected(string option)
    {
        if (selectionStates.ContainsKey(option))
        {
            return selectionStates[option];
        }
        return false;
    }
}
