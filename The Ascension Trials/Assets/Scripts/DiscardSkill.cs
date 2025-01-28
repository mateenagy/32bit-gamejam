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
        SkillUIChecker("dash-container", Skill.Dash);
        SkillUIChecker("fire-rate-container", Skill.FireRate);
        SkillUIChecker("heal-container", Skill.Heal);
    }

    void SkillUIChecker(string _container, Skill skill)
    {
        var container = root.Q<VisualElement>(_container);
        if (SkillManager.Instance.skills.BinarySearch(skill) >= 0)
        {
            container.style.display = DisplayStyle.Flex;
            container.RegisterCallback<ClickEvent>(ev =>
            {
                SkillManager.Instance.RemoveSkill(skill);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                Time.timeScale = 1;
            });
        }
        else
        {
            container.style.display = DisplayStyle.None;
        }
    }

    void Update()
    {
    }

}
