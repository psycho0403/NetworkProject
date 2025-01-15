using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ChatManager : NetworkBehaviour
{
    public static ChatManager Singleton;

    [SerializeField] chatMessage chatMessagePrefab;
    [SerializeField] CanvasGroup chatContent;
    [SerializeField] TMP_InputField chatInput;

    public string playerName;

    private void Awake() 
    { ChatManager.Singleton = this; }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            SendMessage(chatInput.text, playerName);
            chatInput.text = "";
        }
    }

    public void SendChatMessage(string _message, string _fromWho = null)
    {
        if (string.IsNullOrWhiteSpace(_message)) return;

        string S = _fromWho + " > " + _message;
        SendChatMessageServerRpc(S);
    }

    void AddMessage(string msg)
    {
        chatMessage CM = Instantiate(chatMessagePrefab, chatContent.transform);
        CM.SetText(msg);
    }

    [ServerRpc(RequireOwnership = false)]
    void SendChatMessageServerRpc(string message)
    {

    }

    [ClientRpc]
    void ReceiveChatMessageClientRpc(string message)
    {
        ChatManager.Singleton.AddMessage(message);
    }

}
