using Godot;
using System;
using System.Collections.Generic;

namespace Chomp.Essentials;

[GlobalClass]
public partial class InstantiateChildAction : ParamAction<Node>
{
    [Export] bool useObjectPooling = true;
    [Export] NodeReference nodeReference;
    [Export] NodePath nodePath;
    [Export] PackedScene packedScene;

    public override bool Invoke(Node node) {
        Node tar;
        if (nodeReference != null)
            tar = nodeReference.Instance;
        else if (!nodePath.IsEmpty)
            tar = node.GetNode(nodePath);
        else
            tar = node;
        if (useObjectPooling)
            PoolManager.Spawn(packedScene, tar);
        else
            tar.InstantiateChild(packedScene);
        return true;
    }

    public override bool Invoke(Node param, Node node) => Invoke(param);
}
