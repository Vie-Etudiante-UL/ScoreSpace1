using System;
using UnityEngine;

namespace Acteurs
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class DeplacementsTopDown : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private Rigidbody2D rb;
        [Header("Déplacements")] 
        [Tooltip("en m par s")]
        [SerializeField] private float vitesseMax;
        [Tooltip("en m par s")]
        [SerializeField] private float acceleration;
        [Tooltip("en m par s")]
        [SerializeField] private float deceleration;

        private Vector2 direction;

        public Vector2 Direction
        {
            get => direction;
            set  => direction = value.normalized;
        }
    
        private void OnValidate()
        {
            if (!rb) TryGetComponent(out rb);
            rb.angularDrag = 0;
            rb.gravityScale = 0;
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void FixedUpdate()
        {
            SeDeplacer();
        }


        private void SeDeplacer()
        {
            Vector2 velocite = rb.velocity;
            if (direction.magnitude > 0)
            {
                velocite += direction * acceleration * Time.fixedDeltaTime;
                if (velocite.magnitude > vitesseMax) velocite = velocite.normalized * vitesseMax;
            }
            else
            {
                Vector2 decel = velocite.normalized * deceleration * Time.fixedDeltaTime;
            
                int ratioX = velocite.x < 0 ? -1 : 1;
                int ratioY = velocite.y < 0 ? -1 : 1;

                velocite.x -= Mathf.Clamp(Mathf.Abs(decel.x),0, Mathf.Abs(velocite.x)) * ratioX;
                velocite.y -= Mathf.Clamp(Mathf.Abs(decel.y),0, Mathf.Abs(velocite.y)) * ratioY;
            }

            rb.velocity = velocite;
        }
    }
}
