using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using TMPro;
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
        [SerializeField] private Animator aberationChaireBase;
        [SerializeField] private Clone cloneBase;
        public bool peutCloner;
        [SerializeField] private int capaciteClone = 1;
        private int clonesTires;
        private IEnumerator coolDownClonage;
        [SerializeField] private int viandesPourLvlUp;
        [SerializeField] private float rayonSpawnClone;
        private int nbrViandes;
        
        public int CapaciteClone => capaciteClone;

        public int ClonesTires => clonesTires;

        public int ViandesPourLvlUp => viandesPourLvlUp;

        public int NbrViandes => nbrViandes;

        [Header("Inteface Lvl Up")] 
        [SerializeField] private Canvas interfaceLvlUp;

        [SerializeField] private Slider timer;
        [SerializeField] private float chairesPourLvlUp;
        private IEnumerator coolDownlvlUp;

        protected override void OnValidate()
        {
            base.OnValidate();
            if (!controles) TryGetComponent(out controles);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, rayonSpawnClone);
        }

        private void Awake()
        {
            OuvrirInterfaceLvlUp(false);
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
            
            VerifLevelUp();
        }
        
        
        #region Clones

        private void ClonerTirer()
        {
            if (!peutCloner || capaciteClone <= clonesTires) return;
            if (Instantiate(cloneBase.gameObject, transform.position, new Quaternion())
                .TryGetComponent(out Clone nvClone))
            {
                peutCloner = false;
                clonesTires++;
                Vector2 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position)
                    .normalized;
                Vector3 nvllePosition = nvClone.transform.position + (Vector3)direction * rayonSpawnClone;
                nvClone.transform.position = nvllePosition;
                
                nvClone.Init(direction).AddListener(() =>
                {
                    peutCloner = true;
                });
            }
        }

        public void AssimilerClone(Clone cloneAAssimiler, int nbrViande)
        {
            cloneAAssimiler.EtreAssimiler().AddListener(() =>
            {
                clonesTires--;
                nbrViandes += nbrViande;
            });
        }

        public void AjouterCapaciteClone()
        {
            capaciteClone++;
        }
        
        private void LancerCoolDownClonage(float tmps)
        {
            if(coolDownClonage != null) StopCoroutine(coolDownClonage);
            coolDownClonage = CoolDownClonage(tmps);
            StartCoroutine(coolDownClonage);
        }
        
        private IEnumerator CoolDownClonage(float tmps)
        {
            yield return new WaitForSeconds(tmps);
            peutCloner = true;
            coolDownClonage = null;
        }

        #endregion


        #region Level Up

        public void OuvrirInterfaceLvlUp(bool ouvrir)
        {
            if(coolDownlvlUp != null) StopCoroutine(coolDownlvlUp);
            if (ouvrir)
            {
                coolDownlvlUp = CoolDownLvlUp();
                StartCoroutine(coolDownlvlUp);
                peutCloner = false;
            }
            else LancerCoolDownClonage(0.01f);

            interfaceLvlUp.gameObject.SetActive(ouvrir);
            Time.timeScale = ouvrir ? 0 : 1;
        }

        

        private void VerifLevelUp()
        {
            if (nbrViandes >= viandesPourLvlUp && !interfaceLvlUp.gameObject.activeSelf)
            {
                nbrViandes -= viandesPourLvlUp;
                OuvrirInterfaceLvlUp(true);
            }
        }
        
        private IEnumerator CoolDownLvlUp()
        {
            float tmps = 0;

            while (tmps < chairesPourLvlUp)
            {
                yield return new WaitForEndOfFrame();
                tmps += Time.unscaledDeltaTime;
                timer.value = Mathf.Lerp(0, 1, tmps / chairesPourLvlUp);
            }
            OuvrirInterfaceLvlUp(false);

            coolDownlvlUp = null;
        }

        public void AjouterScore()
        {
            GameManager.AjouterScore();
        }
        
        #endregion
    }
}
