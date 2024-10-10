using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject trailPrefab;      // 총알궤적
    public Transform firingPosition;    // 발사 위치, 총알 시작 점

    Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FireWeapon()
    {
        // 현재 상태가 무엇인지 확인
        // Idle 상태에서만 작동
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            animator.SetTrigger("Fire");
            RayCastFire();
        }  
    }

    public void ReloadWeapon()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            animator.SetTrigger("Reload");
        }
    }
    void RayCastFire()
    {
        Camera cam = Camera.main;

        RaycastHit hit;
        // 카메라 정가운데 빛을 만들어서 가지고 온다 
        Ray r = cam.ViewportPointToRay(Vector3.one / 2); // Ray 는 빛, RayCast는 빛 

        Vector3 hitPosition = r.origin + r.direction * 200;
        //광선을 1000 단위 거리까지 발사하여 충돌하는 물체가 있는지 확인
        // 어딘가 빛이 부딪히면 true, 아니면 false, 충돌정보를 hit에 저장
        if (Physics.Raycast(r,out hit,1000))     
        {
            hitPosition = hit.point;            // 충돌하면, 부딪힌 좌표값 
        }

        GameObject go = Instantiate(trailPrefab);   // trailPrefab오브젝트 생성
        Vector3[] pos = new Vector3[] { firingPosition.position, hitPosition }; // 점 위치 저장
        // Positions에 배열을 넣으면 그걸 이어서 직선을그림
        // 프리팹 좌표 지정
        go.GetComponent<LineRenderer>().SetPositions(pos);  
    }
}
