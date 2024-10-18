using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Inscribed")]
    public float speed     = 10f;
    public float fireRate  = 0.3f;
    public float health    = 10;
    public int score       = 100;
    protected BoundsCheck bndChck;
    public float powerUpDropChance = 1f;
    protected bool calledShipDestroyed = false;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        Move();
        if (bndChck.LocIs(BoundsCheck.eScreenLocs.offDown)) {
                Destroy(gameObject);
            }
    }
    public Vector3 pos {
        get {
            return this.transform.position;
        } set {
            this.transform.position = value;
        }
    }
    public virtual void Move() {
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }
    void Awake() {
        bndChck = GetComponent<BoundsCheck>();
    }
    void OnCollisionEnter(Collision coll) {
        GameObject otherGO = coll.gameObject;
        ProjectileHero p = otherGO.GetComponent<ProjectileHero>();
        if (p != null) {
            if (bndChck.isOnScreen) {
                health -= Main.GET_WEAPON_DEFINITION(p.type).damageOnHit;
                if (health <= 0) {
                    if (!calledShipDestroyed) {
                        calledShipDestroyed = true;
                        Main.SHIP_DESTROYED(this);
                    }
                    Destroy(this.gameObject);
                }
            }
            Destroy(otherGO);
        } else {
            print("enemy hit by non projectilehero" + otherGO.name);
        }
    }
}
