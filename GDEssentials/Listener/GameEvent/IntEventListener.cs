using Godot;
using System;
using System.Collections.Generic;

namespace Lambchomp.Essentials;

public partial class IntEventListener : ParamEventListener<bool> {
    [Export] private BoolEvent eventObject;
    protected override ParamEvent<bool> EventObject { get {	return eventObject;	} }
    [Export] private GameAction[] onTrue;
    [Export] private GameAction[] onFalse;

    public override void Dispatch(bool parameter) {
        eventActions?.Invoke<bool>(parameter, this);
        if (parameter)
            onTrue?.Invoke(this);
        else
            onFalse?.Invoke(this);
    }
}
