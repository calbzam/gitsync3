using UnityEngine;
using Obi;

// refer to Test_ColorParticleCollisions.cs

// uses ObiContactEventDispatcher

public class Test_WalkingRope : MonoBehaviour
{
    private ObiRope _rope;
    private ObiInstancedParticleRenderer _ropeRenderer;

    [SerializeField] private Transform _marker;
    private SpriteRenderer _markerRenderer;
    private Color _ropeOrigColor, _markerOrigColor;

    private void Awake()
    {
        _rope = gameObject.GetComponent<ObiRope>();
    }

    private void Start()
    {
        _ropeRenderer = gameObject.GetComponent<ObiInstancedParticleRenderer>();
        _ropeRenderer.material.EnableKeyword("_BASE");
        _markerRenderer = _marker.GetComponent<SpriteRenderer>();
        _markerRenderer.material.EnableKeyword("_BASE");

        _ropeOrigColor = _ropeRenderer.material.GetColor("_BaseColor");
        _markerOrigColor = _markerRenderer.material.GetColor("_BaseColor");
    }

    private void OnDisable()
    {
        _ropeRenderer.material.SetColor("_BaseColor", _ropeOrigColor);
        _markerRenderer.material.SetColor("_BaseColor", _markerOrigColor);
    }

    private Vector3 GetGlobalParticlePos(Vector3 particlePosition)
    {
        Vector3 childUpdated = transform.parent.rotation * Vector3.Scale(particlePosition, transform.parent.lossyScale);

        return childUpdated + transform.parent.position;
    }


    public void OnContactStay(ObiSolver solver, Oni.Contact contact) // used with contactEventDispatcher (not Solver_OnCollision)
    {
        _ropeRenderer.material.SetColor("_BaseColor", Color.red);
        _markerRenderer.material.SetColor("_BaseColor", Color.red);

        int simplexStart = _rope.solver.simplexCounts.GetSimplexStartAndSize(contact.bodyA, out int simplexSize);
        int particleIndex = _rope.solver.simplices[simplexStart + simplexSize - 1];
        _marker.position = GetGlobalParticlePos(_rope.solver.positions[particleIndex]);


        //var world = ObiColliderWorld.GetInstance();
        //var col = world.colliderHandles[contact.bodyB].owner;
        //if (col != null)
        //{
        //    // retrieve the offset and size of the simplex in the solver.simplices array:
        //    int simplexStart = _rope.solver.simplexCounts.GetSimplexStartAndSize(contact.bodyA, out int simplexSize);
        //
        //    // starting at simplexStart, iterate over all particles in the simplex:
        //    for (int i = 0; i < simplexSize; ++i)
        //    {
        //        int particleIndex = _rope.solver.simplices[simplexStart + i];
        //        _marker.position = GetGlobalParticlePos(_rope.solver.positions[particleIndex]);
        //        break;
        //    }
        //}
    }

    public void OnContactExit(ObiSolver solver, Oni.Contact contact) // used with contactEventDispatcher (not Solver_OnCollision)
    {
        _ropeRenderer.material.SetColor("_BaseColor", Color.white);
        _markerRenderer.material.SetColor("_BaseColor", Color.white);

        int simplexStart = _rope.solver.simplexCounts.GetSimplexStartAndSize(contact.bodyA, out int simplexSize);
        int particleIndex = _rope.solver.simplices[simplexStart + simplexSize - 1];
        _marker.position = GetGlobalParticlePos(_rope.solver.positions[particleIndex]);


        //int particle = _rope.solver.simplices[contact.bodyA];
        //_marker.position = GetGlobalParticlePos(_rope.solver.positions[particle]); // if enabled, the _marker position keeps swapping between the collided position and the first index of the _rope
    }
}
