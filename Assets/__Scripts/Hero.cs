using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero  S { get; private set; }

    [Header("Inscribed")]
    public float        speed    = 30;
    public float        rollMult = -45;
    public float        pitchMult = 30;
    public GameObject   projectilePrefab;
    public float        projectileSpeed = 40;
    public Weapon[]     weapons;

    [Header("Dynamic")] [Range(0,4)] [SerializeField]
    private float        _shieldLevel = 4;
    public GameObject lastTriggerGo = null;
    public delegate void WeaponFireDelegate();
    public event WeaponFireDelegate fireEvent;

    void Awake()
    {
        if (S == null) {
            S = this;
        } else {
            Debug.LogError("Hero.Awake() attempted to assign second Hero.S!");
        }
        // fireEvent += TempFire;
        ClearWeapons();
        weapons[0].SetType(eWeaponType.blaster);
    }

    // Update is called once per frame
    void Update()
    {
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        Vector3 pos = transform.position;
        pos.x += hAxis * speed * Time.deltaTime;
        pos.y += vAxis * speed * Time.deltaTime;

        transform.position = pos;

        transform.rotation = Quaternion.Euler(vAxis*pitchMult,hAxis*rollMult,0);
        if (Input.GetAxis("Jump") == 1 && fireEvent != null) {
            fireEvent();
        }
    }
    // void TempFire() {
    //     GameObject projGO = Instantiate<GameObject>(projectilePrefab);
    //     projGO.transform.position = transform.position;
    //     Rigidbody rigidB = projGO.GetComponent<Rigidbody>();
        
    //     ProjectileHero proj = projGO.GetComponent<ProjectileHero>();
    //     proj.type = eWeaponType.blaster;
    //     float tSpeed = Main.GET_WEAPON_DEFINITION(proj.type).velocity;
    //     rigidB.velocity = Vector3.up * tSpeed;
    // }
    void OnTriggerEnter(Collider other) {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;
        PowerUp pUp = go.GetComponent<PowerUp>();
        
        if (go == lastTriggerGo) return;
        lastTriggerGo = go;

        Enemy enemy = go.GetComponent<Enemy>();
        if (enemy != null) {
            shieldLevel--;
            Destroy(go);
        } else if (pUp != null) {
            AbsorbPowerUp(pUp);
        }
        else {
            Debug.LogWarning("Shield trigger hit by unidentified creature");
        }
    }
    public float shieldLevel {
        get { return (_shieldLevel); }
        private set {
            _shieldLevel = Mathf.Min(value, 4);
            if (value < 0) {
                Destroy(this.gameObject);
                Main.HERO_DIED();
            }
        }
    }
    public void AbsorbPowerUp(PowerUp pUp) {
        Debug.Log("absorbed power up" + pUp.type);
        switch (pUp.type) {
            case eWeaponType.shield:
                shieldLevel++;
                break;
            default:
                if(pUp.type == weapons[0].type) {
                    Weapon weap = GetEmptyWeaponSlot();
                    if (weap != null) {
                        weap.SetType(pUp.type); 
                    }
                } else {
                        ClearWeapons();
                        weapons[0].SetType(pUp.type);
                    }
                    
                
                break;
        }
        pUp.AbsorbedBy(this.gameObject);
    }
    Weapon GetEmptyWeaponSlot() {
        for (int i =0; i < weapons.Length; i++) {
            if (weapons[i].type == eWeaponType.none) {
                return(weapons[i]);
            }
        }
        return(null);
    }
    void ClearWeapons() {
        foreach(Weapon w in weapons) {
            w.SetType(eWeaponType.none);
        }
    }
}
