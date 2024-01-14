using Godot;
using System;
using System.Collections.Generic;

namespace Lambchomp.Essentials;

[GlobalClass]
public partial class PlayAnimationAction : ParamAction<string>
{
    [Export] NodePath animationPlayer;
    [Export] string animation;
    [Export] float customSpeed = 1;

    public override bool Invoke(string param, Node node) {
        AnimationPlayer tar;
        if (animationPlayer.IsEmpty)
            tar = node.GetParent<AnimationPlayer>();
        else
            tar = node.GetNode<AnimationPlayer>(animationPlayer);
        tar.Play(param, default, customSpeed);
        return true;
    }

    public override bool Invoke(Node node) => Invoke(animation, node);
}
