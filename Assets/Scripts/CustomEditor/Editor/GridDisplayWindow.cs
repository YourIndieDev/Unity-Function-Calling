using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class GridDisplayWindow : EditorWindow
{
    private List<Object> droppedObjects = new List<Object>();
    private Vector2 scrollPosition;

    [MenuItem("Window/Grid Display")]
    private static void ShowWindow()
    {
        var window = GetWindow<GridDisplayWindow>();
        window.titleContent = new GUIContent("Grid Display");
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Drag and Drop Files Below", EditorStyles.boldLabel);

        // Create a box area for drag and drop
        var box = GUILayoutUtility.GetRect(0, 50, GUILayout.ExpandWidth(true));
        GUI.Box(box, "Drop Files Here");

        // Handle drag and drop logic
        if ((Event.current.type == EventType.DragUpdated || Event.current.type == EventType.DragPerform)
            && box.Contains(Event.current.mousePosition))
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            if (Event.current.type == EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();
                foreach (Object draggedObject in DragAndDrop.objectReferences)
                {
                    // Ignore non-asset files
                    if (!string.IsNullOrEmpty(AssetDatabase.GetAssetPath(draggedObject)))
                    {
                        droppedObjects.Add(draggedObject);
                    }
                }
            }
            Event.current.Use();
        }

        // Set up the scroll view for the grid
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        // Calculate the width for the grid
        float width = EditorGUIUtility.currentViewWidth - 50;
        int columns = Mathf.FloorToInt(width / 100); // 100 is the width for each grid item
        float singleColumnWidth = width / columns;

        // Start a new grid
        int count = 0;
        foreach (Object obj in droppedObjects)
        {
            // Begin a horizontal group on a new line if necessary
            if (count % columns == 0)
            {
                if (count > 0)
                    GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
            }

            // Display the object field for the dropped object
            GUILayout.BeginVertical(GUILayout.Width(singleColumnWidth));
            Texture2D preview = AssetPreview.GetAssetPreview(obj);
            if (preview == null)
                preview = AssetPreview.GetMiniThumbnail(obj);
            GUILayout.Label(preview, GUILayout.Width(90), GUILayout.Height(90));
            EditorGUILayout.ObjectField(obj, typeof(Object), false);
            GUILayout.EndVertical();

            count++;
        }
        if (count > 0)
            GUILayout.EndHorizontal();

        GUILayout.EndScrollView();
    }
}
