using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace Acteurs
{
    public class Clone : Acteur
    {
        private Vector2 direction;

        public Vector2 Direction
        {
            get => direction;
            set
            {
                direction = value;
                deplacements.Direction = direction;
            }
        }
    }
}