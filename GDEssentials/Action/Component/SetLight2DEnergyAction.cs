using Godot;
using System;
using System.Collections.Generic;

namespace Chomp.Essentials;

[GlobalClass]
public partial class SetLight2DEnergyAction : ParamAction<float>
{
    [Export] NodePath light2D;
    [Export] float energy;
    [Export] float multiplier = 1;

    public override bool Invoke(float param, Node node) {
        if (light2D.IsEmpty)
            node.GetParent<Light2D>().Energy = param * multiplier;
        else
            node.GetNode<Light2D>(light2D).Energy = param * multiplier;
        return true;
    }

    public override bool Invoke(Node node) => Invoke(energy, node);
}
