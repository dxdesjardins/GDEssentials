using Godot;
using System;
using System.Collections.Generic;

namespace Lambchomp.Essentials;

[GlobalClass]
public partial class InstantiateChildAction : ParamAction<Node>
{
    [Export] bool preloadOnlyFirstCall = false;
    [Export] NodeReference nodeReference;
    [Export] NodePath nodePath;
    [Export] PackedScene packedScene;
    private Node preloadedNode;

    public override bool Invoke(Node node) {
        if (!IsInstanceValid(preloadedNode) || preloadedNode.IsQueuedForDeletion() || preloadedNode == null) {
            preloadedNode = packedScene.Instantiate<Node>();
            if (preloadOnlyFirstCall)
                return true;
        }
        if (preloadedNode.IsInsideTree())
            return true;
        Node tar;
        if (nodeReference != null)
            tar = nodeReference.Instance;
        else if (!nodePath.IsEmpty)
            tar = node.GetNode(nodePath);
        else
            tar = node;
        tar.InstantiateChild(preloadedNode);
        return true;
    }

    public override bool Invoke(Node param, Node node) => Invoke(param);
}
