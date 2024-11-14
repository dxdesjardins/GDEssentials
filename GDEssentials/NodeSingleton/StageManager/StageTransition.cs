using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chomp.Essentials;

public partial class StageTransition : Node
{
    [Export] private AnimationPlayer animationPlayer;

    public override async void _Ready() {
        animationPlayer ??= this.GetComponent<AnimationPlayer>();
        StageManager.TransitionBeforeFadeOut.Invoke(this);
        animationPlayer.Play("fade_out");
        await ToSignal(animationPlayer, AnimationPlayer.SignalName.AnimationFinished);
        StageManager.TransitionAfterFadeOut.Invoke(this);
        TaskCompletionSource<Node> tcs = new();
        void action(Node node) {
            tcs.TrySetResult(node);
            StageManager.LoadingComplete -= action;
        }
        StageManager.LoadingComplete += action;
        await tcs.Task;
        StageManager.TransitionBeforeFadeIn.Invoke(this);
        if (animationPlayer.HasAnimation("fade_in"))
            animationPlayer.Play("fade_in");
        else
            animationPlayer.Play("fade_out", default, -1, true);
        await ToSignal(animationPlayer, AnimationPlayer.SignalName.AnimationFinished);
        StageManager.TransitionAfterFadeIn.Invoke(this);
        this.Remove();
    }

}
