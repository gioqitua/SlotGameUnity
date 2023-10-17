using System.Linq;
using TMPro;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class Card : MonoBehaviour
{
    public TMP_Text Text;
    MeshRenderer rend;
    public Material WinMaterial;
    public Material DefaultMaterial;
    private void Awake()
    {
        rend = GetComponent<MeshRenderer>();
        rend.material = DefaultMaterial;
    }
    public void SetText(string text)
    {
        Text.text = text;
    }
    public void SetParent(GameObject go)
    {
        this.transform.parent = go.transform;
    }
    public void SetWinMaterial()
    {
        rend.material = WinMaterial;
    }
}
