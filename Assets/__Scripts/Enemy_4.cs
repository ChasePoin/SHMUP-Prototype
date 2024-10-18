using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(EnemyShield))]
public class Enemy_4 : Enemy
{
    [Header("enemy 4 inscribed")]
    public float duration = 4;

    private EnemyShield[] allShields;
    private EnemyShield thisShield;

    private Vector3 p0, p1;
    private float timeStart;
    // Start is called before the first frame update
    void Start()
    {
        allShields = GetComponentsInChildren<EnemyShield>();
        thisShield = GetComponent<EnemyShield>();

        p0 = p1 = pos;
        InitMovement();
    }
    void InitMovement() {
        p0 = p1;
        float widMinRad = bndChck.camWidth - bndChck.radius;
        float hgtMinRad = bndChck.camHeight - bndChck.radius;
        p1.x = Random.Range(-widMinRad, widMinRad);
        p1.y = Random.Range(-hgtMinRad, hgtMinRad);

        if (p0.x * p1.x > 0 && p0.y * p1.y > 0) {
            if (Mathf.Abs(p0.x) > Mathf.Abs(p0.y)) {
                p1.x *= -1;
            } else {
                p1.y *= -1;
            }
        }
        timeStart = Time.time;

    }

    // Update is called once per frame
    public override void Move()
    {
        float u = (Time.time - timeStart) / duration;
        if (u>= 1) {
            InitMovement();
            u = 0;
        }
        u = u - 0.15f * Mathf.Sin(u*2*Mathf.PI);
        pos = (1-u)*p0 + u*p1;
    }
    void OnCollisionEnter(Collision coll) {
        GameObject otherGO = coll.gameObject;
        ProjectileHero p = otherGO.GetComponent<ProjectileHero>();
        if (p != null) {
            Destroy(otherGO);

            if (bndChck.isOnScreen) {
                GameObject hitGO = coll.contacts[0].thisCollider.gameObject;
                if (hitGO == otherGO) {
                    hitGO = coll.contacts[0].otherCollider.gameObject;
                }

                float dmg = Main.GET_WEAPON_DEFINITION(p.type).damageOnHit;

                bool shieldFound = false;
                foreach( EnemyShield es in allShields) {
                    if (es.gameObject == hitGO) {
                        es.TakeDamage(dmg);
                        shieldFound = true;
                    }
                }
                if (!shieldFound) thisShield.TakeDamage(dmg);
                if (thisShield.isActive) return;
                if (!calledShipDestroyed) {
                    Main.SHIP_DESTROYED(this);
                    calledShipDestroyed = true;
                }
                Destroy(gameObject);
            }
            else {
                Debug.Log("Enemy 4 hit by non-rpoejctile hero" + otherGO.name);
            }
        }
        
    }
}
