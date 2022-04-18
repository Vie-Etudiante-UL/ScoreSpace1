using System;
using UnityEngine;

namespace Acteurs
{
    [RequireComponent(typeof(Controles))]
    public class SoniLeo : Acteur
    {
        [SerializeField] private Controles controles;

        protected override void OnValidate()
        {
            base.OnValidate();
            if (!controles) TryGetComponent(out controles);
        }

        private void Update()
        {
            SeDeplacer();
        }

        private void SeDeplacer()
        {
            Vector2 axes = Vector2.zero;
            if (controles.ControleDetecte("Droite", DetectionControle.estAppuye)) axes.x += 1;
            if (controles.ControleDetecte("Gauche", DetectionControle.estAppuye)) axes.x -= 1;
            if (controles.ControleDetecte("Haut", DetectionControle.estAppuye)) axes.y += 1;
            if (controles.ControleDetecte("Bas", DetectionControle.estAppuye)) axes.y -= 1;
            deplacements.Direction = axes;
        }
    }
}