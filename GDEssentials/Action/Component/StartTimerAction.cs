using Godot;
using System;
using System.Collections.Generic;

namespace Chomp.Essentials;

[GlobalClass]
public partial class StartTimerAction : ParamAction<float>
{
    [Export] NodePath timer;
    [Export] float time = -1;

    public override bool Invoke(float param, Node node) {
        if (timer.IsEmpty)
            node.GetParent<Timer>().Start(param);
        else
            node.GetNode<Timer>(timer).Start(param);
        return true;
    }

    public override bool Invoke(Node node) => Invoke(time, node);
}
