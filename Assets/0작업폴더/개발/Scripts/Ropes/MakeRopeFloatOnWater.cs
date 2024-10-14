using System;
using UnityEngine;
using Obi;
using Unity.VisualScripting;

public class MakeRopeFloatOnWater : MonoBehaviour
{
    private ObiRope _rope;

    [Header("Colliders are created in runtime")]
    [SerializeField] private ColType _collliderType = ColType.CircleCollider2D;
    [SerializeField] private int _colsCount = 22;
    [SerializeField] private float _eachColDensity = 3.5f;
    [SerializeField] private float _eachColScale = 0.1f;
    [SerializeField] private float _playerReaderScale = 1.6f;

    [Header("Allows Player to \"push the rope\" after colliders are attached")]
    [SerializeField] private bool _maxDistanceOnly = true;
    [SerializeField] private float _buoyColsPushSpeed = 4f;

    private enum ColType { BoxCollider2D, CircleCollider2D }

    private GameObject _buoyParent;
    private ObiCollider2D[] _buoyCols;
    private Rigidbody2D[] _buoyRbs;

    public void AttachFloatingColliders()
    {
        createBuoyCols(_buoyCols, _buoyRbs);
        attachColsToParticleInOrder(_buoyCols);
        addDistanceJointsToCols(_buoyCols, _buoyRbs);
    }

    private void Awake()
    {
        _rope = gameObject.GetComponent<ObiRope>();
        _buoyCols = new ObiCollider2D[_colsCount];
        _buoyRbs = new Rigidbody2D[_colsCount];
    }

    private void createBuoyCols(ObiCollider2D[] cols, Rigidbody2D[] rbs)
    {
        _buoyParent = new GameObject();
        _buoyParent.name = "buoyParent";
        _buoyParent.transform.SetParent(_rope.solver.transform);

        for (int i = 0; i < _colsCount; ++i)
        {
            GameObject buoyCol = new GameObject();
            buoyCol.name = "col (" + i + ")";
            buoyCol.transform.SetParent(_buoyParent.transform);
            buoyCol.transform.localScale = _eachColScale * Vector3.one;
            buoyCol.layer = Layers.GroundLayer.LayerValue;

            rbs[i] = buoyCol.AddComponent<Rigidbody2D>();
            rbs[i].useAutoMass = true;
            Collider2D col;
            col = (_collliderType == ColType.BoxCollider2D) ? buoyCol.AddComponent<BoxCollider2D>() : buoyCol.AddComponent<CircleCollider2D>();
            col.density = _eachColDensity;
            //col.excludeLayers = Layers.PlayerLayer.MaskValue;
            cols[i] = buoyCol.AddComponent<ObiCollider2D>();
            cols[i].Filter = 1;

            GameObject playerRead = new GameObject();
            playerRead.name = "Player인식";
            playerRead.transform.localScale = _playerReaderScale * Vector3.one;
            playerRead.transform.SetParent(buoyCol.transform);
            playerRead.AddComponent<CircleCollider2D>().isTrigger = true;
            playerRead.AddComponent<RopeBuoyColPushableByPlayer>().PushSpeed = _buoyColsPushSpeed;
        }
    }

    private void attachColsToParticleInOrder(ObiCollider2D[] cols)
    {
        float ropeLength = _rope.elements.Count + 1;
        float division = ropeLength / (cols.Length + 1);

        for (int i = 0; i < cols.Length; ++i)
        {
            int toParticle = _rope.solverIndices[(int)Mathf.Round(division * (i + 1))];
            cols[i].transform.position = RopeCalcs.GetGlobalParticlePos(_rope.transform, _rope.solver.positions[toParticle]);

            var attachment = _rope.AddComponent<ObiParticleAttachment>();
            var group = ScriptableObject.CreateInstance<ObiParticleGroup>();
            group.particleIndices.Add(toParticle);

            attachment.target = cols[i].transform;
            attachment.particleGroup = group;
            attachment.attachmentType = ObiParticleAttachment.AttachmentType.Static;
        }
    }

    private void addDistanceJointsToCols(ObiCollider2D[] cols, Rigidbody2D[] rbs)
    {
        for (int i = 0; i < cols.Length - 1; ++i)
        {
            var joint1 = cols[i].AddComponent<DistanceJoint2D>();
            joint1.connectedBody = rbs[i + 1];
            joint1.enableCollision = true;
            joint1.autoConfigureConnectedAnchor = false;
            joint1.autoConfigureDistance = false;
            joint1.maxDistanceOnly = _maxDistanceOnly;
            joint1.distance = Vector2.Distance(cols[i].transform.position, cols[i + 1].transform.position);

            var joint2 = cols[i + 1].AddComponent<DistanceJoint2D>();
            joint2.connectedBody = rbs[i];
            joint2.enableCollision = joint1.enableCollision;
            joint2.autoConfigureConnectedAnchor = joint1.autoConfigureConnectedAnchor;
            joint2.autoConfigureDistance = joint1.autoConfigureDistance;
            joint2.maxDistanceOnly = joint1.maxDistanceOnly;
            joint2.distance = joint1.distance;
        }
    }
}
