using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageCol : MonoBehaviour
{
    public int attackDamage = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHp>().Damage_HP(attackDamage);
        }
    }
}
