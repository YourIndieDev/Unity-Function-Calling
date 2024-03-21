using CodiceApp;
using Danejw.Attribute;
using UnityEditor;
using UnityEngine;

public class HeadingView
{
    private string title;
    private string description;

    private Color backgroundColor = Color.gray;
    private Texture2D backgroundImage;

    public HeadingView(string title = "", string description = "", Color? background = null, Texture2D backgroundImage = null)
    {
        this.title = title;
        this.description = description;

        
        if (backgroundImage != null)
            this.backgroundImage = backgroundImage;
        else
            // Use the provided color or a default from DesignSettingsManager if null
            this.backgroundColor = background ?? Color.gray;
    }

    public void DrawHeading(float width, float height)
    {
        // Draw the blue rectangle background
        Rect headerRect = new Rect(0, 0, width, height);
        EditorGUI.DrawRect(headerRect, backgroundColor);

        // Set up the style for the title
        GUIStyle titleStyle = new GUIStyle(EditorStyles.largeLabel)
        {
            fontSize = 18,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.UpperCenter,
            normal = { textColor = Color.white } // White text color
        };

        // Set up the style for the description
        GUIStyle descriptionStyle = new GUIStyle(EditorStyles.label)
        {
            fontSize = 11,
            fontStyle = FontStyle.Italic,
            alignment = TextAnchor.LowerCenter,
            normal = { textColor = Color.white } // White text color
        };

        // Begin a vertical group to contain the header elements
        GUILayout.BeginArea(headerRect);
        GUILayout.BeginVertical();

        // Draw the title and description within the blue rectangle
        GUILayout.FlexibleSpace(); // This adds flexible space above the title to center it vertically
        GUILayout.Label(title, titleStyle);
        GUILayout.Label(description, descriptionStyle);
        GUILayout.FlexibleSpace(); // This adds flexible space below the description to center it vertically

        // End the vertical group
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
}

