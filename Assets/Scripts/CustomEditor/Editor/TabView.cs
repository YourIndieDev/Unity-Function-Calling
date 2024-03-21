using System;
using UnityEditor;
using UnityEngine;

public class TabView
{
    public int CurrentTabIndex { get; private set; }
    private string[] tabTitles;
    private Action<int> drawTabContent;
    private GUIStyle tabStyle;

    public TabView(string[] titles, Action<int> contentDrawer, GUIStyle style = null)
    {
        tabTitles = titles;
        drawTabContent = contentDrawer;
        tabStyle = style ?? EditorStyles.toolbarButton; // Use a default style if none is provided
    }

    public void DrawTabs()
    {
        // Draw the tab toolbar and get the currently selected tab index
        CurrentTabIndex = GUILayout.Toolbar(CurrentTabIndex, tabTitles, tabStyle);

        // Draw the content for the current tab
        drawTabContent?.Invoke(CurrentTabIndex);
    }
}

