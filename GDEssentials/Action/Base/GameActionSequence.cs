using Godot;
using System;
using System.Collections.Generic;

namespace Lambchomp.Essentials;

[GlobalClass]
public partial class GameActionSequence : GameAction
{
    [Export] GameAction[] gameActions;

    public override bool Invoke(Node node) {
        gameActions.InvokeGameActions(node);
        return true;
    }
}
