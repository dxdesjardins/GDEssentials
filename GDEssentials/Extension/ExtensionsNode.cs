using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chomp.Essentials;

public static class ExtensionsNode
{
    public static T GetComponentInTree<T>() {
        Window root = (Engine.GetMainLoop() as SceneTree).Root;
        return root.GetComponentInChildren<T>(false);
    }

    public static T[] GetComponentsInChildren<T>(this Node node, bool isChild = true, bool includeParent = true) {
        return GetComponentsInChildren(node, isChild, includeParent).OfType<T>().ToArray();
    }

    public static Node[] GetComponentsInChildren(this Node node, bool isChild = true, bool includeParent = true) {
        List<Node> components = new() { };
        if (isChild)
            node = node.GetParent() ?? node;
        if (includeParent)
            components.Add(node);
        foreach (Node i in node.GetChildren(true)) {
            foreach (Node j in i.GetComponentsInChildren(false, true))
                components.Add(j);
        }
        return components.ToArray();
    }

    public static T GetComponentInChildren<T>(this Node node, bool isChild = true, bool includeParent = true) {
        if (isChild)
            node = node.GetParent() ?? node;
        if (includeParent && node is T parent)
            return parent;
        if (node != null) {
            foreach (Node i in node.GetChildren(true)) {
                foreach (Node j in i.GetComponentsInChildren(false, true))
                    if (j is T component)
                        return component;
            }
        }
        return default;
    }

    public static T GetComponentInParent<T>(this Node node) {
        Node parent = node.GetParent();
        T result;
        do {
            parent = parent.GetParent();
            result = parent.GetComponent<T>(false, true);
        }
        while (result != null);
        return result;
    }

    public static T GetComponent<T>(this Node node, bool isChild = true, bool includeParent = true) {
        if (isChild)
            node = node.GetParent() ?? node;
        if (includeParent && node is T parent)
            return parent;
        var nodes = node.GetChildren(true);
        if (nodes.Count == 0)
            return default;
        foreach (Node n in nodes) {
            if (n is T child)
                return child;
        }
        return default;
    }

    public static void SetActive(this Node node, bool state) {
        if (node is CanvasItem canvasItem)
            canvasItem.Visible = state;
        node.SetPhysicsProcess(state);
        node.ProcessMode = (state) ? Node.ProcessModeEnum.Inherit : Node.ProcessModeEnum.Disabled;
    }

    public static Node AddChild(this Node parent, Node instance, Vector2 position = default) {
        if (instance is Node2D node2D)
            node2D.Position = position;
        else if (instance is Control control)
            control.Position = position;
        parent.AddChild(instance);
        return instance;
    }

	public static Node InstantiateChild(this Node parent, PackedScene packedScene, Vector2 position = default, bool rename = true) {
		Node instance = packedScene.Instantiate<Node>();
        if (rename)
		    instance.MakeNameUnique();
		Node child = parent.AddChild(instance, position);
		return child;
	}

	public static T InstantiateChild<T>(this Node parent, Vector2 position = default) where T : Node {
		Node instance = (Node)Activator.CreateInstance(typeof(T));
		return parent.AddChild(instance, position) as T;
	}

	/// <summary> Returns the Node that has the top rendered deep Sprite2D and at least one shallow child inheriting T. </summary>
	public static Node GetTopRenderedNode<T>(this Node[] nodes, bool areChildren = true) {
        List<Node> tNodes = new List<Node>();
        foreach (Node node in nodes)
            if (node.GetComponent<T>(areChildren) != null)
                tNodes.Add(node);
        Sprite2D topS = null;
        Node topN = null;
        foreach (Node tNode in tNodes) {
            Sprite2D s = tNode.GetComponentInChildren<Sprite2D>(areChildren);
            if (s != null && s.Visible) {
                if (topS == null) {
                    topS = s;
                    topN = tNode;
                    continue;
                }
                int sZIndex = s.GetRelativeZIndex();
                int topSZIndex = topS.GetRelativeZIndex();
                if (sZIndex > topSZIndex) {
                    topS = s;
                    topN = tNode;
                    continue;
                }
                else if (sZIndex < topSZIndex)
                    continue;
                if (s.GetParentOrNull<Node2D>().YSortEnabled || topS.GetParentOrNull<Node2D>().YSortEnabled) {
                    if (s.Position.Y > topS.Position.Y) {
                        topS = s;
                        topN = tNode;
                        continue;
                    }
                    else if (s.Position.Y < topS.Position.Y)
                        continue;
                }
            }
        }
        return topN;
    }

    public static int GetRelativeZIndex(this CanvasItem canvasItem) {
        CanvasItem node = canvasItem;
        int z_index = canvasItem.ZIndex;
        if (!canvasItem.ZAsRelative)
            return canvasItem.ZIndex;
        while (node.GetParentOrNull<CanvasItem>() is CanvasItem parent) {
            z_index += parent.ZIndex;
            if (!parent.ZAsRelative)
                break;
            node = parent;
        }
        return z_index;
    }

    public static bool IsInGroup<[MustBeVariant] TEnum>(this Node node, Godot.Collections.Array<TEnum> groups) {
        var nodeGroups = node.GetGroups();
        for (int i = 0; i < groups.Count; i++) {
            for (int j = 0; j < nodeGroups.Count; j++) {
                if (groups[i].ToString() == nodeGroups[j].ToString())
                    return true;
            }
        }
        return false;
    }

    public static bool IsInGroup(this Node node, string[] groups) {
        for (int i = 0; i < groups.Length; i++) {
            if (node.IsInGroup(groups[i]))
                return true;
        }
        return false;
    }

    public static Node[] GetNodesInGroup(this Node[] nodes, string group) => GetNodesInGroup<Node>(nodes, group);

    public static T[] GetNodesInGroup<T>(this T[] nodes, string group) where T : Node {
        List<T> groupNodes = new();
        foreach (T node in nodes) {
            if (node.IsInGroup(group))
                groupNodes.Add(node);
        }
        return groupNodes.ToArray();
    }

    public static int GetTreeDepth(this Node node) {
        int depth = 0;
        while (node.GetParent() != null) {
            depth++;
            node = node.GetParent();
        }
        return depth;
    }

    public static int GetSiblingIndex(this Node node) {
        if (node.GetParent() == null)
            return -1;
        for (int i = 0; i < node.GetParent().GetChildCount(); i++)
            if (node.GetParent().GetChild(i) == node)
                return i;
        return -1;
    }

    public static async void CallDeferred(this GodotObject node, Action action, int frames = 1) {
        for (int i = 0; i < frames; i++)
            await node.ToSignal(Engine.GetMainLoop() as SceneTree, SceneTree.SignalName.ProcessFrame);
        action.Invoke();
    }

    public static void SafeAddChild(this Node parent, Node child) {
        if (parent.IsNodeReady())
            parent.AddChild(child);
        else
            parent.CallDeferred(Node.MethodName.AddChild, child);
    }

    public static Node GetAncestor(this Node node, int generations) {
        for (int i = 0; i < generations; i++)
            node = node.GetParent();
        return node;
    }

    public static void RemoveParent(this Node node) {
        node.GetParent().GetParent()?.RemoveChild(node.GetParent());
    }

    public static void Remove(this Node node) {
        node.GetParent()?.RemoveChild(node);
    }

    public static void SafeRemoveParent(this Node node) {
        Node parent = node.GetParent();
        Node grandParent = parent.GetParent();
        if (grandParent.IsNodeReady())
            grandParent.RemoveChild(parent);
        else
            grandParent.CallDeferred(Node.MethodName.RemoveChild, parent);
    }

    public static Node2D LookAt(this Node2D node, Vector2 target) {
        node.Rotation = node.GlobalPosition.AngleToPoint(target);
        return node;
    }

    public static bool IsPartOfStage(this Node node) {
        do {
            node = node.GetParent();
            if (node is Stage)
                return true;
        }
        while (node != null);
        return false;
    }

    public static Node GetStage(this Node node) {
        do {
            node = node.GetParent();
            if (node is Stage)
                return node;
        }
        while (node != null);
        return null;
    }

    public static Node GetScene(this Node node) {
        if (node.Owner != null)
            return node.Owner;
        do {
            node = node.GetParent();
            if (!string.IsNullOrEmpty(node.SceneFilePath))
                return node;
        }
        while (node != null);
        return null;
    }

    public static string MakeNameUnique(this Node node, string name = null) {
        name ??= node.Name + node.GetHashCode();
        node.Name = name;
        return name;
	}
}
