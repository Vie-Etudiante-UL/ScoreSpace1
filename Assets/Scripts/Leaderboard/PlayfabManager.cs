using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayfabManager : MonoBehaviour
{
    [Header("Fenêtres")] 
    public GameObject nameWindow;
    public GameObject leaderboardWindow;
    public GameObject loggingInWindow;
    public GameObject menuWindow;

    [Header("Leaderboard")]
    public GameObject rowPrefab;
    public Transform rowsParent;
    public Color colorSelf = Color.white;
    public int typeOfLeaderboard;

    [Header("Fenêtre de nom")] 
    public TMP_InputField nameInput;

    private string playerID;
    private List<LigneScore> listLigneScore = new List<LigneScore>();
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
        /*
        leaderboardWindow.SetActive(false);
        nameWindow.SetActive(false);
        loggingInWindow.SetActive(true);
        Login();
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
            
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSucess, OnError);
    }

    void OnLoginSucess(LoginResult result)
    {
        Debug.Log("Bien loggé / compte crée !");
        playerID = result.PlayFabId;
        string name = null;
        if (result.InfoResultPayload.PlayerProfile != null)
        { 
            name = result.InfoResultPayload.PlayerProfile.DisplayName;
        }
        
        loggingInWindow.SetActive(false);

        if (name == null)
        {
            nameWindow.SetActive(true);
        }
        else
        {
            menuWindow.SetActive(true);
            leaderboardWindow.SetActive(true);
            getLeaderboardByType(typeOfLeaderboard);
        }
    }

    public void getLeaderboardByType(int type)
    {
        if (type == 0)
            GetLeaderboard();
        if(type == 1)
            GetLeaderAroundPlayer();
            
    }
    
    void OnError(PlayFabError error)
    {
        Debug.Log("Erreur de login !");
        Debug.Log(error.GenerateErrorReport());
    }

    public void SumbitNameButton()
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = nameInput.text,
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);
    }

    void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("Nom mis à jour !");
        nameWindow.SetActive(false);
        leaderboardWindow.SetActive(true);
        menuWindow.SetActive(true);
            getLeaderboardByType(typeOfLeaderboard);
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

    public void OnRenameClick()
    {
        leaderboardWindow.SetActive(false);
        nameWindow.SetActive(true);
    }

    public void OnSkipNameClick()
    {
        nameWindow.SetActive(false);
        leaderboardWindow.SetActive(true);
        getLeaderboardByType(typeOfLeaderboard);

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

    public void GetLeaderAroundPlayer()
    {
        var request = new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = "Score",
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnLeaderboardAroundPlayerGet, OnError);
    }
    
    void OnLeaderboardAroundPlayerGet(GetLeaderboardAroundPlayerResult result)
    {
        CleanListScore();
        foreach (var item in result.Leaderboard)
        {
            if (Instantiate(rowPrefab, rowsParent).TryGetComponent(out LigneScore ligne))
            {
                ligne.position.text = (item.Position + 1).ToString();
                if (item.DisplayName != null)
                    ligne.nom.text = item.DisplayName;
                else
                    ligne.nom.text = "Anonymous";
                ligne.score.text = item.StatValue.ToString();

                if (item.PlayFabId == playerID)
                {
                    ligne.nom.color = colorSelf;
                    ligne.position.color = colorSelf;
                }
                
                listLigneScore.Add(ligne);
            }
        }
    }

    void OnLeaderboardGet(GetLeaderboardResult result)
    {
        CleanListScore();
        foreach (var item in result.Leaderboard)
        {
            if (Instantiate(rowPrefab, rowsParent).TryGetComponent(out LigneScore ligne))
            {
                ligne.position.text = (item.Position + 1).ToString();
                if (item.DisplayName != null)
                    ligne.nom.text = item.DisplayName;
                else
                    ligne.nom.text = "Anonymous ";
                ligne.score.text = item.StatValue.ToString();
                listLigneScore.Add(ligne);
                if (item.PlayFabId == playerID)
                {
                    ligne.nom.color = colorSelf;
                    ligne.position.color = colorSelf;
                }
                
            }
        }
    }

    void CleanListScore()
    {
        foreach (var item in listLigneScore)
        {
            Destroy(item.gameObject);
        }
        listLigneScore.Clear();
    }
}
