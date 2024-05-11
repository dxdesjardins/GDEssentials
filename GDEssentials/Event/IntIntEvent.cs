using Godot;
using System;
using System.Collections.Generic;

namespace Chomp.Essentials;

[GlobalClass]
[Tool]
public partial class IntIntEvent : ParamEvent<(int, int)>
{
}
