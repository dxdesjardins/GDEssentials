using Godot;
using System;
using System.Collections.Generic;

namespace Lambchomp.Essentials;

[GlobalClass]
public partial class DebugAction : ParamAction<float>
{
    [Export] NodePath target;

    public override bool Invoke(float param, Node node) {
        Node tar;
        if (!target.IsEmpty)
            tar = node.GetNode(target);
        //else if (param != default)
        //    tar = param;
        else
            tar = node.GetParent();
        GD.Print(param);
        return true;
    }

    public override bool Invoke(Node node) => Invoke(default, node);
}
