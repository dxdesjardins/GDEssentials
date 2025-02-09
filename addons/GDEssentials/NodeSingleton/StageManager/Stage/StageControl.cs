using Godot;
using System;
using System.Collections.Generic;

namespace Chomp.Essentials;

[Tool]
public partial class StageControl : Control, IStage
{
    [Export] public Resource Data { get; private set; }
}
