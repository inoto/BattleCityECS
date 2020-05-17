using UnityEngine;

namespace SimpleBattleCity
{
    public class Player : MonoBehaviour
    {
        BoxCollider2D collider;
        [SerializeField] BoxCollider2D colliderOther;
        [SerializeField] LayerMask Layers;
        public Bounds bounds;

        void Awake()
        {
            collider = GetComponent<BoxCollider2D>();
            // bounds = GetComponent<TankTileProvider>().Entity.GetComponent<TankTileComponent>().Collider;
        }

        void OnDrawGizmos()
        {
            // bounds = GetComponent<TankTileProvider>().Entity.GetComponent<TankTileComponent>().Collider;
            Color y = Color.yellow;
            y.a = 0.3f;
            Gizmos.color = y;
            Gizmos.DrawCube(bounds.center, bounds.size);
        }
    }
}