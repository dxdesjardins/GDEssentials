using Godot;
using System;
using System.Collections.Generic;

namespace Lambchomp.Essentials;

[GlobalClass]
public partial class QueueFreeAllChildrenAction : ParamAction<Node>
{
    [Export] NodeReference nodeReference;
    [Export] NodePath nodePath;

    public override bool Invoke(Node node) {
        Node tar;
        if (nodeReference?.Instance != null)
            tar = nodeReference.Instance;
        else if (!nodePath.IsEmpty)
            tar = node.GetNode(nodePath);
        else
            tar = node;
        foreach (Node child in tar.GetChildren()) {
            child.QueueFree();
            tar.RemoveChild(child);
        }
        return true;
    }

    public override bool Invoke(Node param, Node node) => Invoke(param);
}
