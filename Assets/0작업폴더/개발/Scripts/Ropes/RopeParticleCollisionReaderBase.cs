using System;
using System.Linq;
using UnityEngine;
using Obi;

// refer to Test_ColorParticleCollisions.cs

// uses _rope.solver.OnCollision

public class RopeParticleCollisionReaderBase : MonoBehaviour // may be edited to being a parent class later // edit: it is done
{
    protected ObiRope _rope;
    
    protected bool[] ParticleHasCollision;

    public bool PlayerIsOnRope { get; private set; }
    public event Action PlayerEnteredRope;
    public event Action PlayerExitedRope;

    protected virtual void Awake()
    {
        _rope = gameObject.GetComponent<ObiRope>();
        ParticleHasCollision = new bool[_rope.particleCount];
        
        PlayerIsOnRope = false;
    }

    protected virtual void OnEnable()
    {
        _rope.solver.OnCollision += Solver_OnCollision;
    }

    protected virtual void OnDisable()
    {
        _rope.solver.OnCollision -= Solver_OnCollision;
    }

    private void evalPlayerOnRope()
    {
        bool currentState = (ParticleHasCollision.Count(x => x == true) > 0);
        if (PlayerIsOnRope != currentState)
        {
            if (currentState) PlayerEnteredRope?.Invoke();
            else PlayerExitedRope?.Invoke();
        }
        PlayerIsOnRope = currentState;
    }

    // documentation: https://obi.virtualmethodstudio.com/manual/6.3/scriptingcollisions.html
    protected virtual void Solver_OnCollision(object sender, ObiSolver.ObiCollisionEventArgs e)
    {
        Array.Clear(ParticleHasCollision, 0, ParticleHasCollision.Length);

        var world = ObiColliderWorld.GetInstance();
        foreach (Oni.Contact contact in e.contacts)
        {
            if (contact.distance < 0.01)
            {
                /* do collsion of bodyB */
                var col = world.colliderHandles[contact.bodyB].owner;
                if (col != null && col.CompareTag("Player"))
                {
                    /* do collsion of bodyA particles */
                    int simplexStart = _rope.solver.simplexCounts.GetSimplexStartAndSize(contact.bodyA, out int simplexSize);
                    for (int i = 0; i < simplexSize; ++i)
                    {
                        int particleIndex = _rope.solver.simplices[simplexStart + i];
                        ParticleHasCollision[particleIndex] = true;
                    }
                }
            }
        }

        // evaluate if Player is on _rope
        evalPlayerOnRope();
    }
}
