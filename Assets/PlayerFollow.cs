using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    public Camera player;

    [System.Obsolete]
    private void Start()
    {
        player = FindAnyObjectByType<Camera>();
    }
    void Update()
    {
        Vector3 lookPos = transform.position - player.transform.position;
        lookPos.y = 0;

        if (lookPos.sqrMagnitude < 0.00001f) return;

        transform.rotation = Quaternion.LookRotation(lookPos);
    }
}
