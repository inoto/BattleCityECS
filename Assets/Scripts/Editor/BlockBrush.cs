using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SimpleBattleCity
{
    [CustomGridBrush(true, false, false, "Block Brush")]
    public class BlockBrush : GridBrushBase
    {
        enum BlockType
        {
            Grey,
            Brick,
            Stone,
            Water,
            Forest,
            Ice
        }

        [SerializeField] BlockType Block;
        [SerializeField] GameObject[] blockPrefabs;
        public override void Paint(GridLayout grid, GameObject brushTarget, Vector3Int position)
        {
            base.Paint(grid, brushTarget, position);

            Vector2 halfCellSize = grid.cellSize / 2;
            GameObject go = PrefabUtility.InstantiatePrefab(blockPrefabs[(int)Block]) as GameObject;
            go.transform.position = (Vector2) grid.CellToWorld(position) + halfCellSize;
            go.transform.parent = grid.transform;
        }

        public override void Erase(GridLayout grid, GameObject brushTarget, Vector3Int position)
        {
            base.Erase(grid, brushTarget, position);

            Vector2 halfCellSize = grid.cellSize / 2;
            Vector2 pos = (Vector2) grid.CellToWorld(position) + halfCellSize;
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector3.forward);
            if (hit)
            {
                DestroyImmediate(hit.transform.gameObject);
            }
        }
    }
}
