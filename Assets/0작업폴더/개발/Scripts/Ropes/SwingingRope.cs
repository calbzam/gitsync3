using System;
using UnityEngine;
using Obi;

public class SwingingRope : RidableObject
{
    private ObiRope _rope;
    private static float _jumpedEnoughDistance;

    [Help("Particle 0 is mostly the first (top) particle")]
    [SerializeField] private int _noClimbingParticlesUntil = 4;

    public override event Action<int, bool> PlayerOnThisObject;
    public bool PlayerIsInRange { get; set; }

    private int _currentParticle = -1;
    private ObiPinConstraintsBatch _playerBatch;

    //private bool[] particleHasCollision;

    private void Awake()
    {
        _rope = gameObject.GetComponent<ObiRope>();
        //particleHasCollision = new bool[rope.particleCount];
    }

    protected override void Start()
    {
        base.Start();
        PlayerIsInRange = false;
        _jumpedEnoughDistance = PlayerLogic.Player.Stats.RopeJumpedDistance;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _rope.solver.OnCollision += Solver_OnCollision;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _rope.solver.OnCollision -= Solver_OnCollision;
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

    private int getIndexInActor(int particle)
    {
        return _rope.solver.particleToActor[particle].indexInActor;
    }

    // indexInActor: https://obi.virtualmethodstudio.com/forum/thread-4019-post-14919.html#pid14919
    private void HandleRopeClimb()
    {
        if (!PlayerIsInRange) return;

        if (FrameInputReader.FrameInput.InputDir.y > 0)
        {
            if (!PlayerIsAttached) PlayerIsAttached = true;
            int indexInActor = getIndexInActor(_currentParticle);
            if (indexInActor - 1 > 0 /* first particle in visible rope */)
            {
                detachPlayerFromParticle(_currentParticle);
                int prevParticle = _rope.solverIndices[indexInActor - 1];
                PlayerLogic.PlayerObiCol.transform.position = RopeCalcs.GetGlobalParticlePos(transform, _rope.solver.positions[prevParticle]);
                attachPlayerToParticle(prevParticle);
            }
        }
        else if (FrameInputReader.FrameInput.InputDir.y < 0)
        {
            if (!PlayerIsAttached) PlayerIsAttached = true;
            int indexInActor = getIndexInActor(_currentParticle);
            if (indexInActor + 1 < _rope.elements.Count + 1 /* total number of particles in visible rope */)
            {
                detachPlayerFromParticle(_currentParticle);
                int nextParticle = _rope.solverIndices[indexInActor + 1];
                PlayerLogic.PlayerObiCol.transform.position = RopeCalcs.GetGlobalParticlePos(transform, _rope.solver.positions[nextParticle]);
                attachPlayerToParticle(nextParticle);
            }
        }
    }

    private void Solver_OnCollision(object sender, ObiSolver.ObiCollisionEventArgs e)
    {
        CheckRopePlayerDisconnectedDistance();
        
        //Debug.Log(_ropeAttached + ", " + _ropeJumped); // start from: false, false
        if (PlayerIsAttached) return;
        if (_playerOnOtherObject) return;

        var world = ObiColliderWorld.GetInstance();
        foreach (var contact in e.contacts)
        {
            if (contact.distance < 0.01)
            {
                /* do collsion of bodyB */
                var col = world.colliderHandles[contact.bodyB].owner;

                if (col != null && col.CompareTag("Player"))
                {
                    /* do collsion of bodyA particles */
                    int particle = _rope.solver.simplices[contact.bodyA];
                    if (particle > _noClimbingParticlesUntil)
                    {
                        attachPlayerToParticle(particle);
                        PlayerOnThisObject?.Invoke(gameObject.GetInstanceID(), true);
                        PlayerLogic.PlayerObiCol.enabled = false;
                        PlayerIsInRange = true;
                        PlayerIsAttached = true;
                    }
                    _currentParticle = particle;

                    break;
                }
            }
        }
    }

    public override void DisconnectPlayer()
    {
        if (PlayerIsAttached)
        {
            _playerHasJumped = true;
            PlayerIsInRange = false;
            PlayerOnThisObject?.Invoke(gameObject.GetInstanceID(), false);
            detachPlayerFromParticle(_currentParticle);
            EnableRopeCollision(false);

            PlayerLogic.PlayerObiCol.enabled = true;
            PlayerLogic.DisconnectedPlayerAddJump();
        }
    }

    private void CheckRopePlayerDisconnectedDistance()
    {
        if (PlayerIsAttached && _playerHasJumped)
        {
            Vector3 particlePos = RopeCalcs.GetGlobalParticlePos(transform, _rope.solver.positions[_currentParticle]);
            bool awayFromRope = Vector2.Distance(particlePos, PlayerLogic.PlayerObiCol.transform.position) > _jumpedEnoughDistance;

            if (awayFromRope)
            {
                PlayerIsAttached = _playerHasJumped = false;
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

        _playerBatch.AddConstraint(toParticle, PlayerLogic.PlayerRopeRiderCol, Vector3.zero, Quaternion.identity, 0, 0, float.PositiveInfinity);
        _playerBatch.activeConstraintCount = 1;

        // this will cause the solver to rebuild pin constraints at the beginning of the next frame:
        _rope.SetConstraintsDirty(Oni.ConstraintType.Pin);
        _currentParticle = toParticle;

        PlayerLogic.SetPlayerZPosition(RopeCalcs.GetGlobalParticlePos(transform, _rope.solver.positions[toParticle]).z);
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

        _playerBatch.AddConstraint(toParticle, PlayerLogic.PlayerObiCol, Vector3.zero, Quaternion.identity, 0, 0, float.PositiveInfinity);
        _playerBatch.activeConstraintCount = 1;

        _rope.SetConstraintsDirty(Oni.ConstraintType.Pin);
        _currentParticle = toParticle;
    }
}
