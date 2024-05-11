using Godot;
using System;
using System.Collections.Generic;

namespace Chomp.Essentials;

[GlobalClass]
public partial class SetRichTextLabelTextAction : ParamAction<string>
{
    [Export] NodePath richTextLabel;
    [Export] string text;

    public override bool Invoke(string param, Node node) {
        if (richTextLabel.IsEmpty)
            node.GetParent<RichTextLabel>().Text = param;
        else
            node.GetNode<RichTextLabel>(richTextLabel).Text = param;
        return true;
    }

    public override bool Invoke(Node node) => Invoke(text, node);
}
