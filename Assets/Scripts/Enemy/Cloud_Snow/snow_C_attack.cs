using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scow_attack : MonoBehaviour
{
    public GameObject iceEffect;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("��");

            if (iceEffect != null)
            {
                Quaternion currentRotation = this.transform.rotation;
                GameObject explosion = Instantiate(iceEffect, this.transform.position, currentRotation);
                Destroy(explosion, 0.2f);
            }

            Destroy(gameObject);
        }
    }
}
