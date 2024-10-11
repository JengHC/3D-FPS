using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    GameObject player;
    NavMeshAgent agent;
    public float walkSpeed = 3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();

        // 적의 기본 이동 속도를 설정
        agent.speed = walkSpeed;

        agent.destination = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //agent.destination = player.transform.position;
        if(agent.remainingDistance < 1f)    //목적지까지 남은 거리가 1m 미만이면
        {
            agent.destination = player.transform.position;
        }
    }
}
