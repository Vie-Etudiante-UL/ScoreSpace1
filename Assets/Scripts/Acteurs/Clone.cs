using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace Acteurs
{
    public class Clone : Acteur
    {
        public Vector2 Direction
        {
            get => deplacements.Direction;
            set => deplacements.Direction = value;
        }
        [SerializeField] private float distanceCourseMax;
        [SerializeField] private float distanceParcourue;
        private Vector3 positiondepart;

        [Header("Viande")] 
        [SerializeField] private Transform accumulateurViande;
        [SerializeField] private GameObject viandeBase;
        private List<Transform> viandes = new List<Transform>();
        private float scoreViande;
        [SerializeField] private float multScoreViande = 1;
        [SerializeField] private float decalageVerticalViandes;
        
        
        private bool surLAller = true;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(positiondepart, distanceCourseMax);
        }

        private void Start()
        {
            positiondepart = transform.position;
        }

        private void Update()
        {
            GererAllerRetour();
        }

        private void GererAllerRetour()
        {
            if (surLAller)
            {
                distanceParcourue = Vector2.Distance(transform.position, positiondepart);
                if (distanceParcourue >= distanceCourseMax) surLAller = false;
            }
            else Direction = (Soni.soni.transform.position - transform.position).normalized;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Scientifique scienti))
            {
                Destroy(scienti.gameObject);
                AjouterViande();
                surLAller = false;
            }
            else if (other.TryGetComponent(out Soni soni))
            {
                if(!surLAller)soni.AssimilerClone(this, (int)scoreViande, viandes.Count);
            }
        }

        private void AjouterViande()
        {
            scoreViande += 1 * (multScoreViande * viandes.Count);
            
            Transform viande = Instantiate(viandeBase.gameObject, accumulateurViande).transform;
            
            Vector3 decalageViande = viande.position;
            decalageViande.y += decalageVerticalViandes * viandes.Count;
            viande.position = decalageViande;
            viandes.Add(viande);
        }
    }
}