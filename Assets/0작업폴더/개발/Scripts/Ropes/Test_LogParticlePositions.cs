using System;
using UnityEngine;
using Obi;

public class Test_LogParticlePositions : MonoBehaviour
{
    private ObiRope rope;

    private int firstParticle, lastParticle;
    private Vector3 firstPos, lastPos;

    private Vector3[] particlePositions;

    private void Awake()
    {
        rope = gameObject.GetComponent<ObiRope>();
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

#if UNITY_EDITOR
        LogParticlePositions();
#endif
    }

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

#if UNITY_EDITOR
    private void LogParticlePositions()
    {
        ClearConsole.ClearLog();
        foreach (var pos in particlePositions)
        {
            Debug.Log(pos);
        }
    }
#endif

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("OnParticleCollision collision object: " + other);
    }
}
