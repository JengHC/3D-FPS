using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FireWeapon()
    {
        Debug.Log("Fire");
        animator.SetTrigger("Fire");
    }

    public void ReloadWeapon()
    {
        
    }
}
