using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Acteurs
{
    public class Scientifique : Acteur
    {

        [SerializeField] private float rayonDetection;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, rayonDetection);
        }


        private bool estActif = true;
        public bool EstActif => estActif;
        
        [Header("Animations")] 
        [SerializeField] private SpriteRenderer sprRend;
        [SerializeField] private Aberation aberation;

        // Start is called before the first frame update
        void Start()
        {
         aberation.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            deplacements.Direction = (Soni.soni.transform.position - transform.position).normalized;
        }

        private void FixedUpdate()
        {
            if(estActif) DetecterAutres();
        }

        public UnityEvent SeFaireManger()
        {
            UnityEvent quandAberationEstGrande = new UnityEvent();
            estActif = false;
            deplacements.peutSeDeplacer = false;

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

        private void DetecterAutres()
        {
            Collider2D[] autres = Physics2D.OverlapCircleAll(transform.position, rayonDetection);
            
            foreach (Collider2D autre in autres)
            {
                if (autre.TryGetComponent(out Soni soni)) GameManager.Singleton.GameOver();
            }
        }
    }
}
