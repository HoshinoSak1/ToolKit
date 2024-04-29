using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterData characterData;

    public GameObject weapon;

    public int nowLevel = 1;

    // Start is called before the first frame update
    void Start()
    {
        characterData = GetComponent<CharacterData>();
        characterData.Init(1001);

        weapon.GetComponent<Weapon>().Init(characterData.weaponDir[nowLevel]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
