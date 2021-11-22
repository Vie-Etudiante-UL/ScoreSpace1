using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject fenetreConnexion,fenetreNom, fenetrePrincipale, fenetreOption, fenetreCredits, fenetreLeaderboard;

    // Start is called before the first frame update
    void Start()
    {
        
        fenetreLeaderboard.SetActive(false);
        fenetreNom.SetActive(false);
        fenetreConnexion.SetActive(true);
        fenetrePrincipale.SetActive(false);
        fenetreOption.SetActive(false);
        fenetreCredits.SetActive(false);
        PlayfabManager.Singleton.Login();

    }

    public void clicQuitter()
    {
        Application.Quit();
    }

    public void versScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex,LoadSceneMode.Single);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
