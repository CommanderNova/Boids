using System;
using System.Collections.Generic;
using UnityEngine;

public class UnityBoid : MonoBehaviour
{
	BoidSettings settings = new BoidSettings();

	Boid boid;

	// TODO exclude self boid from otherBoidsData

	void Awake()
	{
		boid = new Boid(settings);
	}

	public void UpdateBoid(List<BoidData> otherBoidsData)
	{
		var (newPosition, newRotation) = boid.GetNewTransform(
			Time.deltaTime, 
			ConvertToSystemVector3(transform.position), 
			ConvertToSystemVector3(transform.forward), 
			otherBoidsData
		);

		transform.position = ConvertToUnityVector3(newPosition);
		transform.rotation = Quaternion.LookRotation(ConvertToUnityVector3(newRotation));
	}

	System.Numerics.Vector3 ConvertToSystemVector3(Vector3 vector)
	{
		return new System.Numerics.Vector3(vector.x, vector.y, vector.z);
	}

	Vector3 ConvertToUnityVector3(System.Numerics.Vector3 vector)
	{
		return new Vector3(vector.X, vector.Y, vector.Z);
	}

    public void UpdateRules(bool separation, bool alignment, bool cohesion, BoidSettings settings)
    {
		boid.UpdateRules(separation, alignment, cohesion, settings);
	}
}
