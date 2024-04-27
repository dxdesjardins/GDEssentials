using Godot;
using System;
using System.Collections.Generic;

namespace Lambchomp.Essentials;

public partial class Vector2EventListener : ParamEventListener<Vector2>
{
    [Export] private Vector2Event eventObject;
    protected override ParamEvent<Vector2> EventObject { get { return eventObject; } }

    public override void Dispatch(Vector2 parameter) {
        eventActions?.Invoke(parameter, this);
    }
}
