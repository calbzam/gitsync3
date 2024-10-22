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

    public static int GetIndexInActor(ObiSolver solver, int particle)
    {
        return solver.particleToActor[particle].indexInActor;
    }

    public static float GetNormalizedMu(ObiRope rope, int particle)
    {
        float startY = rope.GetParticlePosition(rope.GetElementAt(0, out _).particle1).y;
        float endY = rope.GetParticlePosition(rope.GetElementAt(1, out _).particle1).y;
        float currentParticleY = rope.GetParticlePosition(particle).y;

        float normalizedMu = (currentParticleY - startY) / (endY - startY);
        if (normalizedMu < 0) normalizedMu = 0;
        return normalizedMu;
    }

    public static int GetElementIndexOfParticle(ObiRope rope, int particle)
    {
        for (int i = 0; i < rope.elements.Count; ++i)
        {
            if (rope.elements[i].particle2 == particle) return i;
        }
        return -1; // rope.elements[0].particle1
    }
}
