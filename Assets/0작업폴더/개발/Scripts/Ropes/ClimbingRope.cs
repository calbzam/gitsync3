using System;
using UnityEngine;
using Obi;
using UnityEngine.InputSystem;

public class ClimbingRope : MonoBehaviour
{
    private InputControls _input;
    private ObiRope _rope;
    private ObiCollider2D _player;

    private bool _ropeAttached = false;
    private bool _ropeJumped = false;
    private int _attachedParticle;
    private ObiPinConstraintsBatch _playerBatch = null;

    private float _jumpedEnoughDistance = 5f;

    //private bool[] particleHasCollision;

    private void Awake()
    {
        _input = new InputControls();
        _rope = gameObject.GetComponent<ObiRope>();

        //particleHasCollision = new bool[rope.particleCount];
    }

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<ObiCollider2D>();
    }

    private void OnEnable()
    {
        _input.Enable();
        _rope.solver.OnCollision += Solver_OnCollision;
        _input.Player.Jump.started += DisconnectPlayer;
    }

    private void OnDisable()
    {
        _input.Disable();
        _rope.solver.OnCollision -= Solver_OnCollision;
        _input.Player.Jump.started -= DisconnectPlayer;
    }

    //private void Start()
    //{

    //}

    //private void Update()
    //{

    //}

    private void Solver_OnCollision(object sender, ObiSolver.ObiCollisionEventArgs e)
    {
        CheckRopePlayerDistance();

        if (_ropeAttached) return;

        var world = ObiColliderWorld.GetInstance();
        foreach (var contact in e.contacts)
        {
            if (contact.distance < 0.01)
            {
                /* do collsion of bodyB */
                var col = world.colliderHandles[contact.bodyB].owner;
                if (col != null && col.tag.Equals("Player"))
                {
                    /* do collsion of bodyA particles */
                    _attachedParticle = _rope.solver.simplices[contact.bodyA];
                    attachToParticle(_attachedParticle, col);
                    //col.GetComponent<PlayerController>().RopeAttached(true);
                    _ropeAttached = true;
                    break;
                }
            }
        }
    }

    private void DisconnectPlayer(InputAction.CallbackContext ctx)
    {
        if (_ropeAttached)
        {
            detachFromParticle(_player);
            _ropeJumped = true;

            _rope.solver.particleCollisionConstraintParameters.enabled = false;
            _rope.solver.collisionConstraintParameters.enabled = false;
        }
    }

    private void CheckRopePlayerDistance()
    {
        if (_ropeAttached && _ropeJumped)
        {
            Vector3 particlePos = _rope.solver.positions[_attachedParticle];

            //RaycastHit2D hit = Physics2D.Raycast(particlePos, _player.transform.position - particlePos, _jumpedEnoughDistance);
            Ray ray = new Ray(particlePos, _player.transform.position - particlePos);
            bool hit = _rope.solver.Raycast(ray, out QueryResult hitInfo, 0, _jumpedEnoughDistance);

            if (!hit) // finally jumped enough distance away from rope
            {
                _ropeAttached = _ropeJumped = false;

                _rope.solver.particleCollisionConstraintParameters.enabled = true;
                _rope.solver.collisionConstraintParameters.enabled = true;
            }
        }
    }

    // Scripting constraints http://obi.virtualmethodstudio.com/manual/6.3/scriptingconstraints.html
    private void attachToParticle(int particleIndex, ObiColliderBase obiCollider)
    {
        // remove all batches from the constraint type we want, so we start clean:
        var pinConstraints = _rope.GetConstraintsByType(Oni.ConstraintType.Pin) as ObiConstraints<ObiPinConstraintsBatch>;
        pinConstraints.Clear();

        // add a new pin constraints batch
        _playerBatch = new ObiPinConstraintsBatch();
        _playerBatch.AddConstraint(particleIndex, obiCollider, Vector3.zero, Quaternion.identity, 0, 0, float.PositiveInfinity);
        _playerBatch.activeConstraintCount = 1;
        pinConstraints.AddBatch(_playerBatch);

        // this will cause the solver to rebuild pin constraints at the beginning of the next frame:
        _rope.SetConstraintsDirty(Oni.ConstraintType.Pin);
    }

    private void detachFromParticle(ObiColliderBase obiCollider)
    {
        // remove all batches from the constraint type we want, so we start clean:
        var pinConstraints = _rope.GetConstraintsByType(Oni.ConstraintType.Pin) as ObiConstraints<ObiPinConstraintsBatch>;
        pinConstraints.Clear();

        // add a new pin constraints batch
        pinConstraints.RemoveBatch(_playerBatch);
        //Destroy(batch);

        // this will cause the solver to rebuild pin constraints at the beginning of the next frame:
        _rope.SetConstraintsDirty(Oni.ConstraintType.Pin);
    }
}
