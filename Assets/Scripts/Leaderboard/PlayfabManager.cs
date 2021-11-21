using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;

public class PlayfabManager : MonoBehaviour
{
    public GameObject rowPrefab;
    public Transform rowsParent;
    
    private static PlayfabManager cela;

    public static PlayfabManager Singleton
    {
        get
        {
            if (!cela)
                cela = FindObjectOfType<PlayfabManager>();

            return cela;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Login();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSucess, OnError);
    }

    void OnSucess(LoginResult result)
    {
        Debug.Log("Loggé !");
    }
    
    void OnError(PlayFabError error)
    {
        Debug.Log("Erreur de login !");
        Debug.Log(error.GenerateErrorReport());
    }

    public void SendLeaderboard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "Score",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Envoi vers le leaderboard OK");
    }

    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "Score",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }

    void OnLeaderboardGet(GetLeaderboardResult result)
    {
        foreach (var item in result.Leaderboard)
        {
            GameObject newGo = Instantiate(rowPrefab, rowsParent);
            Text[] texts = newGo.GetComponentsInChildren<Text>();
            texts[0].text = item.Position.ToString();
            texts[1].text = item.PlayFabId;
            texts[1].text = item.StatValue.ToString();
            
            Debug.Log(item.Position + " " + item.PlayFabId + " " + item.StatValue);
        }
    }
}
