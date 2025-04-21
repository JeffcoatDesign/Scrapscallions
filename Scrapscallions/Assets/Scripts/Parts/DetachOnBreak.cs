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
        [SerializeField] float m_dropSpeed = 0.1f;
        [SerializeField] float m_timeUntilShrink = 10f;
        public bool IsDetached { get; set; } = false;
        private void OnEnable()
        {
            if (part != null)
                part.Broke += OnBroke;
        }

        private void OnDisable()
        {
            part.Broke -= OnBroke; 
        }

        protected void OnBroke()
        {
            if (part is LegsController)
            {
                LegsController legs = part as LegsController;
                legs.BodyAttachPoint.parent = transform.parent;
                StartCoroutine(DropBody(legs.BodyAttachPoint));
            }

            transform.parent = null;

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

            IsDetached = true;

            Invoke(nameof(DisableColliders), m_timeUntilDisableColliders);

            if (m_doesShrinkAndDisappear)
                StartCoroutine(ShrinkAndDisappear());
        }

        private IEnumerator DropBody(Transform body)
        {
            Transform childTransform = body.GetChild(0);
                if (childTransform == null) childTransform = transform;
            DetachOnBreak bodyDetachPoint = childTransform.GetComponent<DetachOnBreak>();
            if (bodyDetachPoint != null) {
                bodyDetachPoint.IsDetached = true;
            }

            Vector3 velocity = Vector3.zero;
            while (body.localPosition.y > 0)
            {
                velocity += Vector3.down * Time.deltaTime * m_dropSpeed;
                body.localPosition += velocity;
                yield return new WaitForEndOfFrame();
            }
            body.localPosition = body.localPosition.With(y:0);
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