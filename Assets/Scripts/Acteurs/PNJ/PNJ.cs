using System;
using System.Collections.Generic;
using Environnement;
using UnityEngine;

namespace Acteurs.PNJ
{
    public class PNJ : Acteur
    {
        [SerializeField] private bool aBloqueur;
        [SerializeField] [HideInInspector] private Bloqueur bloqueur;
        [SerializeField] private float distanceVision = 4;
        protected SoniLeo cibleSoni;

        [Header("Peur")] 
        [Tooltip("Seuil de peur à partir duquel le PNJ panique et s'enfuit")]
        [SerializeField] private float seuilPanique;
        private float peur = 0;
        private bool fuit;
        public bool Fuit => fuit;
        
        protected bool estAssomed;
        public bool EstAssomed => estAssomed;
        
        
        
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.3f, 0.71f, 0.77f);
            Gizmos.DrawWireSphere(transform.position, distanceVision);
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            if (aBloqueur)
            {
                if (!bloqueur && !TryGetComponent(out bloqueur))
                {
                    bloqueur = gameObject.AddComponent<Bloqueur>();
                }
            }
            if (!aBloqueur && (bloqueur || TryGetComponent(out bloqueur)))
            {
                Destroy(bloqueur);
            }
        }

        protected virtual void Update()
        {
            peur = 0;
            
            VoirSoniEtClonesPlusProche();

            if (peur >= seuilPanique)
            {
                Fuir();
            }
        }

        public void SeFaireAssomer()
        {
            estAssomed = true;
        }

        public void SeFairePorter(Vector2 ptPortage)
        {
            
        }

        public void SeFaireAssimiler()
        {
            
        }

        private void Fuir()
        {
            fuit = true;
        }
        
        protected virtual void VoirSoniEtClonesPlusProche()
        {
            List<Collider2D> sonisADist = new List<Collider2D>(Physics2D.OverlapCircleAll(
                transform.position, distanceVision, LayerMask.GetMask("Soni")));

            List<SoniLeo> sonisEnVue = new List<SoniLeo>();

            //On nettoie la liste des colliders en retirant ceux qui ne sont pas des acteurs,
            //et ceux qui sont cachés par un obstacle
            foreach (var coll  in sonisADist)
            {
                if (EstEnVue(coll) && coll.TryGetComponent(out SoniLeo soni))
                {
                    sonisEnVue.Add(soni);
                }
            }

            peur += sonisEnVue.Count;
            
            if (sonisEnVue.Count >= 1)
            {
                SoniLeo soniCurseur = sonisEnVue[0];
                sonisEnVue.Remove(soniCurseur);
                float distMin = Vector2.Distance(transform.position, soniCurseur.transform.position);
                 
                //On ne garde que l'acteur le plus proche
                while (sonisEnVue.Count > 1)
                {
                    foreach (var soni in sonisEnVue)
                    {
                        float distCurseur = Vector2.Distance(transform.position, soni.transform.position);
                        if (distCurseur < distMin)
                        {
                            distMin = distCurseur;
                            soniCurseur = soni;
                            sonisEnVue.Remove(soni);
                            break;
                        }
                    }
                }

                cibleSoni = soniCurseur;
            }
        }

        private bool EstEnVue(Collider2D coll)
        {
            return Physics2D.Linecast(transform.position, coll.transform.position,
                LayerMask.GetMask("Soni", "Clone", "Mur")).collider == coll;
        }
    }
}