Unity OpenAI Tool Framework
Introduction
The Unity OpenAI Tool Framework is a simple yet powerful tool that integrates OpenAI into Unity games. It allows you to call functions within your game using AI decision-making, providing flexibility and adaptability to your game mechanics.

How It Works
The framework utilizes attributes Tool and Parameter to define tools and parameters for methods in your Unity scripts. These attributes provide essential information to the AI, enabling it to make informed decisions about which action to take and which method/function to call within the Unity game.

Tool Attribute
The Tool attribute is used to define a tool, providing its name and description.

csharp
Copy code
using Indie.Attributes;

[Tool("ToolName", "Description of the tool.")]
public void MyMethod()
{
    // Method implementation
}
Parameter Attribute
The Parameter attribute defines parameters for a method, including their name, description, and optional list of enums.

csharp
Copy code
using Indie.Attributes;

public class MyClass
{
    [Tool("ToolName", "Description of the tool.")]
    [Parameter("myParameter", "Description of the parameter.")]
    public void MyMethod(int myParameter)
    {
        // Method implementation
    }
}

Getting Started
To use the Unity OpenAI Tool Framework in your Unity project, follow these simple steps:

Download the Framework: Download the framework files and add them to your Unity project.

Implement Attributes: Add Tool and Parameter attributes to the methods in your scripts to define tools and parameters.

Integrate with OpenAI: Connect your Unity project to OpenAI to enable AI decision-making.

Call Functions: Let the AI make decisions based on defined tools and parameters, and call functions within your game accordingly.

Example
csharp
Copy code
using UnityEngine;
using Indie.Attributes;

public class MyGameController : MonoBehaviour
{
    [Tool("MovePlayer", "Move the player to a specified position.")]
    public void MovePlayer([Parameter("Position", "Position to move the player to.")] Vector3 position)
    {
        // Method implementation
    }

    // Other methods...
}
License
This project is licensed under the MIT License - see the LICENSE file for details.
