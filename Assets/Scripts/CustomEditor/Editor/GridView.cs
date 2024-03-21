using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class GridView
{
    public List<Object> DroppedObjects { get; private set; } = new List<Object>();
    private Vector2 scrollPosition;
    private float iconSize = 20.0f; // Default icon size, can be adjusted with the slider
    private const float minIconSize = 20.0f; // Minimum icon size
    private const float maxIconSize = 100.0f; // Maximum icon size

    public void DrawGrid()
    {
        // Add top padding to the horizontal group
        GUILayout.Space(5); // Adjust this value for more or less top padding

        GUILayout.BeginHorizontal();

        // Add left padding to the horizontal group
        GUILayout.Space(20); // Adjust the value to add more or less padding

        GUILayout.Label("Drag and Drop Files Below", EditorStyles.boldLabel);

        // Flexible space will push the following controls to the right
        GUILayout.FlexibleSpace();

        // Slider control
        GUILayout.Label("Icon Size:", GUILayout.Width(70));
        iconSize = GUILayout.HorizontalSlider(iconSize, minIconSize, maxIconSize, GUILayout.Width(150));

        // Add right padding to the horizontal group
        GUILayout.Space(20); // Adjust the value to add more or less padding

        // End the horizontal group
        GUILayout.EndHorizontal();

        // Add bottom padding to the horizontal group
        GUILayout.Space(10); // Adjust this value for more or less bottom padding


        // Create a box area for drag and drop
        var box = GUILayoutUtility.GetRect(0, 50, GUILayout.ExpandWidth(true));
        GUI.Box(box, "Drop Files Here");

        // Handle drag and drop logic
        Event evt = Event.current;
        if ((evt.type == EventType.DragUpdated || evt.type == EventType.DragPerform) && box.Contains(evt.mousePosition))
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            if (evt.type == EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();
                foreach (Object draggedObject in DragAndDrop.objectReferences)
                {
                    if (!string.IsNullOrEmpty(AssetDatabase.GetAssetPath(draggedObject)))
                    {
                        // Check if the object already exists in the list
                        if (!DroppedObjects.Contains(draggedObject))
                        {
                            DroppedObjects.Add(draggedObject);
                        }
                    }
                }
            }
            evt.Use();
        }

        // Check if the slider is at its minimum value
        if (Mathf.Approximately(iconSize, minIconSize))
        {
            // Slider is at minimum value, draw a list without icons
            DrawList();
        }
        else
        {
            // Slider is not at minimum value, draw a grid with icons
            DrawGridWithIcons();
        }
    }

    private void DrawList()
    {
        // Begin a scroll view for the list
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        foreach (Object obj in DroppedObjects)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.ObjectField(obj, typeof(Object), false);

            // Draw a small button on the right to remove the object
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                DroppedObjects.Remove(obj);
                // Exit the loop early to prevent invalid iteration after removal
                break;
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();
    }

    public void DrawGridWithIcons()
    {
        // Set up the scroll view for the grid
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        // Calculate the width for the grid
        float width = EditorGUIUtility.currentViewWidth - 50;
        int columns = Mathf.FloorToInt(width / 100); // Assuming 100 is the width for each grid item
        float singleColumnWidth = width / columns;

        GUILayout.BeginHorizontal();
        for (int i = DroppedObjects.Count - 1; i >= 0; i--) // Iterate backwards
        {
            // Draw the object field with a delete button
            GUILayout.BeginVertical(GUILayout.Width(singleColumnWidth));
            Texture2D preview = AssetPreview.GetAssetPreview(DroppedObjects[i]);
            if (preview == null)
                preview = AssetPreview.GetMiniThumbnail(DroppedObjects[i]);

            // Use the iconSize variable to determine the size of the preview
            Rect rect = GUILayoutUtility.GetRect(iconSize, iconSize);
            GUI.DrawTexture(rect, preview, ScaleMode.ScaleToFit);


            // Draw a small button on the top right
            Rect buttonRect = new Rect(rect.xMax - 20, rect.yMin, 20, 20);
            if (GUI.Button(buttonRect, "X"))
            {
                // Remove the object from the list if the button is clicked
                DroppedObjects.RemoveAt(i);
                // Skip the rest of the loop since the collection has changed
                continue;
            }

            EditorGUILayout.ObjectField(DroppedObjects[i], typeof(Object), false);
            GUILayout.EndVertical();

            if ((i - 1) % columns == columns - 1)
            {
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.EndScrollView();
    }
}
