using UnityEngine;
using UnityEngine.Events;

namespace Environnement
{
    public class Bloqueur : MonoBehaviour
    {
        private bool estBloqued;
        private int nbrClonePrDebloquer = 1;
        private UnityEvent quandDebloque = new UnityEvent();

        public bool EstBloqued => estBloqued;

        public int NbrClonePrDebloquer => nbrClonePrDebloquer;

        public UnityEvent QuandDebloque => quandDebloque;
    
    
    }
}
