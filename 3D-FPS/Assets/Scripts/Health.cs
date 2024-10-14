using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float hp = 10;
    public float maxHp = 10;
    public float invincibleTime;    //무적시간

    public Image hpGauge;

    IHealthListener healthListener;
    float lastDamageTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthListener = GetComponent<IHealthListener>();
    }

    public void Damage(float damage)
    {
        if (hp > 0 && lastDamageTime+invincibleTime < Time.time)    // 무적 시간이면 안맞음
        {
            hp -= damage;

            if(hpGauge != null)
            {
                hpGauge.fillAmount = hp / maxHp;
            }

            lastDamageTime =Time.time;

            if (hp <= 0)
            {
                if(healthListener != null)
                {
                    healthListener.OnDie();
                }
            }
            else
            {
                Debug.Log("다침");
            }
        }
    }

    // 클래스 선언과 비슷하지만 이 함수들이 존재하기로 약속한다.
    // 언제 쓰는것인가? 어떻게 쓰는것인가? 에 대한 사례
    public interface IHealthListener
    {
        void OnDie();
    }

}
