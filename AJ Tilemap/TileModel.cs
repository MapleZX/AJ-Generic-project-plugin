using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AJ.Generic.Tools.Tilemaps
{
    [System.Serializable]
    public class TileModel : AJModel
    {
        [SerializeField] private string m_key;
        [SerializeField] private string t_key;
        [SerializeField] private Vector3Int tilePos;
        public string Mkey { get => m_key; set => m_key = value; }
        public string TKey { get => t_key; set => t_key = value; }
        public Vector3Int TilePos { get => tilePos; set => tilePos = value; }
    }
}
