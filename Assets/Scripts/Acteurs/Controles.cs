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
        public struct Controle
        {
            public string nom;
            public List<KeyCode> controles;
        }

        [SerializeField] private List<Controle> controles;

        public bool ControleDetecte(string nomControle, DetectionControle typeDetection)
        {
            Controle controle = controles.Find(controle1 => controle1.nom == nomControle);
            return ControleDetecte(controle.controles, typeDetection);
        }
        
        public static bool ControleDetecte(IEnumerable<KeyCode> controles, DetectionControle typeDetection)
        {
            switch (typeDetection)
            {
                case DetectionControle.estAppuye : 
                    if (controles.Any(Input.GetKey)) return true;
                    break;
                case DetectionControle.quandEnfonce:
                    if (controles.Any(Input.GetKeyDown)) return true;
                    break;
                case DetectionControle.quandRelache:
                    if (controles.Any(Input.GetKeyUp)) return true;
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