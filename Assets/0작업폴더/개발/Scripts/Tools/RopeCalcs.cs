using System;
using UnityEngine;
using Obi;

public static class RopeCalcs
{
    public static Vector3 GetGlobalParticlePos(Transform ropeTransform, Vector3 particlePosition)
    {
        Vector3 childUpdated = ropeTransform.parent.rotation * Vector3.Scale(particlePosition, ropeTransform.parent.lossyScale);
        return childUpdated + ropeTransform.parent.position;
    }

    public static Vector3 GetGlobalContactPos(Transform ropeTransform, ObiSolver solver, Oni.Contact contact)
    {
        int simplexStart = solver.simplexCounts.GetSimplexStartAndSize(contact.bodyA, out int simplexSize);
        int particleIndex = solver.simplices[simplexStart + simplexSize - 1];
        return GetGlobalParticlePos(ropeTransform, solver.positions[particleIndex]);
    }
}
