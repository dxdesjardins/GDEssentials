using Godot;
using System;
using System.Collections.Generic;

namespace Lambchomp.Essentials;

public partial class NodeReferenceSetter : Node
{
    [Export] private Node target;
    [Export] private NodeReference nodeReference;

    public NodeReference NodeReference => nodeReference;

    public override void _EnterTree() {
        RequestReady();
    }

    public override void _Ready() {
        nodeReference.Instance = target ?? this.GetParent<Node>();
        nodeReference ??= new NodeReference();
    }
}
