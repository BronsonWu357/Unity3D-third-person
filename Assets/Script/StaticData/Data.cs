using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static CharacterCustomized;

public static class Data
{
    public static LoadData.CharacterData[] characterDatas;


    public static Mesh MeshCustomizationLoad(string bodyPart)
    {
        foreach (var characterData in characterDatas)
        {
            if(characterData.bodyPart == bodyPart)
            {
                int index = PlayerPrefs.GetInt(bodyPart);

                Mesh mesh = characterData.meshes[index];

                return mesh;
            }
        }
        return null;
    }
}
