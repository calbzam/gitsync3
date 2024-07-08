using System;
using UnityEngine;
using Obi;

public class ClimbingRope : MonoBehaviour
{
    private ObiRope rope;

    //private bool[] particleHasCollision;

    private void Awake()
    {
        rope = gameObject.GetComponent<ObiRope>();

        //particleHasCollision = new bool[rope.particleCount];
    }

    private void OnEnable()
    {
        rope.solver.OnCollision += Solver_OnCollision;
    }

    private void OnDisable()
    {
        rope.solver.OnCollision -= Solver_OnCollision;
    }

    //private void Start()
    //{

    //}

    //private void Update()
    //{

    //}

    private void Solver_OnCollision(object sender, ObiSolver.ObiCollisionEventArgs e)
    {
        var world = ObiColliderWorld.GetInstance();
        var contact = e.contacts[0];
        if (contact.distance < 0.01)
        {
            /* do collsion of bodyB */
            var col = world.colliderHandles[contact.bodyB].owner;
            if (col.tag.Equals("Player") && col != null)
            {
                /* do collsion of bodyA particles */
                attachToParticle(rope.solver.simplices[contact.bodyA], col);
                //col.GetComponent<PlayerController>().DisableYVelocity();
            }
        }
    }

    // Scripting constraints http://obi.virtualmethodstudio.com/manual/6.3/scriptingconstraints.html
    private void attachToParticle(int particleIndex, ObiColliderBase obiCollider)
    {
        // get a hold of the constraint type we want, in this case, pin constraints:
        var pinConstraints = rope.GetConstraintsByType(Oni.ConstraintType.Pin) as ObiConstraints<ObiPinConstraintsBatch>;

        // remove all batches from it, so we start clean:
        pinConstraints.Clear();

        // create a new pin constraints batch
        var batch = new ObiPinConstraintsBatch();

        // Add a couple constraints to it, pinning the first and last particles in the rope:
        batch.AddConstraint(particleIndex, obiCollider, Vector3.zero, Quaternion.identity, 0, 0, float.PositiveInfinity);

        // set the amount of active constraints in the batch to 2 (the ones we just added).
        batch.activeConstraintCount = 1;

        // append the batch to the pin constraints:
        pinConstraints.AddBatch(batch);

        // this will cause the solver to rebuild pin constraints at the beginning of the next frame:
        rope.SetConstraintsDirty(Oni.ConstraintType.Pin);
    }
}
