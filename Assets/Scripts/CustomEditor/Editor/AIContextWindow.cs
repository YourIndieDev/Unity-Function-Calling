using Danejw.Attribute;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Danejw
{
    public class AIContextWindow : EditorWindow
    {
        private GridView gridView;
        private HeadingView headingView;
        private TabView tabView;
        private MultiSelectionView selectionGroup;



        [MenuItem("Danejw/AI Context Window")]
        private static void ShowWindow()
        {
            var window = GetWindow<AIContextWindow>();
            window.titleContent = new GUIContent("AI Context Window");

            window.Show();
        }

        private void OnEnable()
        {
            headingView = new HeadingView("AI Editor", "Your 10x Toolbelt", DesignSettingsManager.Instance.headerColor);
            gridView = new GridView();

            // Initialize the TabGroup with titles and a callback for drawing content
            tabView = new TabView( new[] { "Settings", "Context", "Chat", "Image" }, DrawTabContent);

            var options = new List<string> { "Messages", "Warnings", "Critical" };
            selectionGroup = new MultiSelectionView("Include Logs", options);
        }

        private void OnGUI()
        {
            // Draw the background
            EditorGUI.DrawRect(new Rect(0, 0, position.width, position.height), DesignSettingsManager.Instance.backgroundColor);

            // Draw the header
            headingView.DrawHeading(position.width, 70f);

            // Adjust the Y position for the start of the grid view based on the header height
            GUILayout.BeginArea(new Rect(0, 70f, position.width, position.height - 70f));

            tabView.DrawTabs();

            GUILayout.EndArea();
        }

        // Callback for drawing content of the current tab
        private void DrawTabContent(int tabIndex)
        {
            switch (tabIndex)
            {
                case 0:
                    // Draw Settings
                    break;
                case 1:
                    DrawSelectionGroup();
                    DrawDragAndDropGrid();
                    break;
                case 2:
                    // Draw Chat
                    break;
            }
        }

        private void DrawDragAndDropGrid() => gridView.DrawGrid();

        private void DrawSelectionGroup()
        {
            selectionGroup?.Draw();

            // Now you can check for each option
            if (selectionGroup.IsSelected("Messages"))
            {
                // Display messages
            }
            if (selectionGroup.IsSelected("Warnings"))
            {
                // Display warnings
            }
            if (selectionGroup.IsSelected("Critical"))
            {
                // Display critical logs
            }
        }
    }
}

