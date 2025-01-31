using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewShip", menuName = "ScriptableObjects/Elements/Ship", order = 1)]
public class SO_Ship : SO_BaseElement
{    
    [Space]
    public Vector3 BulletsSpawnPointsLocalPosition;
    public Vector3 PropulsionFireLocalPosition;
    [Space]
    public float movementSpeed;
    public float rotationSpeed;
    [Space]
    public List<ParticleSetup> DestuctionParticles;
}
