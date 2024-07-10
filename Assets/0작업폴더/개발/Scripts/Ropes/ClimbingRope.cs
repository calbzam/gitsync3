using System;
using UnityEngine;
using Obi;
using UnityEngine.InputSystem;

public class ClimbingRope : MonoBehaviour
{
    private InputControls _input;
    private ObiRope _rope;

    private ObiCollider2D _player;
    private ObiCollider2D _playerRopeRider;

    private bool _ropeAttached = false;
    private bool _ropeJumped = false;

    private int _currentParticle = -1;
    private ObiPinConstraintsBatch _playerBatch;

    private float _jumpedEnoughDistance;

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
        _playerRopeRider = GameObject.FindGameObjectWithTag("Player ropeRider").GetComponent<ObiCollider2D>();
        _jumpedEnoughDistance = _player.GetComponent<PlayerController>().Stats.RopeJumpedDistance;
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

    private void FixedUpdate()
    {
        HandleRopeClimb();
    }

    private void EnableRopeCollision(bool enabled)
    {
        _rope.solver.particleCollisionConstraintParameters.enabled = enabled;
        _rope.solver.collisionConstraintParameters.enabled = enabled;
    }

    private Vector3 getGlobalParticlePos(Vector3 particlePosition)
    {
        Vector3 childUpdated = transform.parent.rotation * Vector3.Scale(particlePosition, transform.parent.lossyScale);

        return childUpdated + transform.parent.position;
    }

    private int getIndexInActor(int particle)
    {
        return _rope.solver.particleToActor[particle].indexInActor;
    }

    // indexInActor: https://obi.virtualmethodstudio.com/forum/thread-4019-post-14919.html#pid14919
    private void HandleRopeClimb()
    {
        if (!_ropeAttached) return;

        if (InputReader.FrameInput.Move.y > 0)
        {
            int indexInActor = getIndexInActor(_currentParticle);
            if (indexInActor - 1 > 0 /* first particle in visible rope */)
            {
                detachPlayerFromParticle(_currentParticle);
                int prevParticle = _rope.solverIndices[indexInActor - 1];
                _player.transform.position = getGlobalParticlePos(_rope.solver.positions[prevParticle]);
                attachPlayerToParticle(prevParticle);
            }
        }
        else if (InputReader.FrameInput.Move.y < 0)
        {
            int indexInActor = getIndexInActor(_currentParticle);
            if (indexInActor + 1 < _rope.elements.Count + 1 /* total number of particles in visible rope */)
            {
                detachPlayerFromParticle(_currentParticle);
                int nextParticle = _rope.solverIndices[indexInActor + 1];
                _player.transform.position = getGlobalParticlePos(_rope.solver.positions[nextParticle]);
                attachPlayerToParticle(nextParticle);
            }
        }
    }

    private void Solver_OnCollision(object sender, ObiSolver.ObiCollisionEventArgs e)
    {
        CheckRopePlayerDistance();
        //Debug.Log(_ropeAttached + ", " + _ropeJumped); // start from: false, false

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
                    int particle = _rope.solver.simplices[contact.bodyA];
                    attachPlayerToParticle(particle);
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
            _ropeJumped = true;
            detachPlayerFromParticle(_currentParticle);
            EnableRopeCollision(false);
        }
    }

    private void CheckRopePlayerDistance()
    {
        if (_ropeAttached && _ropeJumped)
        {
            Vector3 particlePos = getGlobalParticlePos(_rope.solver.positions[_currentParticle]);
            bool awayFromRope = Vector2.Distance(particlePos, _player.transform.position) > _jumpedEnoughDistance;

            if (awayFromRope)
            {
                _ropeAttached = _ropeJumped = false;
                EnableRopeCollision(true);
                _currentParticle = -1;
            }
        }
    }

    // Scripting constraints http://obi.virtualmethodstudio.com/manual/6.3/scriptingconstraints.html

    private void initPlayerBatch()
    {
        var pinConstraints = _rope.GetConstraintsByType(Oni.ConstraintType.Pin) as ObiConstraints<ObiPinConstraintsBatch>;
        pinConstraints.Clear(); // remove all batches from the constraint type we want, so we start clean:

        // add a new pin constraints batch
        _playerBatch = new ObiPinConstraintsBatch();
        pinConstraints.AddBatch(_playerBatch);
    }

    private void attachPlayerToParticle(int toParticle)
    {
        initPlayerBatch();

        _playerBatch.AddConstraint(toParticle, _playerRopeRider, Vector3.zero, Quaternion.identity, 0, 0, float.PositiveInfinity);
        _playerBatch.activeConstraintCount = 1;

        // this will cause the solver to rebuild pin constraints at the beginning of the next frame:
        _rope.SetConstraintsDirty(Oni.ConstraintType.Pin);
        _currentParticle = toParticle;
    }

    private void detachPlayerFromParticle(int fromParticle)
    {
        var pinConstraints = _rope.GetConstraintsByType(Oni.ConstraintType.Pin) as ObiConstraints<ObiPinConstraintsBatch>;
        pinConstraints.RemoveBatch(_playerBatch);

        _rope.SetConstraintsDirty(Oni.ConstraintType.Pin);
    }

    private void repinPlayerToParticle(int fromParticle, int toParticle)
    {
        _playerBatch.RemoveConstraint(fromParticle);

        _playerBatch.AddConstraint(toParticle, _player, Vector3.zero, Quaternion.identity, 0, 0, float.PositiveInfinity);
        _playerBatch.activeConstraintCount = 1;

        _rope.SetConstraintsDirty(Oni.ConstraintType.Pin);
        _currentParticle = toParticle;
    }
}
