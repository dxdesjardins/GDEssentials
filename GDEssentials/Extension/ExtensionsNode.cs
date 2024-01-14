using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lambchomp.Essentials;

public static class ExtensionsNode
{
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

    public static T GetComponent<T>(this Node node, bool isChild = true, bool includeParent = true) {
        if (isChild)
            node = node.GetParent() ?? node;
        if (includeParent && node is T parent)
            return parent;
        foreach (Node n in node.GetChildren(true)) {
            if (n is T child)
                return child;
        }
        return default;
    }

    public static void SetActive(this Node node, bool state) {
        node.ProcessMode = (state) ? Node.ProcessModeEnum.Inherit : Node.ProcessModeEnum.Disabled;
        node.SetPhysicsProcess(state);
        if (node is CanvasItem canvasItem)
            canvasItem.Visible = state;
    }

    public static Node InstantiateChild(this Node node, PackedScene packedScene, Vector2 position = default) {
        Node newScene = packedScene.Instantiate<Node>();
        return node.InstantiateChild(newScene, position);
    }

    public static Node InstantiateChild(this Node node, Node preloadedNode, Vector2 position = default) {
        if (preloadedNode is Node2D node2D)
            node2D.Position = position;
        else if (preloadedNode is Control control)
            control.Position = position;
        node.AddChild(preloadedNode);
        return preloadedNode;
    }

    /// <summary>
    /// Returns the Node that has the top rendered deep Sprite2D and at least one shallow child inheriting T.
    /// </summary>
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

    public static bool IsInGroup<[MustBeVariant] TEnum>(this Node node, Godot.Collections.Array<TEnum> allowedGroups) {
        var nodeGroups = node.GetGroups();
        for (int i = 0; i < allowedGroups.Count; i++) {
            for (int j = 0; j < nodeGroups.Count; j++) {
                if (allowedGroups[i].ToString() == nodeGroups[j].ToString())
                    return true;
            }
        }
        return false;
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
}
