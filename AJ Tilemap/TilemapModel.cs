using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AJ.Generic.Tools;
using AJ.Generic.Extension;

namespace AJ.Generic.Tools.Tilemaps
{
    [System.Serializable]
    public class TilemapModel : AJSavedModel
    {
        [SerializeField] private List<TileModel> tiles;
        public List<TileModel> Tiles => tiles;
        public void AddElement(TileModel tile)
        {
            if (this.tiles == null) tiles = new();
            this.tiles.AddElement(tile, (o, n) => o.TilePos.Equals(n.TilePos));
        }
        public void ChangeTile(TileModel tile)
        {
            if (this.tiles == null) tiles = new();
            var oldTile = this.tiles.ResultElement(s => s.TilePos.Equals(tile.TilePos));
            if (oldTile != null)
            {
                this.tiles.Remove(oldTile);
            }
            AddElement(tile);
            SaveData(Name);
            CloudData();
        }
    }
}
