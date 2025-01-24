using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DiscardSkill : MonoBehaviour
{
    UIDocument ui;
    VisualElement root;

    void Start()
    {
        Time.timeScale = 0;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        ui = GetComponent<UIDocument>();
        root = ui.rootVisualElement;
        VisualElement dashButton = root.Q<VisualElement>("dash-container");
        VisualElement fireRateButton = root.Q<VisualElement>("fire-rate-container");
        dashButton.RegisterCallback<ClickEvent>(ev =>
        {
            SkillManager.Instance.RemoveSkill(Skill.Dash);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            Time.timeScale = 1;
        });
        fireRateButton.RegisterCallback<ClickEvent>(ev =>
        {
            SkillManager.Instance.RemoveSkill(Skill.FireRate);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            Time.timeScale = 1;
        });
    }

    void Update()
    {
    }

}
