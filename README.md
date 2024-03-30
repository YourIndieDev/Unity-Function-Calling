# Unity Function Calling Framework

## Introduction

The Unity Function Calling Framework is a simple yet powerful tool that integrates OpenAI into Unity games. It allows you to call functions within your game using AI decision-making, providing flexibility and adaptability to your game mechanics.

## How It Works

The framework utilizes attributes `Tool` and `Parameter` to define tools and parameters for methods in your Unity scripts. These attributes provide essential information to the AI, enabling it to make informed decisions about which action to take and which method/function to call within the Unity game.

### Tool Attribute

The `Tool` attribute is used to define a tool, providing its name and description.

```csharp
using Indie.Attributes;

[Tool("MyMethod", "Description of the tool.")]
public void MyMethod()
{
    // Method implementation
}
```

### Parameter Attribute

The `Parameter` attribute defines parameters for a method, including their name, description, and optional list of enums.

```csharp
using Indie.Attributes;

public class MyClass
{
    [Tool("MyMethod", "Description of the tool.")]
    [Parameter("myParameter", "Description of the parameter.")]
    public void MyMethod(int myParameter)
    {
        // Method implementation
    }
}
```

```csharp
using Indie.Attributes;

public class MyClass
{
    [Tool("MyMethod", "Description of the tool.")]
    [Parameter("param1", "Description of the parameter.")]
    [Parameter("param2", "Description of the parameter.", "option1", "option2", "option3")]
    public void MyMethod(int param1, string param2)
    {
        // Method implementation
    }
}
```

### Getting Started

To use the Unity Function Calling Framework in your Unity project, follow these simple steps:

1.  **Download the Framework**: Download the framework files and add them to your Unity project.
    
2.  **Implement Attributes**: Add `Tool` and `Parameter` attributes to the methods in your scripts to define tools and parameters.
    
3.  **Integrate with OpenAI**: Connect your Unity project to OpenAI to enable AI decision-making.
    
4.  **Call Functions**: Let the AI make decisions based on defined tools and parameters, and call functions within your game accordingly.


```csharp
using UnityEngine;
using Indie.Attributes;

public class MyGameController : MonoBehaviour
{
    public Brain brain;
    
    private void OnEnable()
    {
        brain?.RegisterScript(GetType(), this);
    }
    
    private void OnDisable()
    {
        brain?.UnRegisterScript(GetType());
    }


    [Tool("MovePlayer", "Move the player to a specified position.")]
    [Parameter("position", "Position to move the player to.")] 
    public void MovePlayer(Vector3 position)
    {
        // Method implementation
    }

    // Other methods...
}
```


