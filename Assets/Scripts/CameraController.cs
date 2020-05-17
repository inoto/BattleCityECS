using UnityEngine;

namespace SimpleBattleCity
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] SpriteRenderer Field;

        Camera _camera;

        // [ExecuteInEditMode]
        void Awake()
        {
            // Debug.Log("Editor causes this Awake");

            _camera = GetComponent<Camera>();
            Field = GameObject.Find("Field").GetComponent<SpriteRenderer>();
            if (Field == null)
                return;

            Vector2 fieldSize = Field.size * Field.transform.localScale;
            if (Screen.width / (float)Screen.height <= fieldSize.x / fieldSize.y)
                _camera.orthographicSize = (fieldSize.x * Screen.height) / (Screen.width * 2f);
            else
                _camera.orthographicSize = (fieldSize.y / 2f);
        }
    }
}
