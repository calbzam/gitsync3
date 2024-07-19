using Obi;
using UnityEngine;

public class ZipLineRopeCalculator : MonoBehaviour
{
    private ObiRope _rope;

    [Header("Stop moving if X-distance between currentParticle and first/lastParticle exceeds this value")]
    [SerializeField] private float _stopMargin = 2f;

    private Vector2 _currentParticlePos;
    private Vector2 _firstParticlePos, _middleParticlePos, _lastParticlePos;

    private void Start()
    {
        _rope = gameObject.GetComponent<ObiRope>();
        _rope.solver.OnCollision += Solver_OnCollision;
        GetEndParticlePositions();
    }

    private void OnDisable()
    {
        _rope.solver.OnCollision -= Solver_OnCollision;
    }

    private void GetEndParticlePositions()
    {
        int firstParticle = _rope.elements[0].particle1;
        int lastParticle = _rope.elements[_rope.elements.Count - 1].particle2;
        int middleParticle = _rope.elements[getParticleWithIndex((_rope.elements.Count - 1) / 2)].particle2;

        _firstParticlePos = getGlobalParticlePos(_rope.solver.positions[firstParticle]);
        _middleParticlePos = getGlobalParticlePos(_rope.solver.positions[middleParticle]);
        _lastParticlePos = getGlobalParticlePos(_rope.solver.positions[lastParticle]);
    }

    public Vector2 GetFurtherRopeDir()
    {
        if (_currentParticlePos.x - _firstParticlePos.x < _lastParticlePos.x - _currentParticlePos.x)
        {
            //Vector2 diff = _lastParticlePos - _currentParticlePos;
            //return diff.normalized;
            return Vector2.right;
        }
        else
        {
            //Vector2 diff = _firstParticlePos - _currentParticlePos;
            //return diff.normalized;
            return Vector2.left;
        }
    }

    // dir < 0: left
    // dir > 0: right
    public Vector2 GetNextPulleyDir(float dir)
    {
        Vector2 leftEnd, rightEnd;
        bool firstIsLeft = false, rightIsLast = false;
        if (_currentParticlePos.x < _middleParticlePos.x) { leftEnd = _firstParticlePos; rightEnd = _middleParticlePos; firstIsLeft = true; }
        else { leftEnd = _middleParticlePos; rightEnd = _lastParticlePos; rightIsLast = true; }

        if (dir < 0)
        {
            if (firstIsLeft && _currentParticlePos.x - leftEnd.x < _stopMargin) return Vector2.zero;
            Vector2 diff = leftEnd - _currentParticlePos;
            return diff.normalized;
        }
        else if (dir > 0)
        {
            if (rightIsLast && rightEnd.x - _currentParticlePos.x < _stopMargin) return Vector2.zero;
            Vector2 diff = rightEnd - _currentParticlePos;
            return diff.normalized;
        }
        else
        {
            return Vector2.zero;
        }
    }

    private int getIndexInActor(int particle)
    {
        return _rope.solver.particleToActor[particle].indexInActor;
    }

    private int getParticleWithIndex(int indexInActor)
    {
        return _rope.solverIndices[indexInActor];
    }

    private Vector3 getGlobalParticlePos(Vector3 particlePosition)
    {
        Vector3 childUpdated = transform.parent.rotation * Vector3.Scale(particlePosition, transform.parent.lossyScale);

        return childUpdated + transform.parent.position;
    }

    private void Solver_OnCollision(object sender, ObiSolver.ObiCollisionEventArgs e)
    {
        var world = ObiColliderWorld.GetInstance();
        foreach (var contact in e.contacts)
        {
            if (contact.distance < 0.01)
            {
                /* do collsion of bodyB */
                var col = world.colliderHandles[contact.bodyB].owner;

                if (col != null && col.name.Equals("ZipLine Pulley"))
                {
                    /* do collsion of bodyA particles */
                    _currentParticlePos = getGlobalParticlePos(contact.pointB);

                    break;
                }
            }
        }
    }

}
