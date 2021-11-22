using System;
using System.Collections.Generic;
using PlayFab.EconomyModels;
using UnityEngine;

namespace Acteurs
{
    [RequireComponent(typeof(Animator), typeof(DeplacementsTopDown))]
    public class AnimationsTopDown : MonoBehaviour
    {
        [SerializeField] private DeplacementsTopDown deplacements;
        [SerializeField] private Animator animator;
        [SerializeField] private List<SpriteRenderer> spritesAFlip;
        [SerializeField] private bool inverserFlip;
        [SerializeField] private int modeDeplacement;

        public int ModeDeplacement
        {
            get => modeDeplacement;
            set
            {
                if (value < 0) value = 0;
                if (value != modeDeplacement) ChangerModeDeplacement(value);
                modeDeplacement = value;
            }
        }

        private enum Direction
        {
            haut,
            bas,
            gauche,
            droite
        }

        private Direction direction = Direction.bas;
        [Header("Paramètre d'animator")]
        [SerializeField] private string paramAvancer = "Avancer";
        [SerializeField] private string paramHorizontal = "Horizontal";
        [SerializeField] private string paramHaut = "Haut";
        [SerializeField] private string paramBas = "Bas";
        
        private void OnValidate()
        {
            if (!deplacements) TryGetComponent(out deplacements);
            if (!animator) TryGetComponent(out animator);
            if (modeDeplacement < 0) modeDeplacement = 0;
        }

        private void Awake()
        {
            
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            DetecterDirection();
            AplliquerAnim();
        }

        private void DetecterDirection()
        {
            if(deplacements.Direction.magnitude == 0) return;
            if(Mathf.Abs(deplacements.Direction.x) > Mathf.Abs(deplacements.Direction.y))
            {
                direction = deplacements.Direction.x > 0 ? Direction.droite : Direction.gauche;
            }
            else
            {
                direction = deplacements.Direction.y > 0 ? Direction.haut : Direction.bas;
            }
        }

        private void AplliquerAnim()
        {
            foreach (var sprRend in spritesAFlip)
            {
                if (direction != Direction.droite && direction != Direction.gauche)
                {
                    sprRend.flipX = false;
                    continue;
                }
                
                sprRend.flipX = direction == Direction.gauche;
                sprRend.flipX = inverserFlip ? !sprRend.flipX : sprRend.flipX;
            }
            
            animator.SetBool(paramAvancer,deplacements.Direction.magnitude != 0);

            switch (direction)
            {
                case Direction.haut:
                    animator.SetBool(paramHaut, true);
                    animator.SetBool(paramBas, false);
                    animator.SetBool(paramHorizontal, false);
                    break;
                case Direction.bas:
                    animator.SetBool(paramHaut, false);
                    animator.SetBool(paramBas, true);
                    animator.SetBool(paramHorizontal, false);
                    break;
                case Direction.gauche:
                    case Direction.droite:
                    animator.SetBool(paramHaut, false);
                    animator.SetBool(paramBas, false);
                    animator.SetBool(paramHorizontal, true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ChangerModeDeplacement(int mode)
        {
            animator.Play("Mode"+mode);
        }
    }
}
