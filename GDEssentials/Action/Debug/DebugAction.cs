using Godot;
using System;
using System.Collections.Generic;

namespace Lambchomp.Essentials;

[GlobalClass]
public partial class DebugAction : ParamAction<Node>
{
    [Export] NodePath target;

    public override bool Invoke(Node param, Node node) {
        Node tar;
        if (!target.IsEmpty)
            tar = node.GetNode(target);
        else if (param != null)
            tar = param;
        else
            tar = node.GetParent();
        // Do stuff with tar
        return true;
    }

    public override bool Invoke(Node node) => Invoke(null, node);
}
