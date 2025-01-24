using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum Skill
{
    FireRate,
    Dash
}

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;
    public List<Skill> skills = new();
    public UIDocument skillUI;
    private VisualElement root;


    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void AddSkill(Skill skill)
    {
        skills.Add(skill);
    }

    public void RemoveSkill(Skill skill)
    {
        skills.Remove(skill);
    }

    void Start()
    {
    }

    void Update()
    {
        skillUI = GameObject.Find("Skills").GetComponent<UIDocument>();
        root = skillUI.rootVisualElement;
        SkillUIChecker("fire-rate-container", Skill.FireRate);
        SkillUIChecker("dash-container", Skill.Dash);
    }

    void SkillUIChecker(string _container, Skill skill)
    {
        var container = root.Q<VisualElement>(_container);
        if (skills.BinarySearch(skill) >= 0)
        {
            container.style.display = DisplayStyle.Flex;
        }
        else
        {
            container.style.display = DisplayStyle.None;
        }
    }
}
