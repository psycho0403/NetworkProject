using TMPro;
using UnityEngine;

public class chatMessage : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI messageText;

    public void SetText(string str) { messageText.text = str; }
}
