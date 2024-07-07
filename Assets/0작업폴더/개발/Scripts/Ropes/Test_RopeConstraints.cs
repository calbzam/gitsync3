using System;
using UnityEngine;
using Obi;

public class Test_RopeConstraints : MonoBehaviour
{
    private ObiRope rope;
    [SerializeField] private ObiColliderBase colliderA;
    [SerializeField] private ObiColliderBase colliderB;

    private void Awake()
    {
        rope = gameObject.GetComponent<ObiRope>();
    }

    private void Start()
    {
        SetEndConstraints();
    }

    // Scripting constraints http://obi.virtualmethodstudio.com/manual/6.3/scriptingconstraints.html
    private void SetEndConstraints()
    {
        // get a hold of the constraint type we want, in this case, pin constraints:
        var pinConstraints = rope.GetConstraintsByType(Oni.ConstraintType.Pin) as ObiConstraints<ObiPinConstraintsBatch>;

        // remove all batches from it, so we start clean:
        pinConstraints.Clear();

        // create a new pin constraints batch
        var batch = new ObiPinConstraintsBatch();

        // Add a couple constraints to it, pinning the first and last particles in the rope:
        //batch.AddConstraint(rope.solverIndices[0], colliderA, Vector3.zero, Quaternion.identity, 0, 0, float.PositiveInfinity);
        batch.AddConstraint(rope.solverIndices[rope.blueprint.activeParticleCount - 1], colliderB, Vector3.zero, Quaternion.identity, 0, 0, float.PositiveInfinity);

        // set the amount of active constraints in the batch to 2 (the ones we just added).
        batch.activeConstraintCount = 1;

        // append the batch to the pin constraints:
        pinConstraints.AddBatch(batch);

        // this will cause the solver to rebuild pin constraints at the beginning of the next frame:
        rope.SetConstraintsDirty(Oni.ConstraintType.Pin);
    }
}
