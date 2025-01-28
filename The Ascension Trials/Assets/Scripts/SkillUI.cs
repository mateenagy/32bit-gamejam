using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
public class SkillUI : MonoBehaviour
{
    private UIDocument skillUI;
    private VisualElement root;

    void Start()
    {
        skillUI = GetComponent<UIDocument>();
        root = skillUI.rootVisualElement;
        SkillUIChecker("fire-rate-container", Skill.FireRate);
        SkillUIChecker("dash-container", Skill.Dash);
        SkillUIChecker("heal-container", Skill.Heal);
    }
    void SkillUIChecker(string _container, Skill skill)
    {
        var container = root.Q<VisualElement>(_container);
        if (SkillManager.Instance.skills.BinarySearch(skill) >= 0 || SceneManager.GetActiveScene().buildIndex == 0)
        {
            container.style.display = DisplayStyle.None;
        }
        else
        {
            container.style.display = DisplayStyle.None;
        }
    }
}
