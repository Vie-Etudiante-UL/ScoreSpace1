using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeJeu : MonoBehaviour
{

    public GameObject scoreField;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void sendScore()
    {
        string score = scoreField.GetComponent<TMP_InputField>().text;
        Debug.Log(score);
        PlayfabManager.Singleton.SendLeaderboard(int.Parse(score));
    }

    public void seeLaderboard()
    {
        PlayfabManager.Singleton.GetLeaderboard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
