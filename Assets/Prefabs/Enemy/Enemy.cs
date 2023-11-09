using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour, IMovementInterface, IBTTaskInterface, ITeamInterface
{
    [SerializeField] ValueGuage healthBarPrefab;
    [SerializeField] Transform healthBarAttachTransform;
    [SerializeField] DamageComponent damageComponent;
    [SerializeField] int teamID = 2;

    HealthComponet healthComponet;
    MovementComponent movementComponent;

    Animator animator;

    Vector3 prevPosition;
    Vector3 velocity;

    ValueGuage healthBar;

    private void Awake()
    {
        healthComponet = GetComponent<HealthComponet>();
        healthComponet.onTakenDamage += TookDamage;
        healthComponet.onHealthEmpty += StartDealth;
        healthComponet.onHealthChanged += HealthChanged;


        healthBar = Instantiate(healthBarPrefab, FindObjectOfType<Canvas>().transform);
        UIAttachComponent attachmentComp = healthBar.AddComponent<UIAttachComponent>();
        attachmentComp.SetupAttachment(healthBarAttachTransform);
        movementComponent = GetComponent<MovementComponent>();
        animator = GetComponent<Animator>();
        damageComponent.SetTeamInterface(this);
    }

    public int GetTeamID()
    {
        return teamID;
    }
    private void HealthChanged(float currentHealth, float delta, float maxHealth)
    {
        healthBar.SetValue(currentHealth, maxHealth);
    }

    private void StartDealth(float delta, float maxHealth)
    {
        Destroy(healthBar.gameObject);
        animator.SetTrigger("Die");
        GetComponent<AIController>().StopAILogic();
        Debug.Log("Dead!");
    }

    private void TookDamage(float currentHealth, float delta, float maxHealth, GameObject instigator)
    {
        Debug.Log($"Took Damage {delta}, now health is: {currentHealth}/{maxHealth}");
    }

    // Start is called before the first frame update
    void Start()
    {
        prevPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateVelocity();
    }

    private void CalculateVelocity()
    {
        velocity = (transform.position - prevPosition) / Time.deltaTime;
        prevPosition = transform.position;
        animator.SetFloat("speed", velocity.magnitude);
    }

    public void RotateTowards(Vector3 direction)
    {
        movementComponent.RotateTowards(direction);
    }

    public void RotateTowards(GameObject target)
    {
        movementComponent.RotateTowards(target.transform.position - transform.position);
    }

    public void AttackTarget(GameObject target)
    {
        animator.SetTrigger("Attack");
    }

    public void AttackPoint()
    {
        damageComponent.DoDamage();
    }
    public void DeathAnimationFinished()
    {
       
        Destroy(gameObject);
    }
}
