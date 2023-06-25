using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using AJ.Generic.Utils;

namespace AJ.Generic.Tools.Tilemaps
{
    public class TilemapManager : MonoBehaviour
    {
        [SerializeField, TagProperty] private string tags;
        [SerializeField] private RegisterNameIngredient registerNameIngredient = new();
        public string RegisterName => !registerNameIngredient.isCustom ? name : registerNameIngredient.registerName;
        private Dictionary<string, TileBase> tiles;
        private Dictionary<string, Tilemap> maps = new();
        void Awake() => AJController.Register<TilemapManager>(RegisterName, gameObject);
        void Start() => Initialization();
        void OnDestroy() => AJController.UnRegister<TilemapManager>(RegisterName);
        void Initialization()
        {
            var map = GameObject.FindGameObjectsWithTag(tags.ToString());
            foreach (var m in map)
            {
                maps.Add(m.name, m.GetComponent<Tilemap>());
            }
        }
        public Tilemap GetTilemap(string key)
        {
            return maps[key];
        }
        public bool TryGetValue(string key, out Tilemap map)
        {
            return maps.TryGetValue(key, out map);
        }
        public void CreateTiles(List<TileBase> tiles)
        {
            this.tiles = new();
            foreach (var tile in tiles)
            {
                if (!this.tiles.ContainsKey(tile.name))
                {
                    this.tiles.Add(tile.name, tile);
                }
            }
        }
        public void CreateTiles(Dictionary<string, TileBase> tiles)
        {
            this.tiles = tiles;
        }
        public bool HasTile(string key)
        {
            var hasMap = TryGetValue(key, out var map);
            if (!hasMap) return hasMap;
            return HasTile(map);
        }
        public bool HasTile(Tilemap map)
        {
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var newTilePos = Vector3Int.FloorToInt(pos);
            var z = Mathf.FloorToInt(map.transform.position.z);
            newTilePos = new Vector3Int(newTilePos.x, newTilePos.y, z);
            return map.HasTile(newTilePos);
        }
        public bool HasTile(string key, Vector3Int position)
        {
            var hasMap = TryGetValue(key, out var map);
            if (!hasMap) return hasMap;
            return HasTile(map, position);
        }
        public bool HasTile(Tilemap map, Vector3Int position)
        {
            return map.HasTile(position);
        }
        public void Brush(Tilemap map, TileBase tile, Vector3Int position)
        {
            map.SetTile(position, tile);
        }
        public void Brush(string m_key, TileBase tile, Vector3Int target)
        {
            if (maps == null) return;
            if (!maps.Any()) return;
            var z = Mathf.FloorToInt(maps[m_key].transform.position.z);
            var newTilePos = new Vector3Int(target.x, target.y, z);
            maps[m_key].SetTile(newTilePos, tile);
        }
        public void Brush(string m_key, string t_key, Vector3Int target)
        {
            if (maps == null) return;
            if (tiles == null) return;
            if (!maps.Any()) return;
            if (!tiles.Any()) return;
            var z = Mathf.FloorToInt(maps[m_key].transform.position.z);
            var newTilePos = new Vector3Int(target.x, target.y, z);
            var haveTile = tiles.TryGetValue(t_key, out var tile);
            maps[m_key].SetTile(newTilePos, tile);
        }
        public void Brush(string m_key, string t_key)
        {
            if (maps == null) return;
            if (tiles == null) return;
            if (!maps.Any()) return;
            if (!tiles.Any()) return;
            if (Input.GetMouseButtonDown(0))
            {
                var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var newTilePos = Vector3Int.FloorToInt(pos);
                var z = Mathf.FloorToInt(maps[m_key].transform.position.z);
                newTilePos = new Vector3Int(newTilePos.x, newTilePos.y, z);
                var haveTile = tiles.TryGetValue(t_key, out var tile);
                maps[m_key].SetTile(newTilePos, tile);
            }           
        }      
    }
}
