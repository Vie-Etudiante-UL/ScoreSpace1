using UnityEngine;

namespace Acteurs
{
    [RequireComponent(typeof(Rigidbody2D), typeof(DeplacementsTopDown))]
    public abstract class Acteur : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] protected Rigidbody2D rb;
        [SerializeField] protected DeplacementsTopDown deplacements;

        protected virtual void OnValidate()
        {
            if (!rb) TryGetComponent(out rb);
            if (!deplacements) TryGetComponent(out deplacements);
        }
    }
  
}