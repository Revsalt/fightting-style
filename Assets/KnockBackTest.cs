using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackTest : MonoBehaviour
{
    public PhysicMaterial[] physicMaterials;
    public float knockbackStrength = 800;
    public float Health = 100;
    public float KnockBackDelay = .1f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
            AddKockBack(knockbackStrength, new Vector3(0,1,0));

        if (Input.GetKeyDown(KeyCode.S))
            AddKockBack(knockbackStrength, new Vector3(0, -1, 0));

        if (Input.GetKeyDown(KeyCode.D))
            AddKockBack(knockbackStrength, new Vector3(1, 0, 0));

        if (Input.GetKeyDown(KeyCode.A))
            AddKockBack(knockbackStrength, new Vector3(-1, 0, 0));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Rigidbody>().isKinematic = true;
            transform.position = Vector3.zero + Vector3.up * 2;
        }
    }

    public void AddKockBack(float force , Vector3 direction)
    {
        StopAllCoroutines();
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        float damageDone = (100 - Health);

        StartCoroutine(KnockBackNoMovement());

        IEnumerator KnockBackNoMovement()
        {
            if (damageDone <= 0)
                damageDone = 100;

            rb.AddForce(direction * (force * damageDone) * Time.deltaTime, ForceMode.Impulse);
            GetComponent<MeshRenderer>().material.color = Color.red;
            GetComponent<CapsuleCollider>().material = physicMaterials[1];
            yield return new WaitForSeconds(damageDone * KnockBackDelay);
            GetComponent<MeshRenderer>().material.color = Color.white;
            GetComponent<CapsuleCollider>().material = physicMaterials[0];
            rb.velocity = Vector3.zero;
        }
    }
}
