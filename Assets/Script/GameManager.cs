using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int itemCount = 0;
    public int totalItems = 5;

    public TMP_Text itemText;

    [Header("Door")]
    public GameObject doorClosed;
    public GameObject doorOpen;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdateUI();

        // Pastikan kondisi awal
        doorClosed.SetActive(true);
        doorOpen.SetActive(false);
    }

    public void AddItem()
    {
        itemCount++;

        UpdateUI();

        if (itemCount >= totalItems)
        {
            OpenDoor();
        }
    }

    void UpdateUI()
    {
        itemText.text = "Item Terkumpul: \n" + itemCount + "/" + totalItems;
    }

    void OpenDoor()
    {
        doorClosed.SetActive(false);
        doorOpen.SetActive(true);
    
        Debug.Log("Door Opened!");
    }
}