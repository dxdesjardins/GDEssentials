using Godot;
using System;
using System.Collections.Generic;

namespace Chomp.Essentials;

[Tool]
public partial class Stage : Node
{
    [Export] public Resource Data { get; private set; }
}
