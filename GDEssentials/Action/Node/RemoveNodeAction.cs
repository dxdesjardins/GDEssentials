using Godot;
using System;
using System.Collections.Generic;

namespace Chomp.Essentials;

[GlobalClass]
public partial class RemoveNodeAction : ParamAction<Node>
{
    [Export] private NodeReference nodeReference;
    [Export] private NodePath nodePath;
    [Export] private int numParentsUp = 0;

    public override bool Invoke(Node param, Node node) {
        Node tar;
        if (nodeReference?.Instance != null)
            tar = nodeReference.Instance;
        else if (!nodePath.IsEmpty)
            tar = node.GetNode(nodePath);
        else if (param != null)
            tar = param;
        else
            tar = node.GetParent();
        for (int i = 0; i < numParentsUp; i++)
            tar = tar.GetParent();
        tar.GetParent()?.RemoveChild(tar);
        return true;
    }

    public override bool Invoke(Node node) => Invoke(null, node);
}
