using Godot;
using System;
using System.Collections.Generic;

namespace Chomp.Essentials;

[GlobalClass]
public partial class GameActionSequence : GameAction
{
    [Export] GameAction[] gameActions;

    public override bool Invoke(Node node) {
        gameActions.Invoke(node);
        return true;
    }
}
