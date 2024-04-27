using Godot;
using System;
using System.Collections.Generic;

namespace Lambchomp.Essentials;

/// <summary> NodeReferenceSetter must be positioned after or below the object it is pointing to. </summary>
public partial class NodeReferenceSetter : Node
{
    [Export] private Node target;
    [Export] private NodeReference nodeReference;

    public NodeReference NodeReference => nodeReference;

    public override void _EnterTree() {
        nodeReference.Instance = target ?? this.GetParent<Node>();
        nodeReference ??= new NodeReference();
    }
}
