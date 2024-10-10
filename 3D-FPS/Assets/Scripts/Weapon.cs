using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject trailPrefab;      // �Ѿ˱���
    public Transform firingPosition;    // �߻� ��ġ, �Ѿ� ���� ��

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
        // ���� ���°� �������� Ȯ��
        // Idle ���¿����� �۵�
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
        // ī�޶� ����� ���� ���� ������ �´� 
        Ray r = cam.ViewportPointToRay(Vector3.one / 2); // Ray �� ��, RayCast�� �� 

        Vector3 hitPosition = r.origin + r.direction * 200;
        //������ 1000 ���� �Ÿ����� �߻��Ͽ� �浹�ϴ� ��ü�� �ִ��� Ȯ��
        // ��� ���� �ε����� true, �ƴϸ� false, �浹������ hit�� ����
        if (Physics.Raycast(r,out hit,1000))     
        {
            hitPosition = hit.point;            // �浹�ϸ�, �ε��� ��ǥ�� 
        }

        GameObject go = Instantiate(trailPrefab);   // trailPrefab������Ʈ ����
        Vector3[] pos = new Vector3[] { firingPosition.position, hitPosition }; // �� ��ġ ����
        // Positions�� �迭�� ������ �װ� �̾ �������׸�
        // ������ ��ǥ ����
        go.GetComponent<LineRenderer>().SetPositions(pos);  
    }
}
