using Godot;
using System;
using System.Collections.Generic;

namespace Lambchomp.Essentials;

public partial class SceneInstantiateListener : Node
{
    [Export] private GameAction[] instantiateActions;

    public override void _Notification(int what) {
        if (what == NotificationSceneInstantiated) {
            instantiateActions.Invoke(this);
        }
    }
}
