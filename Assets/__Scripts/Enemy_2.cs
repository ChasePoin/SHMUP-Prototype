using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2 : Enemy
{
    [Header("Enemy_2 Inscribed Fields")]
    public float lifeTime = 10;
    public float sinEccentricity = 0.6f;
    public AnimationCurve rotCurve;
    [Header("Enemy_2 Private Fields")]
    [SerializeField] private float birthTime;
    private Quaternion baseRotation;
    [SerializeField] private Vector3 p0, p1;
    // Start is called before the first frame update
    void Start()
    {
        p0 = Vector3.zero;
        p0.x = -bndChck.camWidth - bndChck.radius;
        p0.y = Random.Range(-bndChck.camHeight, bndChck.camHeight);

        p1 = Vector3.zero;
        p1.x = bndChck.camWidth + bndChck.radius;
        p1.y = Random.Range(--bndChck.camHeight, bndChck.camHeight);

        if (Random.value > 0.5f) {
            p0.x *= -1;
            p1.x *= -1;
        }
        birthTime = Time.time;
        transform.position = p0;
        transform.LookAt(p1, Vector3.back);
        baseRotation = transform.rotation;
    }

    // Update is called once per frame
    public override void Move()
    {
        float u = (Time.time - birthTime) / lifeTime;

        if (u > 1) {
            Destroy(this.gameObject);
            return;
        }

        float shipRot = rotCurve.Evaluate(u)*360;
        transform.rotation = baseRotation * Quaternion.Euler(-shipRot, 0, 0);
        u = u + sinEccentricity*(Mathf.Sin(u*Mathf.PI*2));
        pos = (1-u)*p0 + u*p1;
    }
}
