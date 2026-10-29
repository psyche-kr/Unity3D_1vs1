using UnityEngine;
using UnityEngine.AI;

public class BossMissile : MonoBehaviour
{
    public Transform target;
    NavMeshAgent nav;

    const float LifeTime = 5f;   // 유도 미사일이 유지되는 시간

    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        Destroy(gameObject, LifeTime);   // 매 프레임 예약하지 않고 한 번만 예약
    }

    void Update()
    {
        if (target == null) return;      // 플레이어가 사라진 경우 방어

        nav.SetDestination(target.position);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PlayerStatus>().TakeDamage(1);
            Destroy(gameObject);
        }
    }
}
