using Godot;
using System.Collections.Generic;
using System;

namespace Lambchomp.Essentials;

public static class ExtensionsTilemap
{
    /// <summary>
    /// Get the tile data from a global position. Use tileData.Equals(default(Variant)) to check if no tile data exists here.
    /// </summary>
    public static Variant GetTileData(this TileMap tilemap, Vector2 pos, string layerName) {
        Vector2I tilePos = tilemap.LocalToMap(tilemap.ToLocal(pos));
        TileData tileData = tilemap.GetCellTileData(0, tilePos);
        if (tileData == null)
            return default;
        return tileData.GetCustomData(layerName);
    }

    public static Variant GetTileData(this TileMap tilemap, Vector2 pos, int layerId = 0) {
        Vector2I tilePos = tilemap.LocalToMap(tilemap.ToLocal(pos));
        TileData tileData = tilemap.GetCellTileData(layerId, tilePos);
        if (tileData == null)
            return default;
        return tileData.GetCustomDataByLayerId(layerId);
    }

    public static bool InTileMap(this TileMap tilemap, Vector2 pos) {
        Vector2I tilePos = tilemap.LocalToMap(tilemap.ToLocal(pos));
        return tilemap.GetCellSourceId(0, tilePos) != -1;
    }

    public static string GetTileName(this TileMap tilemap, Vector2 pos, int layer = 0) {
        if (!tilemap.TileExists(pos))
            return "";
        TileData tileData = tilemap.GetCellTileData(layer, tilemap.LocalToMap(pos));
        if (tileData == null)
            return "";
        Variant data = tileData.GetCustomData("Name");
        return data.AsString();
    }

    public static bool TileExists(this TileMap tilemap, Vector2 pos, int layer = 0) {
        return tilemap.GetCellSourceId(layer, tilemap.LocalToMap(pos)) != -1;
    }
}
