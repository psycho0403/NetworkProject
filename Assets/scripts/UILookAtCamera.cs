using UnityEngine;

public class UILookAtCamera : MonoBehaviour
{
    void LateUpdate()
    {
        transform.LookAt(transform.position + Camera.main.transform.position);
    }
}
