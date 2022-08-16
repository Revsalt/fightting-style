using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerAnimation : NetworkBehaviour
{
    [HideInInspector]public CharacterCharacteristic myCharacterCharacteristic;

    [HideInInspector]public bool canAttack = true;

    #region
    //FOR DEBUGGING
    /* 
    public void PlayAnimation()
    {
        KeyFrame[] keys = new KeyFrame[2];
        keys[0] = new KeyFrame { 
            positions = new Vector2[6] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) },
            rotaitons = new float[6] { 0, 0, 0, 0, 0, 0 },
            KeyFrameNumber = 0
        };
        keys[1] = new KeyFrame
        {
            positions = new Vector2[6] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) },
            rotaitons = new float[6] { 50, 20, 30, 10, 20, 20 },
            KeyFrameNumber = 3
        };

        StopAllCoroutines();
        StartCoroutine(Play(new Animation { animationFrames = keys }));
    }*/
    #endregion

    bool canRigidbodyGrab = false;
    private void FixedUpdate()
    {
        if (canRigidbodyGrab)
            GetComponent<CharacterValues>().GrabRigidBodyTo();
    }

    private void Update()
    {
        if (!isLocalPlayer || !canAttack)
            return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            GetComponent<NetworkPlayerManager>().CmdSendCustomAnimationInput(0, GetComponent<NetworkIdentity>());
            canAttack = false;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            GetComponent<NetworkPlayerManager>().CmdSendCustomAnimationInput(1, GetComponent<NetworkIdentity>());
            canAttack = false;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            GetComponent<NetworkPlayerManager>().CmdSendCustomAnimationInput(2, GetComponent<NetworkIdentity>());
            canAttack = false;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Vector2 direc = new Vector2(Mathf.Round(Input.GetAxisRaw("Horizontal")), Mathf.Round(Input.GetAxisRaw("Vertical"))).normalized;

            Debug.Log(direc);

            if ((direc.x != 0 && direc.y == 0) || (direc.x != 0 && direc.y != 0))
            {
                GetComponent<NetworkPlayerManager>().CmdSendAttack(GetComponent<NetworkIdentity>());
                GetComponent<PlayerMovement>().anim.SetTrigger("isAttackSide");
            }
            else
            if (direc.x == 0 && direc.y < 0)
            {
                GetComponent<NetworkPlayerManager>().CmdSendAttack(GetComponent<NetworkIdentity>());
                GetComponent<PlayerMovement>().anim.SetTrigger("isAttackDown");
            }
            else
            if (direc.x == 0 && direc.y > 0)
            {
                GetComponent<NetworkPlayerManager>().CmdSendAttack(GetComponent<NetworkIdentity>());
                GetComponent<PlayerMovement>().anim.SetTrigger("isAttackUp");
            }
            else
            {
                GetComponent<NetworkPlayerManager>().CmdSendAttack(GetComponent<NetworkIdentity>());
                GetComponent<PlayerMovement>().anim.SetTrigger("isAttackSide");
            }

            canAttack = false;
        }
    }

    public void PlayAnimation(Animation animation)
    {
        GetComponent<PlayerMovement>().anim.enabled = false;

        if (!isServerOnly)
            canRigidbodyGrab = true;

        GetComponent<CharacterValues>().bodyOldPos = GetComponent<CharacterValues>().Body.transform.position;
        GetComponent<CharacterValues>().bodyOldPosDirection = GetComponent<PlayerMovement>().anim.transform.localScale.x;

        StopAllCoroutines();
        StartCoroutine(Play(animation));
    }

    IEnumerator Play(Animation animation)
    {
        List<KeyFrame> keyedFrames = animation.animationFrames.ToList();
        List<int> durationBetweenFrames = new List<int>();

        if (keyedFrames.Count == 0)
            yield return null;

        for (int i = 0; i < keyedFrames.Count - 1; i++)
        {
            if (i == 0)
                GetComponent<CharacterValues>().PlayKeyFrame(keyedFrames[i], true, 0);

            if (i + 1 < keyedFrames.Count)
                durationBetweenFrames.Add(keyedFrames[i + 1].KeyFrameNumber - keyedFrames[i].KeyFrameNumber);
        }

        Debug.Log(keyedFrames.Count);
        foreach (var item in keyedFrames)
        {
            int keyindex = keyedFrames.IndexOf(item);

            if (keyindex + 1 <= keyedFrames.Count - 1)
            {
                GetComponent<CharacterValues>().PlayKeyFrame(keyedFrames[keyindex + 1], false, durationBetweenFrames[keyindex]);
                yield return new WaitForSeconds(durationBetweenFrames[keyindex] * .1f);
            }
        }

        GetComponent<PlayerMovement>().anim.enabled = true;
        if (!isServerOnly)
            canRigidbodyGrab = false;

        EnableAttack();
    }

    public void EnableAttack()
    {
        canAttack = true;
        GetComponent<PlayerDamage>().canDamage = false;
    }
}
