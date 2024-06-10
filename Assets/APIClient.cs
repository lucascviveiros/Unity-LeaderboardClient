using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class APIClient : MonoBehaviour
{
    private string baseUrl = "https://localhost:7074/api/Scores"; // Substitua pela sua URL correta

    void Start()
    {
        //Score newScore = new Score { PlayerName = "Player 3", Points = 150, Date = System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") };
        //StartCoroutine(AddScore(newScore));
        StartCoroutine(GetScores());
    }

    public IEnumerator GetScores()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(baseUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                string jsonResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
                List<Score> scores = JsonConvert.DeserializeObject<List<Score>>(jsonResult);
                foreach (var score in scores)
                {
                    //Debug.Log($"Player: {score.PlayerName}, Points: {score.Points}, Date: {score.Date}");
                    Debug.Log($"ID:{score.Id}, Player: {score.PlayerName}, Points: {score.Points}, Date: {score.Date}");
                }
            }
        }
    }

    public IEnumerator AddScore(Score newScore)
    {
        string jsonData = JsonConvert.SerializeObject(newScore);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        using (UnityWebRequest request = UnityWebRequest.PostWwwForm(baseUrl, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                Debug.Log("Score added successfully!");
            }
        }
    }

}

[System.Serializable]
public class Score
{
    public int Id;
    public string PlayerName;
    public int Points;
    public string Date;
}
