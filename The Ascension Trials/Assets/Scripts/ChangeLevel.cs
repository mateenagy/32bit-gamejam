using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLevel : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SkillManager.Instance.AddSkill(Skill.FireRate);
            SkillManager.Instance.AddSkill(Skill.Dash);
            SkillManager.Instance.AddSkill(Skill.Heal);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
