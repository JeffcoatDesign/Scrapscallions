using Scraps.Parts;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Parts
{
    public class DetachOnBreak : MonoBehaviour
    {
        public PartController part;
        public float force = 10f;
        [SerializeField] private LayerMask m_excludeLayers;
        [SerializeField] float m_timeUntilDisableColliders = 2f;
        [SerializeField] bool m_doesShrinkAndDisappear = true;
        [SerializeField] float m_shrinkSpeed = 0.05f;
        [SerializeField] float m_timeUntilShrink = 10f;
        private void OnEnable()
        {
            part.Broke += OnBroke;
        }

        private void OnDisable()
        {
            part.Broke -= OnBroke; 
        }

        protected void OnBroke()
        {
            transform.parent = null;

            if (part is LegsController)
            {
                LegsController legs = part as LegsController;
                legs.BodyAttachPoint.transform.parent = transform.parent;
            }

            if (part != null && part.GetRobot() != null && part.GetRobot().agent != null)
            {
                if (!gameObject.TryGetComponent(out Rigidbody rb))
                    rb = gameObject.AddComponent<Rigidbody>();
                Vector3 randomDir = new(Random.Range(-1f, 1f), Random.Range(0.5f, 1f), Random.Range(-1f, 1f));
                rb.AddForce(((transform.position - part.GetRobot().agent.transform.position).normalized + randomDir) * force, ForceMode.Impulse);
            }

            if(GetComponent<Collider>() == null)
            {
                gameObject.AddComponent<BoxCollider>();
            }

            Invoke(nameof(DisableColliders), m_timeUntilDisableColliders);

            if (m_doesShrinkAndDisappear)
                StartCoroutine(ShrinkAndDisappear());
        }

        private IEnumerator ShrinkAndDisappear()
        {
            yield return new WaitForSeconds(m_timeUntilShrink);
            while(transform.localScale.x > 0f)
            {
                float value = transform.localScale.x - (1 - m_shrinkSpeed) * Time.fixedDeltaTime;
                value = Mathf.Clamp01(value);
                transform.localScale = Vector3.one * value;
                yield return new WaitForFixedUpdate();
            }
            Destroy(gameObject);
            yield return null;
        }

        private void DisableColliders()
        {
            gameObject.layer = LayerMask.NameToLayer("Broken Parts");
            var colliders = GetComponents<Collider>();
            foreach (var collider in colliders) {
                collider.excludeLayers = m_excludeLayers;
            }
        }

        private void Reset()
        {
            part = GetComponent<PartController>();
        }
    }
}