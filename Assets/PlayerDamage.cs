using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerDamage : NetworkBehaviour
{
    [SerializeField]private LayerMask damagable;
    [SerializeField]private int damage;
    [SerializeField] private float colliderRadius;
    [SerializeField] private float yAxesOffsetCollider;
    public GameObject[] Limbs;

    [HideInInspector]public bool canDamage = false;

   
    void Update()
    {
        if (!isServer)
            return;

        if (!canDamage)
            return;

        foreach (var limb in Limbs)
        {
            Collider[] colliders = Physics.OverlapSphere(limb.transform.position - (limb.transform.up * yAxesOffsetCollider), colliderRadius , damagable);
            foreach (var item in colliders)
            {
                if (item.GetComponent<Health>() != GetComponent<Health>())
                {
                    Debug.Log("Damage");
                    item.GetComponent<Health>().TakeDamage(damage, transform.position);
                    canDamage = false;
                    return;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        foreach (var limb in Limbs)
        {
            Gizmos.DrawWireSphere(limb.transform.position - (limb.transform.up * yAxesOffsetCollider), colliderRadius);
        }
    }
}
