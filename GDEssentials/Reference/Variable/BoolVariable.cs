using Godot;
using System;
using System.Collections.Generic;

namespace Chomp.Essentials;

[GlobalClass]
public partial class BoolVariable : GameVariable<bool>
{
    [Export] private bool Variable { get => variable; set => variable = value; }
}
