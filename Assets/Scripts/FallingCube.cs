using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingCube : MonoBehaviour {
    [SerializeField] private float m_fallSpeed = 10.0f;
    [SerializeField] private float m_fallDistance = 50.0f;
    //[SerializeField] private float m_rotateSpeed = 1.0f;
    private Vector3 m_initalPosition;
	// Use this for initialization
	void Start () {
        m_initalPosition = this.transform.position;

    }
	
	// Update is called once per frame
	void Update () {
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - m_fallSpeed * Time.deltaTime, this.transform.position.z);
        if((this.transform.position - m_initalPosition).sqrMagnitude >= m_fallDistance * m_fallDistance)
        {
            this.transform.position = m_initalPosition;
        }
	}
}
