using Godot;
using System;
using System.Collections.Generic;

namespace Chomp.Essentials;

[GlobalClass]
public partial class IntVariable : GameVariable<int>
{
    [Export] private int Variable { get => variable; set => variable = value; }
}
