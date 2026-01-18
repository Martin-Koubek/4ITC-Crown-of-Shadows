using UnityEngine;

public class ConnectionPoint : MonoBehaviour
{
    public bool used = false;

    public Vector3 direction = Vector3.forward;
    public Vector3 GetCorridorStart()
    {
        return transform.position + direction.normalized;
    }
}
