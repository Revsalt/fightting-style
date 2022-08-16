using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkPlayerManager : NetworkBehaviour
{
    [SyncVar] public string playerName;
    [SyncVar(hook = nameof(SetUpCharacterCharacteristic))] public string playerJson;

    public MonoBehaviour[] monoBehaviours;
    public Camera myCamera;

    private void Start()
    {
        if (!isLocalPlayer)
        {
            foreach (var item in monoBehaviours)
            {
                item.enabled = false;
            }
        }

        if (isLocalPlayer)
        {
            myCamera.gameObject.SetActive(true);
            GetComponent<PlayerAnimation>().myCharacterCharacteristic = MyPlayerInitializer.Instance.myCharacterCharacteristic;
            GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    public void SetUpCharacterCharacteristic(string oldjson, string newjson)
    {
        GetComponent<PlayerAnimation>().myCharacterCharacteristic = CharacterCharacteristic.GetCharacterCharacteristicFromJson(newjson);
    }

    [Command]
    public void CmdSendCustomAnimationInput(int i , NetworkIdentity networkIdentity)
    {
        RpcSendCustomAnimationInput(i , networkIdentity);

        GetComponent<PlayerDamage>().canDamage = true;
        foreach (var item in FindObjectsOfType<NetworkPlayerManager>())
        {
            if (item.GetComponent<NetworkIdentity>() == networkIdentity)
                item.GetComponent<PlayerAnimation>().PlayAnimation(item.GetComponent<PlayerAnimation>().myCharacterCharacteristic.animations[i]);
        }
    }

    [Command]
    public void CmdSendAttack(NetworkIdentity networkIdentity)
    {
        GetComponent<PlayerDamage>().canDamage = true;
    }

    [ClientRpc]
    public void RpcSendCustomAnimationInput(int i , NetworkIdentity networkIdentity)
    {
        foreach (var item in FindObjectsOfType<NetworkPlayerManager>())
        {
            if (item.GetComponent<NetworkIdentity>() == networkIdentity)
                item.GetComponent<PlayerAnimation>().PlayAnimation(item.GetComponent<PlayerAnimation>().myCharacterCharacteristic.animations[i]);
        }
    }
}
