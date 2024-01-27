using Godot;
using System;
using System.Collections.Generic;

namespace Lambchomp.Essentials;

[GlobalClass]
public partial class InvokeNodeEventAction : GameAction
{
    [Export] NodeEvent nodeEvent;
    [Export] NodePath nodePath;

    public override bool Invoke(Node node) {
        nodeEvent.Invoke(node.GetNode(nodePath));
        return true;
    }
}
