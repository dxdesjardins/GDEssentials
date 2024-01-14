using Godot;
using System;
using System.Collections.Generic;

namespace Lambchomp.Essentials;

[GlobalClass]
public partial class SetSliderValueAction : ParamAction<float>
{
    [Export] NodePath slider;
    [Export] float value;

    public override bool Invoke(float param, Node node) {
        if (slider.IsEmpty)
            node.GetParent<Slider>().Value = param;
        else
            node.GetNode<Slider>(slider).Value = param;
        return true;
    }

    public override bool Invoke(Node node) => Invoke(value, node);
}
