using UnityEngine;

public class DoorController : MonoBehaviour
{
    public GameObject doorClosed;
    public GameObject doorOpen;

    public void OpenDoor()
    {
        doorClosed.SetActive(false);
        doorOpen.SetActive(true);
    }
}