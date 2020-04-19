using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections.ObjectModel;

public class TargetFinder : MonoBehaviour
{
    public Action<GameObject> OnTargetInFieldOfView = delegate { }; // On scan End
    private bool _isScanning;

    public Action OnTargetDeath = delegate { };
    private bool _isTrackingIfTargetAlive;

    private void Awake()
    {
        _isScanning = false;
        _isTrackingIfTargetAlive = false;
    }


    // *********** Hero Taget Alive Tracker ********
    public void StartTrackIfTargetAlive(GameObject target)
    {
        if (_isTrackingIfTargetAlive)
            return;

        _isTrackingIfTargetAlive = true;
        StartCoroutine(TrackIfTargetAlive(target));
    }

    private IEnumerator TrackIfTargetAlive(GameObject target)
    {
        while (_isTrackingIfTargetAlive && target.activeSelf)
        {
            yield return new WaitForSeconds(GlobalCodeSettings.FRAME_RATE);
        }

        _isTrackingIfTargetAlive = false;
        if (!target.activeSelf)
            OnTargetDeath();
    }
    public void stopTackIfTargetAlive()
    {
        //Debug.Log("stopScan");
        _isTrackingIfTargetAlive = false;
    }


    //************ Hero Target Scanner ************* 

    /// <summary>
    /// Start the traking of a the obj target spesific target if its attackable
    /// Author: Ilan
    /// </summary>
    public void startTrackIfATargetAttackable(GameObject target, Skill skill)
    {
        if (_isScanning)
            return;

        _isScanning = true;
        StartCoroutine(trackIfATargetAttackable(target, skill));
    }

    /// <summary>
    /// A tracking function, that lock and target and keep checking if the target is attackable
    /// </summary>
    /// <returns></returns>
    public IEnumerator trackIfATargetAttackable(GameObject target, Skill skill)
    {
        while (target != null && !skill.isTargetAttackable(target) && _isScanning && target.activeSelf)
        {
            yield return new WaitForSeconds(GlobalCodeSettings.FRAME_RATE);
        }
        //_targetsTo######AttackBank.Add()


        if (_isScanning && OnTargetInFieldOfView != null)
        {
            _isScanning = false;
            OnTargetInFieldOfView(target);
            OnTargetInFieldOfView = null;
        }
        _isScanning = false;
    }


    /// <summary>
    /// Start the traking corutine to track the targetes
    /// Author: Ilan
    /// </summary>
    /// <param name="target">The target to be track</param>
    public void startScanningForAnAttackableTarget(ReadOnlyCollection<GameObject> targets, Skill skill)
    {
        //Debug.Log("startScan");
        if (_isScanning)
            return;

        _isScanning = true;
        StartCoroutine(autoScan(targets, skill));
    }

    /// <summary>
    /// Stop the tracking corutine
    /// Author: Ilan
    /// </summary>
    public void stopScannings()
    {
        //Debug.Log("stopScan");
        _isScanning = false;
        OnTargetInFieldOfView = delegate { };
    }

    /// <summary>
    /// Start tracking after the target
    /// Author: Ilan
    /// </summary>
    /// <param name="target">Target to be track</param>
    /// <returns></returns>
    private IEnumerator autoScan(ReadOnlyCollection<GameObject> targets, Skill skill)
    {
        GameObject target = findAnAttackableTarget(targets, skill);

        while (target == null && _isScanning) // if scanning and target not found
        {
            yield return new WaitForSeconds(GlobalCodeSettings.FRAME_RATE);
            target = findAnAttackableTarget(targets, skill);

            if (targets.Count <= 0) // in case that there are no more targets
                _isScanning = false;
        }

        //targetsToAttack.Remove(target);
        //targetsToAttack.Insert(0, target);


        if (_isScanning && target != null && OnTargetInFieldOfView != null)
        {
            _isScanning = false;
            OnTargetInFieldOfView(target);
            OnTargetInFieldOfView = delegate { };
        }
        _isScanning = false;
    }

    /// <summary>
    /// Search if there an attackable target within the targetbank
    /// </summary>
    /// <returns></returns>
    public GameObject findAnAttackableTarget(ReadOnlyCollection<GameObject> targets, Skill skill)
    {
        foreach (GameObject target in targets)
        {
            if (target != null && skill.isTargetAttackable(target))
                return target;
        }

        return null;
    }
}
