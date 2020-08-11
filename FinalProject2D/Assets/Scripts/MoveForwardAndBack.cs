using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForwardAndBack : MonoBehaviour
{
    [SerializeField] float duration = 1f;
    float timeMovmentStart = 0f;
    Vector3 originalPos;
    [SerializeField] float speed = 1f;

    private void Start()
    {
        Skill skill = GetComponent<Skill>();
        skill.OnAttack += MoveUnitForwardAndBack;
        duration = skill.GetSkillCooldown() * 0.5f;
    }

    public void MoveUnitForwardAndBack(GameObject unit, Vector3 direction)
    {
        originalPos = unit.transform.position;
        timeMovmentStart = Time.time;
        StartCoroutine(move(unit, direction));
    }

    private IEnumerator move(GameObject unit, Vector3 direction)
    {
        Vector3 newPos;
        // move forward
        while((Time.time - timeMovmentStart) < duration / 2f)
        {
            newPos = unit.transform.position + direction * speed * Time.deltaTime;
            unit.transform.position = newPos;
            yield return new WaitForFixedUpdate();
        }
        // move back
        while ((Time.time - timeMovmentStart) < duration)
        {
            newPos = unit.transform.position - direction * speed * Time.deltaTime;
            unit.transform.position = newPos;
            yield return new WaitForFixedUpdate();
        }
        // set to original pos
        unit.transform.position = originalPos;
    }
}
