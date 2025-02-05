using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float m_speed = 5;
    Vector3 m_destination;
    private void Awake()
    {
        m_destination = transform.position;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) { 
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                m_destination = hit.point;
            }
        }

        if (Vector3.Distance(transform.position, m_destination) > 1f)
        transform.position += (m_destination - transform.position).normalized * m_speed * Time.deltaTime;
    }
}
