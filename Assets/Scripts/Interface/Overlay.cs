using System.Collections;
using System.Collections.Generic;
using Acteurs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Overlay : MonoBehaviour
{
    [Header("Capacité en Clones")]
    [SerializeField] private Transform capaciteClones;
    [SerializeField] private Image iconeCloneBase;
    [SerializeField] private Color teinteCloneTire;
    private int clonesTires;
    private List<Image> listeIcones = new List<Image>();

    [Header("Slider de Level Up")] 
    [SerializeField] private Slider sliderLvlUp;

    [Header("Score")] 
    [SerializeField] private TextMeshProUGUI texteScore;
        
    // Start is called before the first frame update
    void Start()
    {
                
    }

    // Update is called once per frame
    void Update()
    {
        MAJInteface();
    }

    private void MAJInteface()
    {
        MAJIconesClone();
        MAJSlider();
        MAJScore();
    }


    private void MAJIconesClone()
    {
        if (Soni.soni.CapaciteClone > listeIcones.Count)
        {
            for (int i = 0; i < Soni.soni.CapaciteClone - listeIcones.Count; i++)
            {
                if (Instantiate(iconeCloneBase.gameObject, capaciteClones).TryGetComponent(out Image nvlleIcone))
                {
                    listeIcones.Add(nvlleIcone);
                    nvlleIcone.gameObject.SetActive(true);
                }
            }

            if (Soni.soni.ClonesTires != clonesTires)
            {
                for (int i = 0; i < listeIcones.Count; i++)
                {
                    listeIcones[i].color = i > clonesTires ? teinteCloneTire : iconeCloneBase.color;
                }

                clonesTires = Soni.soni.ClonesTires;
            }
        }
    }

    private void MAJSlider()
    {
        sliderLvlUp.value = Mathf.Clamp01((float)Soni.soni.NbrViandes / Soni.soni.ViandesPourLvlUp);
    }

    private void MAJScore()
    {
        texteScore.text =  GameManager.Score.ToString();
    }
}
