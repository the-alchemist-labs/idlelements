using System;
using System.Collections;
using UnityEngine;

public class SkillEffectPrefab : MonoBehaviour
{
    private const float EFFECT_TTL = 3f;

    public event Action<GameObject> OnEffectCompleted;

    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Animator animator;

    [SerializeField] private int damage;
    [SerializeField] private int speed;

    private Vector2 _direction;
    private string _targetTag;
    private bool _hasHit;

    public void Initialize(Vector2 target, Skill skill, int power, string targetTag)
    {
        SoundManager.Instance.PlaySkillSFX(skill.Id);
        animator.enabled = false;
        _hasHit = false;

        sprite.sprite = Resources.Load<Sprite>($"Sprites/Skills/{skill.Id}");
        _targetTag = targetTag;
        speed = (int)skill.SkillSpeed;
        damage = power * skill.ImpactValue;
        _direction = (target - (Vector2)transform.position).normalized;

        StartCoroutine(RemoveSkillEffect(EFFECT_TTL));
    }

    void FixedUpdate()
    {
        if (!_hasHit)
        {
            transform.Translate(_direction * speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == _targetTag)
        {
            if (_targetTag == Tags.PartyMember) collision.gameObject.GetComponent<BattleMemberPrefab>().TakeDamage(damage);
            if (_targetTag == Tags.Enemy) collision.gameObject.GetComponent<BattleEnemyPrefab>().TakeDamage(damage);
            Explode();
        }
    }

    private void Explode()
    {
        AnimationClip clip = animator.runtimeAnimatorController.animationClips[0];
        _hasHit = true;
        animator.enabled = true;
        animator.Play(clip.name);
        StartCoroutine(RemoveSkillEffect(clip.length));
    }

    private IEnumerator RemoveSkillEffect(float delay)
    {
        yield return new WaitForSeconds(delay);
        OnEffectCompleted?.Invoke(gameObject);
    }
}
