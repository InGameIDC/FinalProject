using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private GameObject _selectArrow;
    [SerializeField] private Vector3 SelectArrowOffset;
    [SerializeField] private GameObject _walkArrow;
    [SerializeField] private Vector3 WalkArrowOffset;
    [SerializeField] private GameObject _attackArrow;
    [SerializeField] private Vector3 AttackArrowOffset;

    public void SelectUnitInteraction(GameObject unit)
    {
        _selectArrow.SetActive(true);
        _selectArrow.transform.position = unit.transform.position + SelectArrowOffset;
        _selectArrow.transform.parent = unit.transform;
    }

    public void WalkInteraction(Vector3 pos)
    {
        _walkArrow.SetActive(true);
        _walkArrow.transform.position = pos + WalkArrowOffset;
    }

    public void AttackUnitInteraction(GameObject unit)
    {
        _attackArrow.SetActive(true);
        _attackArrow.transform.position = unit.transform.position + AttackArrowOffset;
        _attackArrow.transform.parent = unit.transform;
    }

    public void StopSelectUnitInteraction()
    {
        _selectArrow.SetActive(true);
    }

    public void StopWalkInteraction()
    {
        _walkArrow.SetActive(false);
    }

    public void StopAttackUnitInteraction()
    {
        _attackArrow.SetActive(false);
    }
}
