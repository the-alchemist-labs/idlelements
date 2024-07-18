using System;
using System.Threading.Tasks;

[Serializable]
public class GetPlayerResponse
{
    public PlayerInfo player;
}

public static class PlayerApi
{
    public async static Task<PlayerInfo> GetPlayer(string playerId, bool shouldRegister)
    {
        GetPlayerResponse res = await Http.Get<GetPlayerResponse>($"{Consts.ServerURI}/players/{playerId}?shouldRegister={shouldRegister}");
        return res?.player;
    }
}
