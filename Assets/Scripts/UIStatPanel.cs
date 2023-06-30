using UnityEngine;
using TMPro;

public class UIStatPanel : MonoBehaviour
{
    public TextMeshProUGUI linkTitle;
    public TextMeshProUGUI linkCounter;

    public void UpdateField(string title, int count)
    {
        linkTitle.text = title;
        linkCounter.text = $"{count}";
    }
}