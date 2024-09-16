using Obi;
using UnityEngine;

public class ZipLinePulleyRopeChecker : MonoBehaviour
{
    [SerializeField] ObiSolver _solver;
    [SerializeField] Rigidbody _zipLinePulley;

    private Vector3 _initialPos;
    private Vector3 _lastFinePos;

    private void Start()
    {
        _initialPos = _zipLinePulley.transform.position;
    }

    private void OnEnable()
    {
        PlayerLogic.PlayerRespawned += PlayerRespawnedAction;
        _solver.OnCollision += Solver_OnCollision;
    }

    private void OnDisable()
    {
        PlayerLogic.PlayerRespawned -= PlayerRespawnedAction;
        _solver.OnCollision -= Solver_OnCollision;
    }

    private void PlayerRespawnedAction()
    {
        _lastFinePos = _initialPos;
    }

    private int _contactCntPrev = 0;
    void Solver_OnCollision(object sender, ObiSolver.ObiCollisionEventArgs e)
    {
        var world = ObiColliderWorld.GetInstance();
        int contactCnt = 0;
        foreach (var contact in e.contacts)
        {
            if (contact.distance < 0.01)
            {
                /* do collsion of bodyB */

                if (contact.bodyB >= world.colliderHandles.Count) break;
                var col = world.colliderHandles[contact.bodyB].owner;

                if (col != null && col.CompareTag("ZipLinePulley RopeChecker"))
                {
                    if (++contactCnt > 1)
                    {
                        if (_contactCntPrev > 0) _lastFinePos = _zipLinePulley.transform.position;
                        break;
                    }
                }
            }
        }
        if (contactCnt == 0) FellFromRopeAction();
        _contactCntPrev = contactCnt;
    }

    private void FellFromRopeAction()
    {
        _zipLinePulley.transform.position = _lastFinePos;
        //_zipLinePulley.velocity = Vector3.zero;
    }
}
