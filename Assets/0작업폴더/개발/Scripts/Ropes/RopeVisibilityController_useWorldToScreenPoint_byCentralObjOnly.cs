using System;
using UnityEngine;
using Obi;

public class RopeVisibilityController_useWorldToScreenPoint_byCentralObjOnly : MonoBehaviour
{
    [SerializeField] private ObiUpdater _updater;
    private ObiSolver[] _solvers;

    private void Start()
    {
        _solvers = _updater.solvers.ToArray();
        //EnableRopeIfInView();
    }

    private void Update()
    {
        EnableRopeIfInView();
    }

    private void EnableRopeIfInView()
    {
        foreach (var solver in _solvers)
        {
            float screenPointX = Camera.main.WorldToScreenPoint(solver/*.GetComponentInChildren<ObiRope>()*/.transform.position).x;
            if (0 <= screenPointX && screenPointX <= Screen.width)
            {
                if (!solver.gameObject.activeSelf)
                    solver.gameObject.SetActive(true);
            }
            else
            {
                if (solver.gameObject.activeSelf)
                    solver.gameObject.SetActive(false);
            }
        }
    }
}
