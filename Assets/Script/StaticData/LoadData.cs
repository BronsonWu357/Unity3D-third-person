using System;
using UnityEngine;

public class LoadData : MonoBehaviour
{
    [SerializeField] CharacterData[] characterDatas;

    [Serializable]
    public class CharacterData
    {
        public Mesh[] meshes;
        public string[] names;
        public string bodyPart;
    }

    public void Awake()
    {
        Data.characterDatas = characterDatas;
    }


}
