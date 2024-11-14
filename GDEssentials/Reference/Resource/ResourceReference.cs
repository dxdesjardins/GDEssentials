using Godot;
using System;
using System.Collections.Generic;

namespace Chomp.Essentials;

public abstract partial class ResourceReference<TDerived, TResource> : Resource where TResource : Resource
{
    private static TResource _Instance;

    public static TResource Instance {
        get {
            if (_Instance == null)
                CreateOrLoadInstance();
            return _Instance;
        }
    }

    public ResourceReference() {
        if (!Engine.IsEditorHint())
            return;
        _ = GDE.CallDeferred(() => {
            string uid = GetResourceUid();
            if (!string.IsNullOrEmpty(uid)) {
                long uidL = ResourceUid.TextToId(uid);
                if (!ResourceUid.HasId(uidL)) {
                    ResourceUid.AddId(uidL, this.ResourcePath);
                    GDE.Log($"ResourceReference: {typeof(TDerived)} UID({nameof(ResourceUidAttribute)}) has been assigned to Path({this.ResourcePath}).");
                }
                else if (GDE.UidToResource(uidL) is not TResource)
                    GDE.LogErr($"ResourceReference: {typeof(TDerived)} UID({nameof(ResourceUidAttribute)}) is pointing to the wrong resource type.");
            }
            string path = GetResourcePath();
            if (string.IsNullOrEmpty(path))
                return;
            Resource resource = GD.Load(GetResourcePath());
            if (resource is null)
                GDE.LogErr($"ResourceReference: {typeof(TDerived)} is not pointing to a resource.");
            else if (resource is not TResource)
                GDE.LogErr($"ResourceReference: {typeof(TDerived)} is pointing to the incorrect type of resource.");
        });
    }

    private static void CreateOrLoadInstance() {
        string filePath = GetResourcePath();
        if (!string.IsNullOrEmpty(filePath))
            _Instance = GD.Load<TResource>(filePath);
        else
            filePath = "res://";
        if (Engine.IsEditorHint()) {
            if (_Instance != null)
                return;
            _Instance = (TResource)Activator.CreateInstance(typeof(TResource));
            ResourceSaver.Save(_Instance, filePath);
            GDE.Log($"ResourceReference: {typeof(TDerived).Name} instance has been created at Path({filePath}).");
        }
    }

    private static string GetResourcePath() {
        var attributes = typeof(TDerived).GetCustomAttributes(true);
        foreach (object attribute in attributes) {
            if (attribute is ResourceUidAttribute uidAttribute)
                return GDE.UidToPath(uidAttribute.Uid);
            else if (attribute is ResourcePathAttribute pathAttribute)
                return pathAttribute.Path;
        }
        GDE.LogErr($"ResourceReference: {typeof(TDerived)} does not have a valid {nameof(ResourceUidAttribute)} or {nameof(ResourcePathAttribute)}.");
        return string.Empty;
    }

    private static string GetResourceUid() {
        var attributes = typeof(TDerived).GetCustomAttributes(true);
        foreach (object attribute in attributes) {
            if (attribute is ResourceUidAttribute uidAttribute)
                return uidAttribute.Uid;
        }
        return string.Empty;
    }
}
