using Godot;
using System;
using System.Collections.Generic;

namespace Chomp.Essentials;

[GlobalClass]
public partial class SetModulateAlphaAction : ParamAction<float>
{
    [Export] NodePath target;
    [Export] float alpha = 0;

    public override bool Invoke(float param, Node node) {
        if (target.IsEmpty) {
            Color mod = node.GetParent<CanvasItem>().Modulate;
            node.GetParent<CanvasItem>().Modulate = new Color(mod.R, mod.G, mod.B, param);
        }
        else {
            Color mod = node.GetNode<CanvasItem>(target).Modulate;
            node.GetNode<CanvasItem>(target).Modulate = new Color(mod.R, mod.G, mod.B, param);
        }
        return true;
    }

    public override bool Invoke(Node node) => Invoke(alpha, node);
}
