using UnityEngine;

namespace SimpleBattleCity
{
    public class Spawner : MonoBehaviour
    {
        void OnDrawGizmos()
        {
            Color m = Color.magenta;
            m.a = 0.3f;
            Gizmos.color = m;
            Gizmos.DrawCube(transform.position, Vector3.one*2);
        }
    }
}