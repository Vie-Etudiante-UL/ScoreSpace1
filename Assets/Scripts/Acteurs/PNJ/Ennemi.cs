using UnityEngine;

namespace Acteurs.PNJ
{
    public class Ennemi : PNJ
    {
        private Vector2 derPosConnue = Vector2.zero;
        [SerializeField] protected float distanceAttaque = 0.1f;

        protected override void OnValidate()
        {
            base.OnValidate();
        }

        protected override void Update()
        {
            base.Update();
            if (cibleSoni)
            {
                Operer();
            }
        }
        
        /// <summary>
        /// Fonction a appeler toutes les frames pour faire se dérouler le mode opératoire
        /// </summary>
        private void Operer()
        {
            if (Vector2.Distance(transform.position, cibleSoni.transform.position) <= distanceAttaque)
            {
                Attaquer();
            }
            
            else if (Vector2.Distance(cibleSoni.transform.position, derPosConnue) > 0.1f)
            {
                TrouverCheminVersCible();
                derPosConnue = cibleSoni.transform.position;
            }
        }

        protected virtual void Attaquer()
        {
            
        }

        private void TrouverCheminVersCible()
        {
            
        }

        private void AtteindreCible(/*Chemin vers cible*/)
        {
            //On utise le déplacement pour se déplacer au prochain point du chemin
        }
    }
}