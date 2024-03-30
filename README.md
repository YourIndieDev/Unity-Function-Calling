# Unity OpenAI Tool Framework

## Introduction

The Unity OpenAI Tool Framework is a simple yet powerful tool that integrates OpenAI into Unity games. It allows you to call functions within your game using AI decision-making, providing flexibility and adaptability to your game mechanics.

## How It Works

The framework utilizes attributes `Tool` and `Parameter` to define tools and parameters for methods in your Unity scripts. These attributes provide essential information to the AI, enabling it to make informed decisions about which action to take and which method/function to call within the Unity game.

### Tool Attribute

The `Tool` attribute is used to define a tool, providing its name and description.

```csharp
using Indie.Attributes;

[Tool("ToolName", "Description of the tool.")]
public void MyMethod()
{
    // Method implementation
}
```
