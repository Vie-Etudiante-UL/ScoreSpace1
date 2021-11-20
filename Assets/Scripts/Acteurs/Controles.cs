using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using UnityEngine;

namespace Acteurs
{
    public class Controles : MonoBehaviour
    {
        [Serializable]
        private struct Controle
        {
            public string nom;
            public List<KeyCode> controles;
            public List<int> indexSouris;
        }

        [SerializeField] private List<Controle> controles;

        public bool ControleDetecte(string nomControle, DetectionControle typeDetection)
        {
            Controle controle = controles.Find(controle1 => controle1.nom == nomControle);
            if (string.IsNullOrEmpty(controle.nom)) throw new ArgumentException(nomControle + "n'est pas un nom de controle existant");
            return ControleDetecte(controle, typeDetection);
        }
        
        private bool ControleDetecte(Controle controle, DetectionControle typeDetection)
        {
            switch (typeDetection)
            {
                case DetectionControle.estAppuye : 
                    if (controle.controles.Any(Input.GetKey) || controle.indexSouris.Any(Input.GetMouseButton)) return true;
                    break;
                case DetectionControle.quandEnfonce:
                    if (controle.controles.Any(Input.GetKeyDown) || controle.indexSouris.Any(Input.GetMouseButtonDown)) return true;
                    break;
                case DetectionControle.quandRelache:
                    if (controle.controles.Any(Input.GetKeyUp) || controle.indexSouris.Any(Input.GetMouseButtonUp)) return true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(typeDetection), typeDetection, null);
            }
            return false;
        }
    }
    public enum DetectionControle
    {
        quandEnfonce,
        quandRelache,
        estAppuye
    }
}