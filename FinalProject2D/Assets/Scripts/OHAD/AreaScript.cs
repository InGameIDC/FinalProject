using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaScript : MonoBehaviour
{
    private List<GameObject> objectsInField;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject unit = collision.transform.gameObject;
        if (unit.tag.Equals("HeroDamageHitArea"))
        {
            GameObject targetParentUnit = unit.transform.parent.gameObject;
            unit = targetParentUnit;
            Debug.Log(unit.name);
            //objectsInField.Add(unit);
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
            //objectsInField.Remove(unit);
            unit.GetComponent<Movment2D>().ChangeSpeed(1f);
        }

        
    }
}
