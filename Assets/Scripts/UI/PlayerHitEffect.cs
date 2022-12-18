using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHitEffect : MonoBehaviour
{
    [SerializeField] private PlayerShooting playerShooting;

    [SerializeField]private Health playerHealth;

    [SerializeField] private UnityEvent playerHit;

    // Start is called before the first frame update
    void OnEnable()
    {
        if(playerShooting == null)
        {
            playerShooting = FindObjectOfType<PlayerShooting>();
        }

        if(playerHealth == null)
        {
            playerHealth = playerShooting.GetComponentInParent<Health>();

        }

        playerHealth.onTakeDamage += HitEffect;
    }

    private void OnDisable()
    {
        playerHealth.onTakeDamage -= HitEffect;
    }

    public void HitEffect(int damage)
    {
        playerHit.Invoke();
    }

    
}
