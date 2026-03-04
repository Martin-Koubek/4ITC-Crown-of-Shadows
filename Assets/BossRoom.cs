using System.Collections;
using UnityEngine;

public class BossRoom : MonoBehaviour
{
    public Boss boss;
    public GameObject RoomDoor;

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            StartCoroutine(CloseDoor());
        }
    }
    private IEnumerator CloseDoor()
    {
        yield return new WaitForSeconds(2);
        RoomDoor.SetActive(true);
        boss.playerInRoom = true;
    }
}
