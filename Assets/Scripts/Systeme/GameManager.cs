using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Permissions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager cela;

    public static GameManager Singleton
    {
        get
        {
            if (!cela) cela = FindObjectOfType<GameManager>();
            return cela;
        }
    }
    
    private static int scoreParViande = 100;
    private static int score;
    public static int Score => score;

    
    [Header("Délai spawn")]
    [SerializeField] private float secondesEntreSpawn = 5;
    [SerializeField] private float facteurDescente = 0.003f;
    [SerializeField] private float palierDescente = 0.3f;
    [SerializeField] private float minimumDelai = 0.2f;
    private float mmaximumDelai;
    [SerializeField] private float palier = 30;
    private int secondesDepuisDebut = 0;
    
    [Header("Interface")] 
    [SerializeField] private GameObject menuGameOver;
    [SerializeField] private GameObject menuLeaderboard;
    [SerializeField] private GameObject interfaceJeu;
    private bool isPaused = false;

    [SerializeField] private TextMeshProUGUI texteScore;
    [SerializeField] private Slider timer;
    [SerializeField] private float tmpsPourQuitter = 3f;
    private IEnumerator coolDownGameOver;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    private void Start()
    {
        mmaximumDelai = secondesEntreSpawn;
        interfaceJeu.SetActive(true);
        menuGameOver.SetActive(false);
        if(menuLeaderboard)menuLeaderboard.SetActive(false);
        LoopFacteurSpawn();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void AjouterScore()
    {
        score += scoreParViande;
    }

    
    private IEnumerator LoopFacteurSpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            CalculeDelaiSpawn();
        }
    }
    
    private void CalculeDelaiSpawn()
    {
        secondesDepuisDebut++;
        
        if (secondesDepuisDebut % palier == 0)
        {
            secondesEntreSpawn -= palierDescente;
        }
        
        secondesEntreSpawn -= secondesEntreSpawn * secondesEntreSpawn * facteurDescente;
        secondesEntreSpawn = Mathf.Clamp(secondesEntreSpawn, minimumDelai, secondesDepuisDebut);
        SpawnerScientifique.Singleton.multTempsSpawn = secondesEntreSpawn;
    }
    
    public void GameOver()
    {
        Time.timeScale = 0;
        interfaceJeu.SetActive(false);
        menuGameOver.SetActive(true);
        texteScore.text = "Your score: " + score;

        if (PlayfabManager.Singleton.isLogged())
        {
            if(menuLeaderboard)menuLeaderboard.SetActive(true);
            PlayfabManager.Singleton.SendLeaderboard(score);
            PlayfabManager.Singleton.getLeaderboardByType(1);
        }
        if(coolDownGameOver != null) StopCoroutine(coolDownGameOver);
        coolDownGameOver = CoolDownGameOver();
        StartCoroutine(coolDownGameOver);
    }

    private IEnumerator CoolDownGameOver()
    {
        float tmps = 0;

        while (tmps < tmpsPourQuitter)
        {
            yield return new WaitForEndOfFrame();
            tmps += Time.unscaledDeltaTime;
            timer.value = Mathf.Lerp(0, 1, tmps / tmpsPourQuitter);
        }
        coolDownGameOver = null;
        
        Relancer();
    }
    
    
    public void Relancer()
    {
        
        score = 0;
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
        interfaceJeu.SetActive(false);

    }

    public void GoMainMenu()
    {
        Destroy(GameObject.FindGameObjectWithTag("Musique"));
        score = 0;
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    
    public void QuitterJeu()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBGL
        Application.OpenURL("about:blank");
#else
        Application.Quit();
#endif
    }
}
