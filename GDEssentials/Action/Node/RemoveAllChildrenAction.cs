using Godot;
using System;
using System.Collections.Generic;

namespace Chomp.Essentials;

[GlobalClass]
public partial class RemoveAllChildrenAction : ParamAction<Node>
{
    [Export] NodeReference nodeReference;
    [Export] NodePath nodePath;
    [Export] PackedScene[] packedSceneExceptions;

    public override bool Invoke(Node node) {
        Node tar;
        if (nodeReference?.Instance != null)
            tar = nodeReference.Instance;
        else if (!nodePath.IsEmpty)
            tar = node.GetNode(nodePath);
        else
            tar = node;
        foreach (Node child in tar.GetChildren()) {
            bool exception = false;
            for (int i = 0; i < packedSceneExceptions.Length; i++)
                if (packedSceneExceptions[i].ResourcePath == child.SceneFilePath)
                    exception = true;
            if (!exception)
                tar.RemoveChild(child);
        }
        return true;
    }

    public override bool Invoke(Node param, Node node) => Invoke(param);
}
