using Godot;
using System;
using System.Collections.Generic;

namespace Chomp.Essentials;

[GlobalClass]
public partial class SetActiveAction : ParamAction<bool>
{
    [Export] NodePath nodePath;
    [Export] bool state;

    public override bool Invoke(bool param, Node node) {
        if (nodePath.IsEmpty)
            node.GetParent().SetActive(param);
        else
            node.GetNode(nodePath).SetActive(param);
        return true;
    }

    public override bool Invoke(Node node) => Invoke(state, node);
}
