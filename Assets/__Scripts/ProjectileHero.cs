using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoundsCheck))]
public class ProjectileHero : MonoBehaviour
{
    private BoundsCheck bndChck;
    private Renderer rend;
    [Header("dynamic")]
    public Rigidbody rigid;
    [SerializeField]
    private eWeaponType _type;
    public eWeaponType type {
        get { return(_type);}
        set {SetType(value);}
    }
    // Start is called before the first frame update
    void Awake()
    {
        bndChck = GetComponent<BoundsCheck>();
        rend = GetComponent<Renderer>();
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bndChck.LocIs(BoundsCheck.eScreenLocs.offUp)) {
            Destroy(gameObject);
        }
    }
    public void SetType(eWeaponType eType) {
        _type = eType;
        WeaponDefinition def = Main.GET_WEAPON_DEFINITION(_type);
        rend.material.color = def.projectileColor;
    }
    public Vector3 vel {
        get { return rigid.velocity;}
        set { rigid.velocity = value;}
    }
}
