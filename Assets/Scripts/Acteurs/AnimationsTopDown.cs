using UnityEngine;

namespace Acteurs
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class AnimationsTopDown : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;

        private void OnValidate()
        {
            if (!rb) TryGetComponent(out rb);
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
