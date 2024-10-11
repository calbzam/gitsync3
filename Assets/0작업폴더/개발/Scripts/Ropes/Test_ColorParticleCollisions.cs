using System;
using UnityEngine;
using Obi;

//[RequireComponent(typeof(ObiParticleRenderer))]
public class Test_ColorParticleCollisions : MonoBehaviour
{
    private ObiRope _rope;
    private ObiInstancedParticleRenderer _renderer;
    //[SerializeField] private ObiContactEventDispatcher contactEventDispatcher;

    private bool[] particleHasCollision;

    private void Awake()
    {
        _rope = gameObject.GetComponent<ObiRope>();
        _renderer = gameObject.GetComponent<ObiInstancedParticleRenderer>();

        particleHasCollision = new bool[_rope.particleCount];

        //contactEventDispatcher = gameObject.GetComponent<ObiContactEventDispatcher>();
    }

    private void OnEnable()
    {
        _rope.solver.OnCollision += Solver_OnCollision;
        //contactEventDispatcher.onContactEnter.AddListener(OnContactEnter);
        //contactEventDispatcher.onContactExit.AddListener(OnContactExit);
    }

    private void OnDisable()
    {
        _rope.solver.OnCollision -= Solver_OnCollision;
        //contactEventDispatcher.onContactEnter.RemoveListener(OnContactEnter);
        //contactEventDispatcher.onContactExit.RemoveListener(OnContactExit);
    }

    private void Start()
    {
        //foreach (var keyword in _renderer.material.shaderKeywords) Debug.Log(keyword);
        //_renderer.material.EnableKeyword("_BASE");
        //Debug.Log(_renderer.material.GetColor("_BaseColor"));
    }

    private void Update()
    {
        SetParticleColorsAll();
    }

    private void SetParticleColorsAll()
    {
        for (int i = 0; i < particleHasCollision.Length && i < _rope.solver.colors.count; ++i)
        {
            //if (particleHasCollision[i])
            //{
            //    _rope.solver.colors[i] = Color.red; // not working with ObiParticleRenderer
            //}
            //else
            //{
            //    _rope.solver.colors[i] = Color.white;
            //}

            if (particleHasCollision[i])
            {
                _renderer.material.SetColor("_BaseColor", Color.red);
                return;
            }
            else
            {
                _renderer.material.SetColor("_BaseColor", Color.white);
                return;
            }
        }
    }

    // documentation: https://obi.virtualmethodstudio.com/manual/6.3/scriptingcollisions.html
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
                    int simplexStart = _rope.solver.simplexCounts.GetSimplexStartAndSize(contact.bodyA, out int simplexSize);
                    for (int i = 0; i < simplexSize; ++i)
                    {
                        int particleIndex = _rope.solver.simplices[simplexStart + i];
                        particleHasCollision[particleIndex] = true;
                    }
                }
            }
        }
    }


    // http://obi.virtualmethodstudio.com/forum/thread-2485-post-7857.html#pid7857
    // but ObiContactEventDispatcher oncontactexit is not working correctly?
    private void OnContactEnter(ObiSolver solver, Oni.Contact contact) // used with contactEventDispatcher (not Solver_OnCollision)
    {
        //rope.solver.colors[contact.bodyA] = Color.red;

        var world = ObiColliderWorld.GetInstance();
        var col = world.colliderHandles[contact.bodyB].owner;
        if (col != null)
        {
            // retrieve the offset and size of the simplex in the solver.simplices array:
            int simplexStart = _rope.solver.simplexCounts.GetSimplexStartAndSize(contact.bodyA, out int simplexSize);

            // starting at simplexStart, iterate over all particles in the simplex:
            for (int i = 0; i < simplexSize; ++i)
            {
                int particleIndex = _rope.solver.simplices[simplexStart + i];
                particleHasCollision[particleIndex] = true;
                _rope.solver.colors[particleIndex] = Color.red;
            }
        }
    }

    private void OnContactExit(ObiSolver solver, Oni.Contact contact) // used with contactEventDispatcher (not Solver_OnCollision)
    {
        //rope.solver.colors[contact.bodyA] = Color.white;

        var world = ObiColliderWorld.GetInstance();
        var col = world.colliderHandles[contact.bodyB].owner;
        if (col != null)
        {
            // retrieve the offset and size of the simplex in the solver.simplices array:
            int simplexStart = _rope.solver.simplexCounts.GetSimplexStartAndSize(contact.bodyA, out int simplexSize);

            // starting at simplexStart, iterate over all particles in the simplex:
            for (int i = 0; i < simplexSize; ++i)
            {
                int particleIndex = _rope.solver.simplices[simplexStart + i];
                particleHasCollision[particleIndex] = false;
                _rope.solver.colors[particleIndex] = Color.white;
            }
        }
    }
}
