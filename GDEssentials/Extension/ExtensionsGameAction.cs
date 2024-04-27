using Godot;
using System;
using System.Collections.Generic;

namespace Lambchomp.Essentials;

public static class ExtensionsGameAction
{
    public static void Invoke<T>(this GameAction[] gameActions, T value, Node node) {
        if (gameActions != null && gameActions.Length > 0)
            foreach (GameAction gameAction in gameActions)
                if (gameAction is ParamAction<T> paramAction) {
                    if (!paramAction.Invoke(value, node))
                        break;
                }
                else if (!gameAction.Invoke(node))
                    break;
    }

    public static void Invoke(this GameAction[] gameActions, Node node) {
        if (gameActions != null && gameActions.Length > 0)
            foreach (GameAction gameAction in gameActions)
                if (!gameAction.Invoke(node))
                    break;
    }
}
