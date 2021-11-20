using System;
using UnityEngine;

namespace Acteurs
{
    public class Scientifique : Acteur
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            deplacements.Direction = (Soni.soni.transform.position - transform.position).normalized;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            
            if (other.TryGetComponent(out Soni soni))
            {
                GameManager.Singleton.GameOver();
            }
        }
    }
}
