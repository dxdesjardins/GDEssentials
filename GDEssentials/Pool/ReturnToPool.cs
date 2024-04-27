using Godot;
using System;
using System.Collections.Generic;

namespace Lambchomp.Essentials;

public partial class ReturnToPool : Node
{
    public override void _Notification(int what) {
        switch (what) {
            case (int)NotificationReady:
                PoolManager.I.ReturnObjectsToPoolEvent.AddListener(() => this.RemoveParent());
                break;
            case (int)NotificationExitTree:
                bool isPooled = PoolManager.I.ReleaseObject(this.GetParent());
                if (!isPooled)
                    isPooled = PoolManager.I.AddObject(this.GetParent());
                break;
            case (int)NotificationPredelete:
                if (PoolManager.I.IsPooledObject(this.GetParent()))
                    GD.PrintErr("Warning: ", System.IO.Path.GetFileNameWithoutExtension(this.GetParent().SceneFilePath), " is being freed while part of an Object Pool.");
                break;
        }
    }
}
