using System;
using System.Collections;
using UnityEngine;

public class SkillEffectPrefab : MonoBehaviour
{
    private const float EFFECT_TTL = 3f;

    public event Action<GameObject> OnEffectCompleted;

    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private int _damage;
    [SerializeField] private int _speed;

    private Vector2 _direction;
    private string _targetTag;

    public void Initialize(Vector2 target, ElementalSkill skill, int power, string targetTag)
    {
        _sprite.sprite = Resources.Load<Sprite>($"Sprites/Skills/{skill.Id}");
        _targetTag = targetTag;
        _speed = (int)skill.SkillSpeed;
        _damage = power * skill.ImpactValue;
        _direction = (target - (Vector2)transform.position).normalized;
        StartCoroutine(RemoveSkillEffect());
    }

    void FixedUpdate()
    {
        transform.Translate(_direction * _speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == _targetTag)
        {
            if (_targetTag == Tags.PartyMember) collision.gameObject.GetComponent<BattleMemberPrefab>().TakeDamage(_damage);
            if (_targetTag == Tags.Enemy) collision.gameObject.GetComponent<BattleEnemyPrefab>().TakeDamage(_damage);

            OnEffectCompleted?.Invoke(gameObject);
        }
    }

    private IEnumerator RemoveSkillEffect()
    {
        yield return new WaitForSeconds(EFFECT_TTL);
        OnEffectCompleted?.Invoke(gameObject);
    }
}
