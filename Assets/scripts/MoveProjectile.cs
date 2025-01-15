using Unity.Netcode;
using UnityEngine;

public class MoveProjectile : NetworkBehaviour
{

    public ShootProjectile parent;
    [SerializeField] private float shootForce;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        rb.linearVelocity = rb.transform.forward * shootForce;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsOwner) return;
        //Destroy(gameObject);

        parent.DestroyServerRpc();
    }
}
