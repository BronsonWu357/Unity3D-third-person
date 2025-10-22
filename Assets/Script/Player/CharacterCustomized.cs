using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static CharacterCustomized;

public class CharacterCustomized : MonoBehaviour
{
    [Header("Outwear")]
    [SerializeField] private BodyPartData outwearBPD;

    [Header("Shoes")]
    [SerializeField] private BodyPartData shoesBPD;

    [Header("Body")]
    [SerializeField] private SkinnedMeshRenderer BodySkinnedMeshRenderer;
    [SerializeField] private Slider skinColorSlider;


    [Serializable]
    public class BodyPartData
    {
        public SkinnedMeshRenderer skinnedMeshRenderer;
        public Mesh[] meshes;
        public string[] names;
        public TMP_Text value;
        public int index;
    }


    public void Start()
    {
        LoadAllData();
    }


    public void ChangeNext(BodyPartData bodyPartData)
    {
        //int index = System.Array.IndexOf(strings, text.text);
        //当SkinnedMeshRenderer.sharedMesh不包含于meshs时，会返回-1
        //if (index == -1)
        //{
        //    index = 0;
        //}
        //求余防止容器越界

        bodyPartData.index = Mathf.Abs((bodyPartData.index + 1) % bodyPartData.meshes.Length);

        bodyPartData.skinnedMeshRenderer.sharedMesh = bodyPartData.meshes[bodyPartData.index];

        bodyPartData.value.text = bodyPartData.names[bodyPartData.index];
    }


    public void ChangeLast(BodyPartData bodyPartData)
    {
        if (bodyPartData.index == 0)
        {
            bodyPartData.index = bodyPartData.names.Length;
        }

        bodyPartData.index = Mathf.Abs((bodyPartData.index - 1) % bodyPartData.meshes.Length);

        bodyPartData.skinnedMeshRenderer.sharedMesh = bodyPartData.meshes[bodyPartData.index];

        bodyPartData.value.text = bodyPartData.names[bodyPartData.index];
    }


    public void ChangeOutwearNext()
    {
        ChangeNext(outwearBPD);
    }


    public void ChangeOutwearLast()
    {
        ChangeLast(outwearBPD);
    }


    public void ChangeShoesNext()
    {
        ChangeNext(shoesBPD);
    }


    public void ChangeShoesLast()
    {
        ChangeLast(shoesBPD);
    }


    public void ChangeSkinColor(float value)
    {
        float colorValue = (255 - value) / 255;

        Color color = new Color(colorValue,colorValue,colorValue,1f);

        BodySkinnedMeshRenderer.material.SetColor("_BaseColor", color);
    }


    public void Save()
    {
        PlayerPrefs.SetInt("outwearCustomizaton", outwearBPD.index);

        PlayerPrefs.SetInt("shoesCustomization", shoesBPD.index);

        Color color = BodySkinnedMeshRenderer.material.GetColor("_BaseColor");
        
        string json = JsonUtility.ToJson(color);

        PlayerPrefs .SetString("skinColorCustomization", json);

        Mesh mesh = outwearBPD.skinnedMeshRenderer.sharedMesh;

        PlayerPrefs.Save();
    }


    public void LoadAllData()
    {
        Load("outwearCustomizaton",outwearBPD);

        Load("shoesCustomization", shoesBPD);

        string json = PlayerPrefs.GetString("skinColorCustomization");

        Color color = JsonUtility.FromJson<Color>(json);

        BodySkinnedMeshRenderer.material.SetColor("_BaseColor", color);
    }


    public void Load(String name,BodyPartData bodyPartData)
    {
        int index = PlayerPrefs.GetInt(name);

        bodyPartData.index = index;

        bodyPartData.value.text = bodyPartData.names[index];

        bodyPartData.skinnedMeshRenderer.sharedMesh = bodyPartData.meshes[index];
    }
}
