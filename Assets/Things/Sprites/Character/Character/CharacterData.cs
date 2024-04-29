using cfg;
using cfg.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData:MonoBehaviour
{
    public int id;
    public string characterName;

    public Dictionary<int,int> weaponDir = new Dictionary<int,int>();

    public void Init(int chaId)
    {
        CharacterDefine define = TableDataManager.Instance.tables.CharacterDefine;

        CharacterConfigs configs =  define.DataMap[chaId];

        id = configs.Id;
        characterName = configs.Name;
        for(int i = 0;i< configs.Level.Count;i++)
        {
            weaponDir.Add(configs.Level[i], configs.WeaponId[i]);
        }
    }
}
