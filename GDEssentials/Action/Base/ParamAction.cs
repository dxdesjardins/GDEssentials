using Godot;
using System;
using System.Collections.Generic;

namespace Chomp.Essentials;

public abstract partial class ParamAction<T> : GameAction
{
	public abstract bool Invoke(T param, Node node);
}
