using System;
using UnityEngine;
using Obi;

public class ClimbingRope : MonoBehaviour
{
    private ObiRope rope;

    private bool[] particleHasCollision;

    private void Awake()
    {
        rope = gameObject.GetComponent<ObiRope>();

        particleHasCollision = new bool[rope.particleCount];
    }

    private void OnEnable()
    {
        rope.solver.OnCollision += Solver_OnCollision;
    }

    private void OnDisable()
    {
        rope.solver.OnCollision -= Solver_OnCollision;
    }

    private void Start()
    {

    }

    private void Update()
    {

    }

    // How to attach a GameObject to a rope from script? http://obi.virtualmethodstudio.com/forum/thread-2974.html
    // Scripting constraints http://obi.virtualmethodstudio.com/manual/6.3/scriptingconstraints.html
    private void Solver_OnCollision(object sender, ObiSolver.ObiCollisionEventArgs e)
    {
        Array.Clear(particleHasCollision, 0, particleHasCollision.Length);

        var world = ObiColliderWorld.GetInstance();
        foreach (Oni.Contact contact in e.contacts)
        {
            if (contact.distance < 0.01)
            {
                /* do collsion of bodyB */
                var col = world.colliderHandles[contact.bodyB].owner;
                if (col != null)
                {
                    /* do collsion of bodyA particles */
                    int simplexStart = rope.solver.simplexCounts.GetSimplexStartAndSize(contact.bodyA, out int simplexSize);

                    for (int i = 0; i < simplexSize; ++i)
                    {
                        int particleIndex = rope.solver.simplices[simplexStart + i];
                        particleHasCollision[particleIndex] = true;
                    }
                }
            }
        }
    }
}
