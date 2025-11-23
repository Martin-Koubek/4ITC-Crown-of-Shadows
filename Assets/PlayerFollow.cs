using Unity.Mathematics;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    public Transform player;

    void Update()
    {
        Vector3 lookPos = transform.position - player.position;
        lookPos.y = 0;

        if (lookPos.sqrMagnitude < 0.00001f) return;

        transform.rotation = Quaternion.LookRotation(lookPos);
    }
}
