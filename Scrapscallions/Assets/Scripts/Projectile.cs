using Scraps.Parts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.Gameplay
{
    public class Projectile : MonoBehaviour
    {
        public Robot robot;
        public int damage = 10;
        public float lifetime = 10f;
        public bool canHit = true;
        Vector3 dir = Vector3.zero;
        public void Launch(Vector3 direction, float force, bool useAngular = false)
        {
            Vector3 velocity = direction.normalized * force;
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb.isKinematic) { 
                rb.isKinematic = false;
            }
            canHit = true;
            transform.parent = null;
            rb.AddForce(velocity, ForceMode.VelocityChange);

            if(useAngular)
            {
                rb.angularVelocity = velocity;
            }

            dir = velocity;

            Invoke(nameof(Kill), lifetime);
        }

        private void Kill()
        {
            Destroy(gameObject);
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
                Kill();
            }
        }

        private void Update()
        {
            Debug.DrawLine(transform.position, transform.position + dir, Color.yellow);
        }
    }
}