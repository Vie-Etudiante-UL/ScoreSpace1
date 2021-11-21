using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;

namespace Acteurs
{
    public class Clone : Acteur
    {
        [SerializeField] private float distanceCourseMax;
        [SerializeField] private float distanceParcourue;
        private Vector3 positionDepart;
        [SerializeField] private float rayonDetection;
        [Header("Viande")] 
        [SerializeField] private Transform accumulateurViande;
        [SerializeField] private GameObject viandeBase;
        private List<Transform> viandes = new List<Transform>();
        private float scoreViande;
        [SerializeField] private float multScoreViande = 1;
        [SerializeField] private float decalageVerticalViandes;

        [Header("Animations")] 
        [SerializeField] private SpriteRenderer sprRend;
        [SerializeField] private Aberation aberation;

        private bool estActif;


        private bool surLAller = true;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(positionDepart, distanceCourseMax);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, rayonDetection);
        }

        private void Start()
        {
            positionDepart = transform.position;
        }

        private void Update()
        {
            GererAllerRetour();
        }

        private void FixedUpdate()
        {
            if(estActif)DetecterAutres();
        }

        public UnityEvent Init(Vector2 direciton)
        {
            UnityEvent quandAberationEstGrande = new UnityEvent();
            deplacements.Direction = direciton;
            sprRend.gameObject.SetActive(false);
            estActif = false;
            deplacements.peutSeDeplacer = false;
            
            UnityEvent[] eventsAberation = aberation.LancerAnim();
            eventsAberation[0].AddListener(() =>
            {
                estActif = true;
                deplacements.peutSeDeplacer = true;
                sprRend.gameObject.SetActive(true);
                quandAberationEstGrande.Invoke();
            });

            return quandAberationEstGrande;
        }

        public UnityEvent EtreAssimiler()
        {
            UnityEvent quandAberationEstGrande = new UnityEvent();
            
            estActif = false;
            deplacements.peutSeDeplacer = false;
            accumulateurViande.gameObject.SetActive(false);
            
            UnityEvent[] eventsAberation = aberation.LancerAnim();
            eventsAberation[0].AddListener(() =>
            {
                sprRend.gameObject.SetActive(false);
                quandAberationEstGrande.Invoke();
            });
            eventsAberation[1].AddListener(() =>
            {
                Destroy(gameObject);
            });
            return quandAberationEstGrande;
        }
        
        private void GererAllerRetour()
        {
            if (surLAller)
            {
                distanceParcourue = Vector2.Distance(transform.position, positionDepart);
                if (distanceParcourue >= distanceCourseMax) surLAller = false;
            }
            else deplacements.Direction = (Soni.soni.transform.position - transform.position).normalized;
        }
        
        
        private void AjouterViande()
        {
            scoreViande += 1 + multScoreViande * viandes.Count;
            
            Transform viande = Instantiate(viandeBase.gameObject, accumulateurViande).transform;
            
            Vector3 decalageViande = viande.position;
            decalageViande.y += decalageVerticalViandes * viandes.Count;
            viande.position = decalageViande;
            viandes.Add(viande);
        }

        private void DetecterAutres()
        {
            Collider2D[] autres = Physics2D.OverlapCircleAll(transform.position, rayonDetection);

            foreach (Collider2D autre in autres)
            {
                if (autre.TryGetComponent(out Scientifique scienti) && scienti.EstActif)
                {
                    AttaquerScientifique(scienti);
                }
                else if (autre.TryGetComponent(out Soni soni))
                {
                    if(!surLAller)soni.AssimilerClone(this, (int)scoreViande);
                }
            }
        }

        private void AttaquerScientifique(Scientifique scienti)
        {
            estActif = false;
            scienti.SeFaireManger().AddListener(() =>
            {
                AjouterViande();
            });
            
            estActif = false;
            deplacements.peutSeDeplacer = false;
            
            UnityEvent[] eventsAberation = aberation.LancerAnim();
            eventsAberation[0].AddListener(() =>
            {
                estActif = true;
                deplacements.peutSeDeplacer = true;
                surLAller = false;
                print(deplacements.peutSeDeplacer);    
            });
        }
    }
}