using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Acteurs
{
    [RequireComponent(typeof(Controles))]
    public class Soni : Acteur
    {

        private static Soni cela;

        public static Soni soni
        {
            get
            {
                if (!cela) cela = FindObjectOfType<Soni>();
                return cela;
            }
        }
        
        [SerializeField] private Controles controles;
        
        protected override void OnValidate()
        {
            base.OnValidate();
            if (!controles) TryGetComponent(out controles);
        }

        void Start()
        {
        
        }

        private void Update()
        {
            Vector2 axes = Vector2.zero;
            if (controles.ControleDetecte("Droite", DetectionControle.estAppuye)) axes.x += 1;
            if (controles.ControleDetecte("Gauche", DetectionControle.estAppuye)) axes.x -= 1;
            if (controles.ControleDetecte("Haut", DetectionControle.estAppuye)) axes.y += 1;
            if (controles.ControleDetecte("Bas", DetectionControle.estAppuye)) axes.y -= 1;
            deplacementsTopDown.Direction = axes;
        }
        
        
    }
}
