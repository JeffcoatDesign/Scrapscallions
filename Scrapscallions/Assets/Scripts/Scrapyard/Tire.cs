using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.Props
{
    public class Tire : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out AttackCollider attackCollider))
            {
                if (attackCollider.CanHit)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}