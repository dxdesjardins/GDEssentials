using Godot;
using System;
using System.Collections.Generic;

namespace Lambchomp.Essentials;

[GlobalClass]
public partial class PrintAction : ParamAction<string>
{
    [Export] private string message;

    public override bool Invoke(string param, Node node) {
        if (String.IsNullOrEmpty(message))
            GD.Print(message);
        else
            GD.Print(param);
        return true;
    }

    public override bool Invoke(Node node) => Invoke(message, node);
}
