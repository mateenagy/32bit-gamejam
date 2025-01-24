using Unity.VisualScripting;
using UnityEngine;

public class TriggerDialog : MonoBehaviour
{
    [SerializeField] private DialogSystem dialogSystem;
    [SerializeField] bool triggered = false;
    [SerializeField] bool entered = false;
    PlayerSM playerSM;

    void Start()
    {
        playerSM = FindFirstObjectByType<PlayerSM>();
    }

    void Update()
    {
        if (playerSM && playerSM.IsGrounded && entered && !triggered)
        {
            dialogSystem.dialog.SetActive(true);
            triggered = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered");
        if (other.CompareTag("Player") && !triggered)
        {
            entered = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && triggered)
        {
            triggered = true;
            entered = false;
        }
    }
}
