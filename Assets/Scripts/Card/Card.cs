using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    public TMP_Text Text;
    Renderer rend;
    private void Awake()
    {
        rend = GetComponent<Renderer>();
    }
    public void SetText(string text)
    {
        Text.text = text;
    }
    public void SetParent(GameObject go)
    {
        this.transform.parent = go.transform;
    }
    public void SetColor(Color color)
    {
        rend.material.color = color;
    }
}
