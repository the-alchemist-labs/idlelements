using UnityEngine;

public class ProjectilePrefab : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private int speed;

    private Vector2 direction;
    private string targetTag;

    public void Initialize(Vector2 target, ElementalSkill skill, int statValue, string targetTag)
    {
        this.targetTag = targetTag;
        speed = (int)skill.SkillSpeed;
        damage = statValue * skill.ImpactValue;
        direction = (target - (Vector2)transform.position).normalized;
    }

    void FixedUpdate()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == targetTag)
        {
            if (targetTag == Tags.PartyMember) collision.gameObject.GetComponent<BattleMemberPrefab>().TakeDamage(damage);
            if (targetTag == Tags.Enemy) collision.gameObject.GetComponent<BattleEnemyPrefab>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
