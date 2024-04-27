using Godot;
using System;
using System.Collections.Generic;

namespace Lambchomp.Essentials;

[GlobalClass]
[Tool]
public partial class PlayAnimationAction : ParamAction<string>
{
    [Export] NodePath animationPlayer;
    [Export] string animation;
    [Export] bool randomPlayBackwards = false;
    [Export] float customSpeed = 1;

    public override bool Invoke(string param, Node node) {
        AnimationPlayer tar;
        if (!animationPlayer.IsEmpty)
            tar = node.GetNode<AnimationPlayer>(animationPlayer);
        else
            tar = node.GetParent<AnimationPlayer>();
        if (randomPlayBackwards && new Random().NextDouble() < 0.5)
            tar.Play(param, default, -customSpeed, true);
        else
            tar.Play(param, default, customSpeed);
        return true;
    }

    public override bool Invoke(Node node) => Invoke(animation, node);
}
