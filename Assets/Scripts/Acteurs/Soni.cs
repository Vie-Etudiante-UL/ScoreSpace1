using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using UnityEngine;
using UnityEngine.UI;

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
        [Header("Clonage")]
        [SerializeField] private Clone cloneBase;
        public bool peutCloner;
        private int capaciteClone = 1;
        private int clonesTires;
        [SerializeField] private int viandesPourLvlUp;
        private int nbrViandes;
        
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
            deplacements.Direction = axes;

            if (controles.ControleDetecte("Cloner", DetectionControle.quandRelache)) ClonerTirer();
        }

        private void ClonerTirer()
        {
            if (!peutCloner || capaciteClone <= clonesTires) return;
            if (Instantiate(cloneBase.gameObject, transform.position, new Quaternion())
                .TryGetComponent(out Clone nvClone))
            {
                clonesTires++;
                Vector2 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position)
                    .normalized;
                nvClone.Direction = direction;
            }
        }

        public void AssimilerClone(Clone cloneAAssimiler, int scoreViande, int nbrViande)
        {
            Destroy(cloneAAssimiler.gameObject);
            clonesTires--;
            nbrViandes += nbrViande;
            while (nbrViandes >= viandesPourLvlUp)
            {
                nbrViandes = viandesPourLvlUp - nbrViandes;
                capaciteClone++;
            }
        }
    }
}
