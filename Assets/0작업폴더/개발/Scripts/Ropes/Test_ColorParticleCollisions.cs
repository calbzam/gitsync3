using System;
using UnityEngine;
using Obi;

[RequireComponent(typeof(ObiParticleRenderer))]
public class Test_ColorParticleCollisions : MonoBehaviour
{
    private ObiRope rope;

    //[SerializeField] private ObiContactEventDispatcher contactEventDispatcher;

    private bool[] particleHasCollision;

    private void Awake()
    {
        rope = gameObject.GetComponent<ObiRope>();

        particleHasCollision = new bool[rope.particleCount];

        //contactEventDispatcher = gameObject.GetComponent<ObiContactEventDispatcher>();
    }

    private void OnEnable()
    {
        rope.solver.OnCollision += Solver_OnCollision;
        //contactEventDispatcher.onContactEnter.AddListener(OnContactEnter);
        //contactEventDispatcher.onContactExit.AddListener(OnContactExit);
    }

    private void OnDisable()
    {
        rope.solver.OnCollision -= Solver_OnCollision;
        //contactEventDispatcher.onContactEnter.RemoveListener(OnContactEnter);
        //contactEventDispatcher.onContactExit.RemoveListener(OnContactExit);
    }

    private void Start()
    {

    }

    private void Update()
    {
        SetParticleColors();
    }

    private void SetParticleColors()
    {
        for (int i = 0; i < particleHasCollision.Length && i < particleHasCollision.Length; ++i)
        {
            if (particleHasCollision[i])
                rope.solver.colors[i] = Color.red;
            else
                rope.solver.colors[i] = Color.white;
        }
    }

    private void Solver_OnCollision(object sender, ObiSolver.ObiCollisionEventArgs e)
    {
        Array.Clear(particleHasCollision, 0, particleHasCollision.Length);

        var world = ObiColliderWorld.GetInstance();
        foreach (Oni.Contact contact in e.contacts)
        {
            // if this one is an actual collision:
            if (contact.distance < 0.01)
            {
                var col = world.colliderHandles[contact.bodyB].owner;
                if (col != null)
                {
                    // retrieve the offset and size of the simplex in the solver.simplices array:
                    int simplexStart = rope.solver.simplexCounts.GetSimplexStartAndSize(contact.bodyA, out int simplexSize);

                    // starting at simplexStart, iterate over all particles in the simplex:
                    for (int i = 0; i < simplexSize; ++i)
                    {
                        int particleIndex = rope.solver.simplices[simplexStart + i];
                        
                        particleHasCollision[particleIndex] = true;
                        //rope.solver.colors[particleIndex] = Color.white;
                    }
                }
            }
        }
    }


    // ObiContactEventDispatcher oncontactexit not working correctly?
    // http://obi.virtualmethodstudio.com/forum/thread-2485-post-7857.html#pid7857
    private void OnContactEnter(ObiSolver solver, Oni.Contact contact)
    {
        //rope.solver.colors[contact.bodyA] = Color.red;

        var world = ObiColliderWorld.GetInstance();
        var col = world.colliderHandles[contact.bodyB].owner;
        if (col != null)
        {
            // retrieve the offset and size of the simplex in the solver.simplices array:
            int simplexStart = rope.solver.simplexCounts.GetSimplexStartAndSize(contact.bodyA, out int simplexSize);

            // starting at simplexStart, iterate over all particles in the simplex:
            for (int i = 0; i < simplexSize; ++i)
            {
                int particleIndex = rope.solver.simplices[simplexStart + i];
                rope.solver.colors[particleIndex] = Color.red;
            }
        }
    }

    private void OnContactExit(ObiSolver solver, Oni.Contact contact)
    {
        //rope.solver.colors[contact.bodyA] = Color.white;

        var world = ObiColliderWorld.GetInstance();
        var col = world.colliderHandles[contact.bodyB].owner;
        if (col != null)
        {
            // retrieve the offset and size of the simplex in the solver.simplices array:
            int simplexStart = rope.solver.simplexCounts.GetSimplexStartAndSize(contact.bodyA, out int simplexSize);

            // starting at simplexStart, iterate over all particles in the simplex:
            for (int i = 0; i < simplexSize; ++i)
            {
                int particleIndex = rope.solver.simplices[simplexStart + i];
                rope.solver.colors[particleIndex] = Color.white;
            }
        }
    }
}
