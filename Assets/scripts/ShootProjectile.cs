using NUnit.Framework;
using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ShootProjectile : NetworkBehaviour
{
    [SerializeField] private GameObject BulletProjectile;
    [SerializeField] private GameObject HiChatBox;
    [SerializeField] private Transform shootTransform;
    // Not optimal but "me dumb" List to hold all the instantiaded projectiles
    [SerializeField] private List<GameObject> spawnedProjectile = new List<GameObject>();

    private void Update()
    {
        if(!IsOwner) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootServerRpc();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            ShootChatBoxServerRpc();
        }
    }

    [ServerRpc]
    private void ShootServerRpc()
    {
        GameObject go = Instantiate(BulletProjectile, shootTransform.position, shootTransform.rotation);
        spawnedProjectile.Add(go);
        go.GetComponent<MoveProjectile>().parent = this;
        go.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc]
    private void ShootChatBoxServerRpc()
    {
        GameObject chatBox = Instantiate(HiChatBox, shootTransform.position, Quaternion.identity);
        chatBox.transform.rotation = Quaternion.Euler(0, 0, 0); // Align with player's facing direction
        chatBox.GetComponent<NetworkObject>().Spawn();
        StartCoroutine(DestroyAfterSeconds(chatBox, 5f)); // Destroy after 5 seconds
    }


    [ServerRpc(RequireOwnership =false)]
    public void DestroyServerRpc()
    {
        GameObject toDestroy = spawnedProjectile[0];
        toDestroy.GetComponent<NetworkObject>().Despawn();
        spawnedProjectile.Remove(toDestroy);
        Destroy(toDestroy);
    }

    private IEnumerator DestroyAfterSeconds(GameObject obj, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (obj != null)
        {
            obj.GetComponent<NetworkObject>().Despawn();
            Destroy(obj);
        }
    }
}
