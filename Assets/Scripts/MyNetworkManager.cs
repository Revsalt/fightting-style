using System;
using UnityEngine;
using Mirror;

public class MyNetworkManager : NetworkManager
{
    // Set by UI element UsernameInput OnValueChanged
    public string PlayerName { get; set; }
    public string PlayerCharacterCharacteristic { get; set; }
    public string address { get; set; }

    public struct CreatePlayerMessage : NetworkMessage
    {
        public string name;
        public string json;
    }

    public void UpdateConnectionDetails()
    {
        networkAddress = address;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        NetworkServer.RegisterHandler<CreatePlayerMessage>(OnCreatePlayer);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        // tell the server to create a player with this name
        conn.Send(new CreatePlayerMessage { name = PlayerName, json = CharacterCharacteristic.GetCharacterCharacteristicToJson(MyPlayerInitializer.Instance.myCharacterCharacteristic)});
    }

    void OnCreatePlayer(NetworkConnection connection, CreatePlayerMessage createPlayerMessage)
    {
        // create a gameobject using the name supplied by client
        GameObject playergo = Instantiate(playerPrefab);
        playergo.GetComponent<NetworkPlayerManager>().playerName = createPlayerMessage.name;

        playergo.GetComponent<NetworkPlayerManager>().playerJson = createPlayerMessage.json;
        playergo.GetComponent<NetworkPlayerManager>().SetUpCharacterCharacteristic("" , createPlayerMessage.json);

        // set it as the player
        NetworkServer.AddPlayerForConnection(connection, playergo);
    }
}
