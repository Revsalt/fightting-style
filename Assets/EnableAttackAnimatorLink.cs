using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableAttackAnimatorLink : MonoBehaviour
{
    public PlayerAnimation pm;

    public void EnableAttack()
    {
        pm.EnableAttack();
    }
}
