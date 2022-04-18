using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Environnement
{
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] [CanBeNull] private Collider2D collisionneur;

        private void OnValidate()
        {
            if (!collisionneur && TryGetComponent(out collisionneur))
            {
                
            }

            gameObject.layer = LayerMask.GetMask("Mur");
        }
    }
}