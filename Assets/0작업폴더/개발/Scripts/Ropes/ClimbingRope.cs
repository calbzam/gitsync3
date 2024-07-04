using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;
using UnityEngine.Events;
using Unity.Collections.LowLevel.Unsafe;

public class ClimbingRope : MonoBehaviour
{
    private ObiRope rope;

    private int firstParticle, lastParticle;
    private Vector3 firstPos, lastPos;

    private Vector3[] particlePositions;

    [SerializeField] private ObiContactEventDispatcher contactEventDispatcher;

    private void Awake()
    {
        rope = gameObject.GetComponent<ObiRope>();
        //contactEventDispatcher = gameObject.GetComponent<ObiContactEventDispatcher>();
    }

    private void OnEnable()
    {
        //rope.solver.OnCollision += Solver_OnCollision;
        contactEventDispatcher.onContactEnter.AddListener(OnContactEnter);
        contactEventDispatcher.onContactExit.AddListener(OnContactExit);
    }

    private void OnDisable()
    {
        //rope.solver.OnCollision -= Solver_OnCollision;
        contactEventDispatcher.onContactEnter.RemoveListener(OnContactEnter);
        contactEventDispatcher.onContactExit.RemoveListener(OnContactExit);
    }

    private void Start()
    {
        particlePositions = new Vector3[rope.elements.Count + 1];

        GetEndPositions();

        Debug.Log(firstParticle);
        Debug.Log(lastParticle);
    }

    private void Update()
    {
        GetParticlePositions();

        //LogParticlePositions();
    }

    // ObiContactEventDispatcher oncontactexit not working correctly?
    // http://obi.virtualmethodstudio.com/forum/thread-2485-post-7857.html#pid7857
    private void OnContactEnter(ObiSolver solver, Oni.Contact contact)
    {
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


        //rope.solver.colors[contact.bodyA] = Color.red;
        //rope.solver.colors[contact.bodyB] = Color.red;


        //// retrieve the offset and size of the simplex in the solver.simplices array:
        //int simplexStart = rope.solver.simplexCounts.GetSimplexStartAndSize(contact.bodyA, out int simplexSize);
        //
        //// starting at simplexStart, iterate over all particles in the simplex:
        //for (int i = 0; i < simplexSize; ++i)
        //{
        //    int particleIndex = rope.solver.simplices[simplexStart + i];
        //    rope.solver.colors[particleIndex] = Color.red;
        //}
    }


    //public void OnExit(ObiSolver solver, Oni.Contact c)
    //{
    //    var col = ObiColliderWorld.GetInstance().colliderHandles[c.other].owner;

    //    var blinker = col.GetComponent<Blinker>();
    //    blinker.highlightColor = Color.red;
    //    blinker.Blink();
    //}
    private void OnContactExit(ObiSolver solver, Oni.Contact contact)
    {
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



        //rope.solver.colors[contact.bodyA] = Color.white;
        //rope.solver.colors[contact.bodyB] = Color.white;


        //// retrieve the offset and size of the simplex in the solver.simplices array:
        //int simplexStart = rope.solver.simplexCounts.GetSimplexStartAndSize(contact.bodyA, out int simplexSize);
        //
        //// starting at simplexStart, iterate over all particles in the simplex:
        //for (int i = 0; i < simplexSize; ++i)
        //{
        //    int particleIndex = rope.solver.simplices[simplexStart + i];
        //    rope.solver.colors[particleIndex] = Color.white;
        //}
    }


    //ObiList<Oni.Contact> prevContacts;
    //private void Solver_OnCollision(object sender, ObiSolver.ObiCollisionEventArgs e)
    //{
    //    if (prevContacts != null)
    //    {
    //        if (prevContacts.Count != e.contacts.Count) Debug.Log(prevContacts.Count);
    //        foreach (Oni.Contact contact in prevContacts)
    //        {
    //            //Debug.Log(contact.bodyA);
    //            if (!e.contacts.Contains(contact))
    //            {
    //                Debug.Log("gone");
    //                rope.solver.colors[contact.bodyA] = Color.white;
    //            }
    //        }
    //    }

    //    //var world = ObiColliderWorld.GetInstance();
    //    // just iterate over all contacts in the current frame:
    //    foreach (Oni.Contact contact in e.contacts)
    //    {
    //        // retrieve the offset and size of the simplex in the solver.simplices array:
    //        int simplexStart = rope.solver.simplexCounts.GetSimplexStartAndSize(contact.bodyA, out int simplexSize); // simplexSize = 2
    //        rope.solver.colors[contact.bodyA] = Color.red;

    //        //// starting at simplexStart, iterate over all particles in the simplex:
    //        //for (int i = 0; i < simplexSize; ++i)
    //        //{
    //        //    int particleIndex = rope.solver.simplices[simplexStart + i];
    //        //    rope.solver.colors[particleIndex] = Color.red;
    //        //}

    //        //// if this one is an actual collision:
    //        //if (contact.distance < 0.01)
    //        //{
    //        //    ObiColliderBase col = world.colliderHandles[contact.bodyB].owner;
    //        //    if (col != null)
    //        //    {
    //        //        // do something with the collider.
    //        //    }
    //        //}
    //    }

    //    prevContacts = e.contacts;
    //}

    private void GetEndPositions()
    {
        // first particle in the rope is the first particle of the first element:
        // last particle in the rope is the second particle of the last element:
        firstParticle = rope.elements[0].particle1;
        lastParticle = rope.elements[rope.elements.Count - 1].particle2;

        // now get their positions (expressed in solver space):
        firstPos = rope.solver.positions[firstParticle];
        lastPos = rope.solver.positions[lastParticle];
    }

    private void GetParticlePositions()
    {
        particlePositions[0] = firstPos;
        for (int i = 0; i < rope.elements.Count; ++i)
        {
            particlePositions[i + 1] = rope.solver.positions[rope.elements[i].particle2];
        }
    }

    private void LogParticlePositions()
    {
        ClearConsole.ClearLog();
        foreach (var pos in particlePositions)
        {
            Debug.Log(pos);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log(other);
    }
}
