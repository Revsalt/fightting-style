using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Mirror;
using UnityEngine.UI;

public class Health : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnHealthChange))]public int health = 0;
    public Text healthDisplay;
    Vector3 damagePos;

    public UnityEvent OnTakeDamage;

    public void TakeDamage(int _health, Vector3 _damagePos)
    {
        RpcTakeDamage(GetComponent<NetworkIdentity>(), _damagePos);

        GetComponent<Health>().health += _health;
        GetComponent<Health>().damagePos = _damagePos;
    }

    [ClientRpc]
    public void RpcTakeDamage(NetworkIdentity ntd , Vector3 _damagePos)
    {
        if (ntd.isLocalPlayer)
        {
            ntd.GetComponent<PlayerMovement>().AddKockBack(200 , -(_damagePos - transform.position).normalized);
        }
    }

    public void OnHealthChange(int oldint , int newint)
    {
        if (healthDisplay != null)
            healthDisplay.text = newint.ToString();

        OnTakeDamage.Invoke();
    }

    public void Respawn()
    {
        transform.position = Vector3.zero;
        CmdResetHealth();
    }

    [Command]
    public void CmdResetHealth()
    {
        health = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Death")
            Respawn();
    }

}
