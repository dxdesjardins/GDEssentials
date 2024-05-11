using Godot;
using System;
using System.Collections.Generic;

namespace Chomp.Essentials;

[GlobalClass]
public partial class InvokeReadyAction : ParamAction<Node>
{
    [Export] NodeReference nodeReference;
    [Export] NodePath nodePath;
    [Export] PackedScene packedScene;

    public override bool Invoke(Node node) {
        Node tar;
        if (nodeReference?.Instance != null)
            tar = nodeReference.Instance;
        else if (!nodePath.IsEmpty)
            tar = node.GetNode(nodePath);
        else
            tar = node;
        tar._Ready();
        return true;
    }

    public override bool Invoke(Node param, Node node) => Invoke(param);
}
