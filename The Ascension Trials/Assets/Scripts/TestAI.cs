using UnityEngine;
using UnityEngine.AI;

public class TestAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // NavMeshHit hit;

        // if (NavMesh.SamplePosition(player.transform.position, out hit, 1.0f, NavMesh.AllAreas))
        // {
        //     agent.SetDestination(hit.position);
        // }
        agent.SetDestination(player.transform.position);
    }
}
