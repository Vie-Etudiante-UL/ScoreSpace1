using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TexteFeedBack : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI texte;
    private static GameObject texteBFBase;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static bool GenererTexteFB(string texte, Vector3 position, float tmpsAffichage,  out TextMeshProUGUI tmp, params OptionsTexteFB[] options)
    {
        if (!texteBFBase) texteBFBase = Resources.Load("TexteFB") as GameObject;

        if (Instantiate(texteBFBase, position, new Quaternion(), Overlay.Singleton.transform)
            .TryGetComponent(out TexteFeedBack texteFB))
        {
            texteFB.LancerAffichage(tmpsAffichage,options);
            texteFB.texte.text = texte;
            tmp = texteFB.texte;
            return true;
        }

        tmp = null;
        return false;
    }

    private void LancerAffichage(float tmpsAffichage, params OptionsTexteFB[] options)
    {
        StartCoroutine(RoutineAffichage(tmpsAffichage, options));
    }
    
    private IEnumerator RoutineAffichage(float tmpsAffichage, params OptionsTexteFB[] options)
    {
        float tmps = 0;
        while (tmps <= tmpsAffichage)
        {
            yield return new WaitForEndOfFrame();
            tmps += Time.unscaledDeltaTime;

            #region Application Des Options
            
            Color nvlleColeur = texte.color;
            Vector3 posDepart = texte.transform.position;
            float tmpsNorm = tmps / tmpsAffichage;
            
            foreach (var option in options)
            {
                switch (option)
                {
                    case GradiantBinaire binaire:
                    {
                        float alpha = nvlleColeur.a;
                        nvlleColeur = Color.Lerp(binaire.couleurDepart, binaire.couleurArrive, tmpsNorm);
                        nvlleColeur.a = alpha;
                        break;
                    }
                    case Gradiant gradiant:
                    {
                        float alpha = nvlleColeur.a;
                        nvlleColeur = gradiant.gradiant.Evaluate(tmpsNorm);
                        nvlleColeur.a = alpha;
                        break;
                    }
                    case Fondu fondu:
                    {
                        if (tmpsNorm >= fondu.prcentDebutFondu && tmpsNorm <= fondu.prcentFinFondu)
                        {
                            float pas = (tmpsNorm - fondu.prcentDebutFondu) / 1 /
                                        (fondu.prcentFinFondu - fondu.prcentDebutFondu);
                            
                            nvlleColeur.a = fondu.arriveeFondu ? Mathf.Lerp(0, 1, pas) : Mathf.Lerp(1, 0, pas);    
                        }
                        break;
                    }
                    case Deplacement deplacement:
                    {
                        texte.transform.position = Vector3.Lerp(posDepart, posDepart + (Vector3)deplacement.decalage, tmpsNorm);
                        break;
                    }
                }
            }
            texte.color = nvlleColeur;

            #endregion
            
        }
        Destroy(gameObject);
    }

    #region Definition des options

    public abstract class OptionsTexteFB
    {
        
    }
    
    public class GradiantBinaire : OptionsTexteFB
    {
        public Color couleurDepart;
        public Color couleurArrive;

        public GradiantBinaire(Color _couleurDepart, Color _couleurArrive)
        {
            couleurDepart = _couleurDepart;
            couleurArrive = _couleurArrive;
        }
    }
    
    public class Gradiant : OptionsTexteFB
    {
        public Gradient gradiant;

        public Gradiant(Gradient _gradiant)
        {
            gradiant = _gradiant;
        }
    }


    public class Deplacement : OptionsTexteFB
    {
        public Vector2 decalage;

        public Deplacement(Vector2 _decalage)
        {
            decalage = _decalage;
        }
    }

    public class Fondu : OptionsTexteFB
    {
        public bool arriveeFondu;
        public float prcentDebutFondu; 
        public float prcentFinFondu; 

        public Fondu(float prcentDebutFondu = 0, float _prcentFinFondu = 1, bool _arriveeFondu = true)
        {
            arriveeFondu = _arriveeFondu;
            prcentDebutFondu = prcentDebutFondu;
            prcentFinFondu = _prcentFinFondu;
        }
    }

    #endregion
    
}
