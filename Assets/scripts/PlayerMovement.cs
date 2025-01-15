using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{

    [SerializeField] private float movementSpeed = 7f;
    [SerializeField] private float rotasionSpeed = 500f;
    [SerializeField] private float positionRange = 5f;
    private Animator animator; // probably wont use (Time constraints)

    void Start()
    {
        
    }

    public override void OnNetworkSpawn()
    {
        UpdatePositionServerRpc();
    }

    void Update()
    {
        if(!IsOwner) return;
        // cache input values in floats
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // create a movement direction vector3 and store the vertical and horizaontal values in it
        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        movementDirection.Normalize();

        // Move the transform in the movement direction
        transform.Translate(movementDirection * movementSpeed * Time.deltaTime, Space.World);

        // rotate the player to face the movement direction
        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotasionSpeed * Time.deltaTime);
        }

        // change the animation based on the movement value, probably wont use (Time constraints)
    }

    [ServerRpc (RequireOwnership = false)]
    private void UpdatePositionServerRpc()
    {
        transform.position = new Vector3(Random.Range(positionRange, -positionRange), 0, Random.Range(positionRange, -positionRange));
        transform.rotation = new Quaternion(0, 100, 0, 0);
    }
}
