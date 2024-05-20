using UnityEngine;
using System.Net.Http;
using System.Threading.Tasks;
using PlayerInterface;

public class PlayerDataHandler: MonoBehaviour
{
    public static string mockResponseBody = @"
    {
      ""playerInfo"": {
        ""ign"": ""Player1"",
        ""playerId"": ""123456"",
        ""level"": 10,
        ""currentLevelExp"": 500,
        ""baseStats"": {
          ""encounterRate"": 0.75,
          ""catchRate"": 0.5
        }
      },
      ""inventory"": {
        ""resources"": {
          ""gold"": 1000,
          ""diamonds"": 50
        }
      },
      ""nextLevelRequiredExp"": 1000,
      ""mapLocation"": ""Town""
    }";

    private static readonly HttpClient client = new HttpClient();

    public static async Task<PlayerData> FetchPlayerData(string playerId)
    {


        // HttpResponseMessage response = await client.GetAsync($"https://your-api-endpoint.com/playerdata/{playerId}");
        // response.EnsureSuccessStatusCode();
        // string responseBody = await response.Content.ReadAsStringAsync();
        await Task.Delay(1000); // Simulate delay of 1 second
        string responseBody = mockResponseBody;
        return JsonUtility.FromJson<PlayerData>(responseBody);
    }
}

