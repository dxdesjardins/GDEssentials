using Godot;
using System;
using System.Collections.Generic;

namespace Lambchomp.Essentials;

[GlobalClass]
public partial class QueueFreeAction : ParamAction<Node>
{
    [Export] NodeReference nodeReference;
    [Export] NodePath nodePath;

    public override bool Invoke(Node node) {
        Node tar;
        if (nodeReference?.Instance != null)
            tar = nodeReference.Instance;
        else if (nodePath != default)
            tar = node.GetNode(nodePath);
        else
            tar = node.GetParent();
        tar.QueueFree();
        return true;
    }

    public override bool Invoke(Node param, Node node) => Invoke(param);
}
