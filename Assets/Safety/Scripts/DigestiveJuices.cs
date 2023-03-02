using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigestiveJuices : MonoBehaviour
{
    public int damageToFire = 1;
    void OnTriggerEnter(Collider other)
    {
        FirePlace fire = other.gameObject.GetComponent<FirePlace>();
        if(fire != null)
            fire.GetDamage(damageToFire);
    }
}
