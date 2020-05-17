using UnityEngine;

namespace SimpleBattleCity
{
    [ExecuteInEditMode]
    public class AABB : MonoBehaviour
    {
        public Bounds Bounds;

        Color draw = Color.green;

        
        void Awake()
        {
            Bounds.center = transform.position;
        }

        void OnDrawGizmos()
        {
            draw.a = 0.3f;
            Gizmos.color = draw;
            Gizmos.DrawCube(Bounds.center, Bounds.size);
        }
    }
}