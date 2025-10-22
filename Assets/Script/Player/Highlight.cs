using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    [SerializeField] private List<Renderer> renderers;

    [SerializeField] private Color color = Color.white;

    private List<Material> materials;

    public bool canHighlight = true;

    public void Awake()
    {
        materials = new List<Material>();

        foreach (var renderer in renderers)
        {
            materials.AddRange(new List<Material>(renderer.materials));
        }
    }


    public void ToggleHighlight(bool val)
    {
        if (val)
        {
            foreach (var material in materials)
            {
                //���� Unity ����Ⱦϵͳ������� _EMISSION ���Է�֧��
                material.EnableKeyword("_EMISSION");

                material.SetColor("_EmissionColor", color);
            }
        }
        else
        {
            foreach (var material in materials)
            {
                material.DisableKeyword("_EMISSION");
            }
        }
    }
}
