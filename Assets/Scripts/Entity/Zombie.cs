using UnityEngine;
using System.Collections;

public class Zombie : LivingEntity {
    public NavMeshAgent agent;
    public override void Update()
    {
        SetDestination(Cache.surfCharacter.transform.position);
    }
    public void SetDestination(Vector3 pos)
    {
        agent.SetDestination(pos);
    }
}
