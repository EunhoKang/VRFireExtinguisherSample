using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePlace : MonoBehaviour
{
    public int health = 20;
    private int initialHelath;
    private Vector3 initialScale;
    private Vector3 endScale;
    private BoxCollider boxCollider;

    public void Start()
    {
        initialHelath = health;
        initialScale = transform.localScale;
        endScale = transform.localScale * 0.2f;
        boxCollider = GetComponent<BoxCollider>(); 
    }
    
    public void GetDamage(int amount)
    {
        health -= amount;
        if(health <= 0) 
        {
            health = 0;
            Dead();
        }
        transform.localScale = Vector3.Lerp(initialScale, endScale, (float)(initialHelath - health) / initialHelath);
    }

    public void Dead()
    {
        gameObject.SetActive(false);
    }
}
