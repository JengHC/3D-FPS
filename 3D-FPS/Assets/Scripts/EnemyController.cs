using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.UIElements;

// Health.IHealthListener 인터페이스 구현,
// c#은 다중 상속이 금지되어 있기에, 인터페이스로 구현
// 이미 스크립트에 MonoBehaviour 클래스 상속받았기에 인터페이스로 구현
public class EnemyController : MonoBehaviour, Health.IHealthListener
{
    enum State
    {
        Idle,
        Follow,
        Attack,
    }

    GameObject player;
    NavMeshAgent agent;
    Animator animator;
    new AudioSource audio;  
    public float walkSpeed = 3;

    State state;    // 적의 현재 상태
    float currentStateTime; // 현재 상태 시간
    public float timeForNextState = 2; //2초간 같은 상태에 머문다

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        audio = GetComponent<AudioSource>();

        // 적의 기본 이동 속도를 설정
        agent.speed = walkSpeed;

        agent.destination = player.transform.position;

        state = State.Idle;
        currentStateTime = timeForNextState = 2;
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case State.Idle:
                currentStateTime -= Time.deltaTime;
                if(currentStateTime <0)
                {
                    float distance = (player.transform.position - transform.position).magnitude;
                    if(distance<1.5f)
                    {
                        StartAttack();
                    }
                    else
                    {
                        StartFollow();
                    }
                }
                break;

            case State.Follow:
                //남은 거리가 1m 미만이면 또는,Agent가 갈 수 있는 경로를 가지고 있는지 확인
                if (agent.remainingDistance < 1.5f || !agent.hasPath) 
                {
                    StartIdle();
                }
                break;

            case State.Attack:
                currentStateTime -= timeForNextState;
                if(currentStateTime<0)
                {
                    StartIdle();
                }

                break;
        }
    }
    void StartIdle()
    {
        audio.Stop();
        state = State.Idle;
        currentStateTime = timeForNextState;
        agent.isStopped = true;
        animator.SetTrigger("Idle");
    }

    void StartFollow()
    {
        audio.Play();
        state = State.Follow;
        agent.destination = player.transform.position;
        agent.isStopped = false;
        animator.SetTrigger("Run");
    }
    void StartAttack()
    {
        state = State.Attack;
        agent.isStopped = true;
        currentStateTime = timeForNextState;
        animator.SetTrigger("Attack");

    }

    // Health.IHealthListener의 OnDie()함수 필수
    public void OnDie()
    {
        //throw new System.NotImplementedException();
        Debug.Log("Real Die");
        agent.isStopped = true;
        animator.SetTrigger("Die");
        Invoke("OnDestroy", 2);
    }

    void OnDestroy()
    {
        GameManager.Instance.EnemyDie();
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<Health>().Damage(10);
        }
    }
}
