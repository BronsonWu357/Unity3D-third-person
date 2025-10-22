using UnityEngine;

public class Loader : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer outwearSkinnedMeshRenderer;

    [SerializeField] private SkinnedMeshRenderer shoesSkinnedMeshRenderer;

    [SerializeField] private SkinnedMeshRenderer bodySkinnedMeshRenderer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        outwearSkinnedMeshRenderer.sharedMesh = Data.MeshCustomizationLoad("outwearCustomizaton");

        shoesSkinnedMeshRenderer.sharedMesh = Data.MeshCustomizationLoad("shoesCustomization");

        string json = PlayerPrefs.GetString("skinColorCustomization");

        Color color = JsonUtility.FromJson<Color>(json);

        bodySkinnedMeshRenderer.material.SetColor("_BaseColor", color);
    }

}
