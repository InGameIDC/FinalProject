using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaScript : MonoBehaviour
{
    private List<GameObject> objectsInField;
    public float _hitRadius;
    public float shootDamege;
    public int timeToDie;
    public GameObject am; // AreaManager

    // Start is called before the first frame update
    void Start()
    {
        List<GameObject> objectOnStart = new List<GameObject>();
        am = GameObject.FindGameObjectWithTag("AreaManager");
        am.GetComponent<AreaManager>().AddedArea();

        Collider2D[] damageHitAreasInSphere = damageHitAreasInSphere =
            Physics2D.OverlapCircleAll(transform.position, _hitRadius, LayerMask.GetMask("DamageHitArea"));
        foreach (Collider2D damageHitArea in damageHitAreasInSphere)
        {
            GameObject unit = damageHitArea.transform.parent.gameObject;
            //Damage the enemy
            if (!objectOnStart.Contains(unit) && unit.tag == "EnemyUnit")
            {
                //on hitting - the health is lowered
                unit.GetComponentInChildren<Health>().TakeDamage(shootDamege);

                // After hitting the object, add it to a "damaged list" so it won't be hit again.
                objectOnStart.Add(unit);
                //GetComponent<Collider2D>().isTrigger = false; // turn off the trigger (can't use the same bullet twice)
            }
        }

        StartCoroutine(waitToDestroy());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator waitToDestroy()
    {

        yield return new WaitForSeconds(timeToDie);

        am.GetComponent<AreaManager>().RemovedArea();
        Destroy(gameObject);

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject unit = collision.transform.gameObject;
        if (unit.tag.Equals("HeroDamageHitArea"))
        {
            GameObject targetParentUnit = unit.transform.parent.gameObject;
            unit = targetParentUnit;
            Debug.Log(unit.name);
            objectsInField.Add(unit);
            unit.GetComponent<Movment2D>().ChangeSpeed(0.5f);

        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject unit = collision.transform.gameObject;
        if (unit.tag.Equals("HeroDamageHitArea"))
        {
            GameObject targetParentUnit = unit.transform.parent.gameObject;
            unit = targetParentUnit;
            objectsInField.Remove(unit);
            unit.GetComponent<Movment2D>().ChangeSpeed(1f);
        }

    }
}
