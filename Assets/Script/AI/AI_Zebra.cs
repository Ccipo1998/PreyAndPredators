using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// all zebra's ai mechanics are handled here
public class AI_Zebra : MonoBehaviour
{
    [SerializeField]
    private float _ReactionTime;

    [SerializeField]
    private MovementMechanic _MovementMechanic;
    [SerializeField]
    private BT_Search _BTSearch;

    private HSM_Zebra _hsm;

    private void OnEnable()
    {
        // init zebra hsm
        _hsm = new HSM_Zebra(_BTSearch);

        StartCoroutine(UpdateHSM());
    }

    private IEnumerator UpdateHSM()
    {
        // for all zebra's life
        while (true)
        {
            _hsm.Update();

            yield return new WaitForSeconds(_ReactionTime);
        }
    }
}
