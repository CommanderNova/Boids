using System.Collections.Generic;
using System.Numerics;

public class Boid
{
	bool separation = true;
	bool alignment = true;
	bool cohesion = true;

	private BoidSettings settings;

	public Boid(BoidSettings settings)
	{
		this.settings = settings;
	}

	public (Vector3, Vector3) GetNewTransform(float deltaTime, Vector3 position, Vector3 rotation, List<BoidData> otherBoids)
	{
		var targetDirection = Vector3.Zero;

		if(separation)
			AvoidNearbyBoids(position, ref targetDirection, otherBoids);
		if(alignment)
			AlignWithFlock(position, ref targetDirection, otherBoids);
		if(cohesion)
			SteerTowardsCenter(position, ref targetDirection, otherBoids);

		TurnTowardsTarget(deltaTime, ref rotation, targetDirection);
		MoveForward(deltaTime, ref position, rotation);

		return (position, rotation);
	}

	private void TurnTowardsTarget(float deltaTime, ref Vector3 rotation, Vector3 targetDirection)
	{
		rotation += targetDirection * settings.targetAttractionForce * deltaTime;
	}

	private void MoveForward(float deltaTime, ref Vector3 position, Vector3 rotation)
	{
		position += BoidHelper.GetNormalizedVector(rotation) * settings.speed * deltaTime;
	}

	/// Seperation
	/// Steer to avoid crowding local flockmates
	void AvoidNearbyBoids(Vector3 position, ref Vector3 targetDirection, List<BoidData> otherBoids)
	{
		foreach(var boid in otherBoids)
		{
			targetDirection += BoidHelper.CalculateRepelDirection(boid.position, position, settings);
		}
	}

	/// Alignment
	/// Steer towards the average heading of local flockmates
	void AlignWithFlock(Vector3 position, ref Vector3 targetDirection, List<BoidData> otherBoids)
	{
		var averageDirection = Vector3.Zero;
		foreach (var boid in otherBoids)
		{
			var distance = Vector3.Distance(position, boid.position);
			if (distance > settings.maxBoidAlignDistance)
				continue;

			averageDirection += boid.rotation;
		}
		averageDirection = BoidHelper.GetNormalizedVector(averageDirection);

		targetDirection += averageDirection * settings.alignForce;
	}

	/// Cohesion
	/// Steer to move towards the average position (center of mass) of local flockmates
	void SteerTowardsCenter(Vector3 position, ref Vector3 targetDirection, List<BoidData> otherBoids)
	{
		var averagePosition = Vector3.Zero;
		int closeBoids = 0;
		foreach (var boid in otherBoids)
		{
			var distance = Vector3.Distance(position, boid.position);
			if (distance > settings.maxBoidCohesionDistance)
				continue;

			closeBoids++;
			averagePosition += boid.rotation;
		}
		averagePosition /= closeBoids;

		targetDirection += averagePosition * settings.cohesionForce;
	}

	public void UpdateRules(bool separation, bool alignment, bool cohesion, BoidSettings settings)
	{
		this.separation = separation;
		this.alignment = alignment;
		this.cohesion = cohesion;
		this.settings = settings;
	}
}
