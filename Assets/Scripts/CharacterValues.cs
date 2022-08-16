using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CharacterValues : MonoBehaviour
{
    public Transform Head, LeftArm, RightArm, LeftLeg, RightLeg, Body;

    public KeyFrame key;
    [Header("InGame")]
    public Transform rigidbodyTarget;
    public Rigidbody rb = null;
    public float rigidbodyGrabMultiplier = 0;

    float lerpDuration = 1;

    [HideInInspector]public Vector2 bodyOldPos = Vector2.zero;
    [HideInInspector]public float bodyOldPosDirection = 1;

    public void PlayKeyFrame(KeyFrame _key , bool isSnap , float _lerpDuration)
    {
        lerpDuration = _lerpDuration;

        if (isSnap)
        {
            key = null;

            Head.localPosition = new Vector3(_key.positions[0].x, _key.positions[0].y, 0);
            RightArm.localPosition = new Vector3(_key.positions[1].x, _key.positions[1].y, 0);
            LeftArm.localPosition = new Vector3(_key.positions[2].x, _key.positions[2].y, 0);
            RightLeg.localPosition = new Vector3(_key.positions[3].x, _key.positions[3].y, 0);
            LeftLeg.localPosition = new Vector3(_key.positions[4].x, _key.positions[4].y, 0);
            Body.localPosition = new Vector3(_key.positions[5].x, _key.positions[5].y, 0);

            Head.localEulerAngles = new Vector3(0, 0, _key.rotaitons[0]);
            LeftArm.localEulerAngles = new Vector3(0, 0, _key.rotaitons[1]);
            RightArm.localEulerAngles = new Vector3(0, 0, _key.rotaitons[2]);
            LeftLeg.localEulerAngles = new Vector3(0, 0, _key.rotaitons[3]);
            RightLeg.localEulerAngles = new Vector3(0, 0, _key.rotaitons[4]);
            Body.localEulerAngles = new Vector3(0, 0, _key.rotaitons[5]);
        }
        else
        {
            key = _key;
            StartCoroutine(LerpCoroutinePos(Head , Head.localPosition, new Vector3(key.positions[0].x, key.positions[0].y, 0), lerpDuration / 10));
            StartCoroutine(LerpCoroutinePos(RightArm, RightArm.localPosition, new Vector3(key.positions[1].x, key.positions[1].y, 0), lerpDuration / 10));
            StartCoroutine(LerpCoroutinePos(LeftArm, LeftArm.localPosition, new Vector3(key.positions[2].x, key.positions[2].y, 0), lerpDuration / 10));
            StartCoroutine(LerpCoroutinePos(RightLeg, RightLeg.localPosition, new Vector3(key.positions[3].x, key.positions[3].y, 0), lerpDuration / 10));
            StartCoroutine(LerpCoroutinePos(LeftLeg, LeftLeg.localPosition, new Vector3(key.positions[4].x, key.positions[4].y, 0), lerpDuration / 10));

            if (GetComponent<PlayerAnimation>() == null)
                StartCoroutine(LerpCoroutinePos(Body, Body.localPosition, new Vector3(key.positions[5].x, key.positions[5].y, 0), lerpDuration / 10));
            else
                rigidbodyTarget.position = bodyOldPos + (new Vector2(key.positions[5].x * Mathf.RoundToInt(bodyOldPosDirection), key.positions[5].y - 0.35f));


            StartCoroutine(LerpCoroutineRot(Head, Head.localRotation, Quaternion.Euler(0, 0, key.rotaitons[0]), lerpDuration / 10));
            StartCoroutine(LerpCoroutineRot(LeftArm, LeftArm.localRotation, Quaternion.Euler(0, 0, key.rotaitons[1]), lerpDuration / 10));
            StartCoroutine(LerpCoroutineRot(RightArm, RightArm.localRotation, Quaternion.Euler(0, 0, key.rotaitons[2]), lerpDuration / 10));
            StartCoroutine(LerpCoroutineRot(LeftLeg, LeftLeg.localRotation, Quaternion.Euler(0, 0, key.rotaitons[3]), lerpDuration / 10));
            StartCoroutine(LerpCoroutineRot(RightLeg, RightLeg.localRotation, Quaternion.Euler(0, 0, key.rotaitons[4]), lerpDuration / 10));
            StartCoroutine(LerpCoroutineRot(Body, Body.localRotation, Quaternion.Euler(0, 0, key.rotaitons[5]), lerpDuration / 10));
        }
    }

    private IEnumerator LerpCoroutinePos(Transform target , Vector3 pos1, Vector3 pos2, float duration)
    {
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            target.localPosition = Vector3.Lerp(pos1, pos2, t / duration);
            yield return 0;
        }
        target.localPosition = pos2;
    }

    private IEnumerator LerpCoroutineRot(Transform target, Quaternion pos1, Quaternion pos2, float duration)
    {
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            target.localRotation = Quaternion.Lerp(pos1, pos2, t / duration);
            yield return 0;
        }
        target.localRotation = pos2;
    }

    public void GrabRigidBodyTo()
    {
        rigidbodyTarget.SetParent(null);
        Vector3 direction = new Vector3(rigidbodyTarget.position.x, rigidbodyTarget.position.y , rigidbodyTarget.position.z) - rb.position;
        rb.velocity = (direction.normalized * (rigidbodyGrabMultiplier * Vector3.Distance(new Vector2(rigidbodyTarget.position.x, rigidbodyTarget.position.y) , rb.position)) * Time.deltaTime);
        // need to switch to addforce
    }
}
