using Godot;
using System;
using static Godot.OpenXRInterface;

public partial class BallVisualDisplayer : HBoxContainer
{

    public void AddVisualElement(Control pVisualEntity)
    {

        this.AddChild(pVisualEntity);
    }

    public void ClearDisplay()
    {
        foreach (Node child in this.GetChildren())
        {
            child.QueueFree();
        }
    }
}
