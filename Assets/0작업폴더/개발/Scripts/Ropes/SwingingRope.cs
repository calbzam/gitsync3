using System;
using UnityEngine;
using Obi;

public class SwingingRope : RidableObject
{
    private ObiRope _rope;
    private static float _jumpedEnoughDistance;

    [Header("Disable Player-Ground collision while climbing")]
    [SerializeField] private bool _disablePlayerGroundCollision = false;

    [Header("Particle 0 is mostly the first (top) particle")]
    [SerializeField] private int _noClimbingParticlesUntil = 4;

    public override event Action<int, bool> PlayerOnThisObject;
    public bool PlayerIsInRange { get; set; }

    private int _currentParticle;
    private int _currentIndexInElements;
    private ObiConstraints<ObiPinConstraintsBatch> _ropePinConstraints;
    private ObiPinConstraintsBatch _playerBatch;

    public bool InitializingComplete { get; private set; }

    //private bool[] particleHasCollision;

    private void Awake() // is called every time when scene is reloaded!
    {
        _rope = gameObject.GetComponent<ObiRope>();
        _currentParticle = -1;
        _currentIndexInElements = -2;

        //particleHasCollision = new bool[rope.particleCount];
    }

    protected override void Start()
    {
        base.Start();
        PlayerIsInRange = false;
        _jumpedEnoughDistance = PlayerLogic.Player.Stats.RopeJumpedDistance;

        //GetRopePinConstraints();
        //InitPlayerBatch();
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

    private void GetRopePinConstraints()
    {
        _ropePinConstraints = _rope.GetConstraintsByType(Oni.ConstraintType.Pin) as ObiConstraints<ObiPinConstraintsBatch>;
    }

    private void ClearRopePinConstraints()
    {
        _ropePinConstraints.Clear(); // remove all batches from the constraint type we want, so we start clean:
    }

    private void EnablePlayerGroundCollision(bool enabled)
    {
        PlayerLogic.IgnorePlayerGroundCollision(!enabled);
        PlayerLogic.Player.GroundCheckAllowed = enabled;
    }

    public static void EnablePlayerRopeCollision(bool enabled)
    {
        if (PlayerLogic.PlayerObiCol.enabled != enabled)
            PlayerLogic.PlayerObiCol.enabled = enabled;

        //_rope.solver.particleCollisionConstraintParameters.enabled = enabled; // these disable rope collision entirely, not just with Player
        //_rope.solver.collisionConstraintParameters.enabled = enabled;
    }

    private int GetPrevParticleInElements()
    {
        if (_currentIndexInElements == 0) return _rope.elements[_currentIndexInElements--].particle1;
        else return _rope.elements[--_currentIndexInElements].particle2;
    }

    private int GetNextParticleInElements()
    {
        return _rope.elements[++_currentIndexInElements].particle2;
    }

    // indexInActor: https://obi.virtualmethodstudio.com/forum/thread-4019-post-14919.html#pid14919
    private void HandleRopeClimb()
    {
        if (!PlayerLogic.Player.RopeClimbAllowed) return;
        if (!PlayerIsInRange || PlayerOnOtherObject) return;

        if (FrameInputReader.FrameInput.InputDir.y > 0)
        {
            if (!PlayerIsAttached) AnnouncePlayerOnThisObject();
            if (_currentIndexInElements > -1 /* first particle in visible rope */)
            {
                DetachPlayerFromParticle(_currentParticle);
                int prevParticle = GetPrevParticleInElements();
                PlayerLogic.PlayerObiCol.transform.position = RopeCalcs.GetGlobalParticlePos(transform, _rope.solver.positions[prevParticle]);
                AttachPlayerToParticle(prevParticle);
            }
        }
        else if (FrameInputReader.FrameInput.InputDir.y < 0)
        {
            if (!PlayerIsAttached) AnnouncePlayerOnThisObject();
            if (_currentIndexInElements < _rope.elements.Count - 1 /* last particle in visible rope */)
            {
                DetachPlayerFromParticle(_currentParticle);
                int nextParticle = GetNextParticleInElements();
                PlayerLogic.PlayerObiCol.transform.position = RopeCalcs.GetGlobalParticlePos(transform, _rope.solver.positions[nextParticle]);
                AttachPlayerToParticle(nextParticle);
            }
        }
    }

    private void AnnouncePlayerOnThisObject()
    {
        PlayerIsAttached = true;
        PlayerOnOtherObject = false;
        PlayerOnThisObject?.Invoke(gameObject.GetInstanceID(), true);
        _currentIndexInElements = RopeCalcs.GetElementIndexOfParticle(_rope, _currentParticle);
        if (_disablePlayerGroundCollision) EnablePlayerGroundCollision(false);
    }

    private void Solver_OnCollision(object sender, ObiSolver.ObiCollisionEventArgs e)
    {
        if (!RopePlayerDisconnectedDistanceSatisfied()) return;

        if (PlayerIsAttached) return;
        if (PlayerOnOtherObject) return;

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
                    _currentParticle = _rope.solver.simplices[contact.bodyA];
                    PlayerIsInRange = true;
                    if (PlayerLogic.Player.RopeClimbAllowed && _currentParticle > _noClimbingParticlesUntil)
                    {
                        AttachPlayerToParticle(_currentParticle);
                        AnnouncePlayerOnThisObject();
                        PlayerLogic.PlayerObiCol.enabled = false;
                    }

                    break;
                }
                else
                {
                    PlayerIsInRange = false;
                }
            }
        }
    }

    public void DisconnectPlayerKeepRangeNoJump()
    {
        if (PlayerIsAttached)
        {
            _playerHasJumped = true;
            PlayerOnThisObject?.Invoke(gameObject.GetInstanceID(), false);
            if (_disablePlayerGroundCollision) EnablePlayerGroundCollision(true);
            DetachPlayerFromParticle(_currentParticle);

            PlayerLogic.PlayerObiCol.enabled = true;
        }
    }

    public override void DisconnectPlayer()
    {
        if (PlayerIsAttached)
        {
            _playerHasJumped = true;
            PlayerIsInRange = false;
            PlayerOnThisObject?.Invoke(gameObject.GetInstanceID(), false);
            if (_disablePlayerGroundCollision) EnablePlayerGroundCollision(true);
            DetachPlayerFromParticle(_currentParticle);

            PlayerLogic.DisconnectedPlayerAddJump();
            PlayerLogic.PlayerObiCol.enabled = true;
        }
    }

    private bool RopePlayerDisconnectedDistanceSatisfied()
    {
        if (PlayerIsAttached && _playerHasJumped)
        {
            Vector3 particlePos = RopeCalcs.GetGlobalParticlePos(transform, _rope.solver.positions[_currentParticle]);
            bool awayFromRope = Vector2.Distance(particlePos, PlayerLogic.PlayerObiCol.transform.position) > _jumpedEnoughDistance;

            if (awayFromRope)
            {
                PlayerIsAttached = _playerHasJumped = false;
                EnablePlayerRopeCollision(true);
                _currentParticle = -1;
            }

            return awayFromRope;
        }
        else return true;
    }

    // Scripting constraints http://obi.virtualmethodstudio.com/manual/6.3/scriptingconstraints.html

    private void InitPlayerBatch()
    {
        GetRopePinConstraints();
        //_ropePinConstraints.Clear(); // remove all batches (including all constraints manually added in the Unity Editor) from the constraint type we want, so we start clean:

        // add a new pin constraints batch
        _playerBatch = new ObiPinConstraintsBatch();
        _ropePinConstraints.AddBatch(_playerBatch);
    }

    private void AttachPlayerToParticle(int toParticle)
    {
        InitPlayerBatch();

        PlayerLogic.SetPlayerZPosition(RopeCalcs.GetGlobalParticlePos(transform, _rope.solver.positions[toParticle]).z);
        _playerBatch.AddConstraint(toParticle, PlayerLogic.PlayerRopeRiderCol, Vector3.zero, Quaternion.identity, 0, 0, float.PositiveInfinity);
        _playerBatch.activeConstraintCount = 1;

        // this will cause the solver to rebuild pin constraints at the beginning of the next frame:
        _rope.SetConstraintsDirty(Oni.ConstraintType.Pin);

        _currentParticle = toParticle;
    }

    private void DetachPlayerFromParticle(int fromParticle)
    {
        GetRopePinConstraints();
        _ropePinConstraints.RemoveBatch(_playerBatch);

        _rope.SetConstraintsDirty(Oni.ConstraintType.Pin);
    }

    private void RepinPlayerToParticle(int fromParticle, int toParticle) // rather unstable without SetConstraintsDirty, i assume
    {
        if (_playerBatch.activeConstraintCount > 0)
            /*_playerBatch.RemoveConstraint(fromParticle);*/ _playerBatch.Clear();

        _playerBatch.AddConstraint(toParticle, PlayerLogic.PlayerObiCol, Vector3.zero, Quaternion.identity, 0, 0, float.PositiveInfinity);
        _playerBatch.activeConstraintCount = 1;

        _rope.SetConstraintsDirty(Oni.ConstraintType.Pin);

        PlayerLogic.SetPlayerZPosition(RopeCalcs.GetGlobalParticlePos(transform, _rope.solver.positions[toParticle]).z);
        _currentParticle = toParticle;
    }
}
