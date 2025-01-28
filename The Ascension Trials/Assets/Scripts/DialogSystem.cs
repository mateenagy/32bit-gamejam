using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class DialogSystem : MonoBehaviour
{
    public string[] lines;
    public GameObject dialog;
    public InputActionReference nextDialogKey;
    int index = 0;
    public UIDocument ui;
    public bool showOnStart = false;
    public AudioSource stepSound;
    public bool isTextDialog = true;
    Label label;
    // Start is called before the first frame update

    void Awake()
    {
        if (!showOnStart)
        {
            dialog.SetActive(false);
        }
    }
    void Start()
    {
        if (showOnStart)
        {
            label = ui.rootVisualElement.Q("dialog-container").Q<Label>("dialog");
            label.text = lines[index];
            GameManager.Instance.isDialogue = true;
            // LevelManager.Instance.isDialog = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if (!LevelManager.Instance.isDialog)
        // {
        // 	Time.timeScale = 1;
        // 	return;
        // }

        if (isTextDialog)
        {
            GameManager.Instance.isDialogue = true;
            if (ui.rootVisualElement != null)
            {
                label = ui.rootVisualElement.Q("dialog-container").Q<Label>("dialog");


                if (Input.GetKeyDown(KeyCode.Return) || nextDialogKey.action.triggered)
                {
                    index += 1;
                    // stepSound.Play(); 
                }
                if (index < lines.Length)
                {
                    Time.timeScale = 0;
                    label.text = lines[index];
                }
                else
                {
                    // stepSound.Play();
                    // index = 0;
                    // LevelManager.Instance.isDialog = false;
                    Time.timeScale = 1;
                    StartCoroutine(HideDialog());
                    // GameManager.Instance.isDialogue = false;
                    // return;
                }
            }
        }
    }

    IEnumerator HideDialog()
    {
        Time.timeScale = 1;
        yield return new WaitForSeconds(0.01f);
        dialog.SetActive(false);
        GameManager.Instance.isDialogue = false;
        // dialog.SetActive(false);
    }

    public void ShowDialog()
    {
        dialog.SetActive(true);
        // LevelManager.Instance.isDialog = true;
    }
}
