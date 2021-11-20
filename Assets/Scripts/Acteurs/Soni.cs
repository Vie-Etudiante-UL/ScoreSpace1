using System;
using System.Collections;
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
        private IEnumerator coolDownClonage;
        [SerializeField] private int viandesPourLvlUp;
        private int nbrViandes;
        
        public int CapaciteClone => capaciteClone;

        public int ClonesTires => clonesTires;

        public int ViandesPourLvlUp => viandesPourLvlUp;

        public int NbrViandes => nbrViandes;

        [Header("Inteface Lvl Up")] 
        [SerializeField] private Canvas interfaceLvlUp;

        [SerializeField] private Slider timer;
        [SerializeField] private float tmpsPourLvlUp;
        private IEnumerator coolDownlvlUp;

        protected override void OnValidate()
        {
            base.OnValidate();
            if (!controles) TryGetComponent(out controles);
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
                LancerCoolDownClonage(1f);
            }
        }

        public void AssimilerClone(Clone cloneAAssimiler, int scoreViande, int nbrViande)
        {
            Destroy(cloneAAssimiler.gameObject);
            clonesTires--;
            nbrViandes += nbrViande;
            if (nbrViandes >= viandesPourLvlUp)
            {
                nbrViandes = viandesPourLvlUp - nbrViandes;
                OuvrirInterfaceLvlUp(true);
            }
        }

        public void AjouterCapaciteClone()
        {
            capaciteClone++;
        }

        public void OuvrirInterfaceLvlUp(bool ouvrir)
        {
            if(coolDownlvlUp != null) StopCoroutine(coolDownlvlUp);
            if (ouvrir)
            {
                coolDownlvlUp = CoolDownLvlUp();
                StartCoroutine(coolDownlvlUp);
                peutCloner = false;
            }
            else LancerCoolDownClonage(0.2f);

            
            interfaceLvlUp.gameObject.SetActive(ouvrir);
            Time.timeScale = ouvrir ? 0 : 1;
            
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
        
        private IEnumerator CoolDownLvlUp()
        {
            float tmps = 0;

            while (tmps < tmpsPourLvlUp)
            {
                yield return new WaitForEndOfFrame();
                tmps += Time.unscaledDeltaTime;
                timer.value = Mathf.Lerp(0, 1, tmps / tmpsPourLvlUp);
            }
            OuvrirInterfaceLvlUp(false);

            coolDownlvlUp = null;
        }

        public void AjouterScore()
        {
            GameManager.AjouterScore();
        }
    }
}
