using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iceAttackE : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    private Rigidbody2D rigid;
    public GameObject iceEffect;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer ������Ʈ ��������
        rigid = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 7f);
    }

    private void Update()
    {
        spriteRenderer.flipX = rigid.velocity.x > 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("��");

            if (iceEffect != null)
            {
                Quaternion currentRotation = this.transform.rotation;
                GameObject explosion = Instantiate(iceEffect, this.transform.position, currentRotation);
                SpriteRenderer effectSpriteRenderer = explosion.GetComponent<SpriteRenderer>();
                if (effectSpriteRenderer != null)
                {
                    effectSpriteRenderer.flipX = spriteRenderer.flipX;
                }
                Destroy(explosion, 0.4f);
            }

            if (gameObject != null)
            {
                Destroy(gameObject);
            }
        }
    }
}