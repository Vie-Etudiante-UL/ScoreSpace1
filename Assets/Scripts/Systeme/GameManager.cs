using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static int scoreParViande = 100;
    private static int score;
    public static int Score => score;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void AjouterScore()
    {
        score += scoreParViande;
    }
}
