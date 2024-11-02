using System;
using System.Linq;
using UnityEngine;
using Obi;

// refer to Test_ColorParticleCollisions.cs

// uses ObiContactEventDispatcher

// PlayerIsOnRope evaluation doesn't work well on Player exit with OnContactExit

public class RopeParticleCollisionReaderBase_ObiContactEventDispatcher : MonoBehaviour // may be edited to being a parent class later // edit: it is done
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

    public void OnContactStay(ObiSolver solver, Oni.Contact contact) // used with contactEventDispatcher (not Solver_OnCollision)
    {
        var col = ObiColliderWorld.GetInstance().colliderHandles[contact.bodyB].owner;
        if (col != null && col.CompareTag(Tags.PlayerTag))
        {
            /* do collsion of bodyA particles */
            int simplexStart = _rope.solver.simplexCounts.GetSimplexStartAndSize(contact.bodyA, out int simplexSize);
            for (int i = 0; i < simplexSize; ++i)
            {
                int particleIndex = _rope.solver.simplices[simplexStart + i];
                ParticleHasCollision[particleIndex] = true;
            }
        }

        // evaluate if Player is on _rope
        evalPlayerOnRope();
    }

    public void OnContactExit(ObiSolver solver, Oni.Contact contact) // used with contactEventDispatcher (not Solver_OnCollision)
    {
        var col = ObiColliderWorld.GetInstance().colliderHandles[contact.bodyB].owner;
        if (col != null && col.CompareTag(Tags.PlayerTag))
        {
            /* do collsion of bodyA particles */
            int simplexStart = _rope.solver.simplexCounts.GetSimplexStartAndSize(contact.bodyA, out int simplexSize);
            for (int i = 0; i < simplexSize; ++i)
            {
                int particleIndex = _rope.solver.simplices[simplexStart + i];
                ParticleHasCollision[particleIndex] = true;
            }
        }

        // evaluate if Player is on _rope
        evalPlayerOnRope();
    }
}
