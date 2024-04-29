using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponData weaponData;

    public void Init(int weaponid)
    {
        weaponData = GetComponent<WeaponData>();
        weaponData.Init(weaponid);
    }
}
