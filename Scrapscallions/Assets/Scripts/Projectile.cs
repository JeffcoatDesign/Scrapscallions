using Scraps.Parts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Robot robot;
    public int damage = 10;
    public int force = 25;
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * force, ForceMode.VelocityChange);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Robot")) return;

        if (other.TryGetComponent(out PartController otherPart))
        {
            if (otherPart.GetRobot() == robot) return;
            if (otherPart.isBroken) return;
            Debug.Log("Ranged hit for " + damage);
            otherPart.Hit(damage);
            Destroy(gameObject);
        }
    }
}
