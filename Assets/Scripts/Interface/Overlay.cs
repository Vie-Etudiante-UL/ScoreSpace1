using System;
using System.Collections;
using System.Collections.Generic;
using Acteurs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Overlay : MonoBehaviour
{
    private static Overlay cela;

    public static Overlay Singleton
    {
        get
        {
            if (!cela) cela = FindObjectOfType<Overlay>();
            return cela;
        }
    }
    
    [Header("Capacité en Clones")]
    [SerializeField] private Transform capaciteClones;
    [SerializeField] private Image iconeCloneBase;
    [SerializeField] private Color teinteCloneTire;
    private int clonesTires;
    private readonly List<Image> listeIcones = new List<Image>();

    [Header("Slider de Level Up")] 
    [SerializeField] private Slider sliderLvlUp;
    [SerializeField] private Transform spawnScoreViande;
    [SerializeField] private Color couleurFBViande;
    [SerializeField] private Color couleurFBCombo;

    [Header("Score")] 
    [SerializeField] private TextMeshProUGUI texteScore;
    [SerializeField] private Transform spawnScore;

    private int score;

    private void Awake()
    {
        Clone.quandAjouteViande.AddListener(AfficherFBViande);
    }

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
        }

        if (Soni.soni.ClonesTires != clonesTires)
        {
            clonesTires = Soni.soni.ClonesTires;
            int cloneATeindre = clonesTires;
            
            for (int i = listeIcones.Count - 1; i >= 0; i--)
            {
                listeIcones[i].color = cloneATeindre > 0 ? teinteCloneTire : iconeCloneBase.color;
                cloneATeindre--;
            }
        }
    }

    private void MAJSlider()
    {
        sliderLvlUp.value = Mathf.Clamp01((float)Soni.soni.NbrViandes / Soni.soni.ViandesPourLvlUp);
    }

    private void MAJScore()
    {
        int difScore = GameManager.Score - score;
        if (difScore != 0)
        {
            string FBScore = difScore > 0 ? "+" + difScore : difScore.ToString();
            if (TexteFeedBack.GenererTexteFB(FBScore, spawnScore.position, 1f,
                out TextMeshProUGUI tMP,
                new TexteFeedBack.Fondu(0, 0.6f),
                new TexteFeedBack.Fondu(0.95f),
                new TexteFeedBack.Deplacement(Vector2.up * 200) ))
            {
                tMP.fontSize = 60;
                tMP.color = texteScore.color;
                tMP.alignment = TextAlignmentOptions.Right;
            }

            score = GameManager.Score;
        }
        texteScore.text =  score.ToString();
    }
    
    private void AfficherFBViande(int nbrViandes, int combo)
    {
        if(nbrViandes == 0) return;
        
        string texteFB = "+" + nbrViandes;
        texteFB += combo > 0 ? "<color=#" + ColorUtility.ToHtmlStringRGBA(couleurFBCombo) + ">\n+" +
                               combo + " COMBO!!!</color>" : "";
            
        if (TexteFeedBack.GenererTexteFB(texteFB, spawnScoreViande.position, 3f, 
            out TextMeshProUGUI tMP,
            new TexteFeedBack.Fondu(0.95f, _fadeIn:false),
            new TexteFeedBack.Fondu(_prcentFinFondu: 0.05f),
            new TexteFeedBack.Deplacement(Vector2.up * 60)))
        {
            tMP.color = couleurFBViande;
            tMP.fontStyle = FontStyles.Bold;
            tMP.fontSize = 38;
            tMP.alignment = TextAlignmentOptions.Center;
        }
    }
}
