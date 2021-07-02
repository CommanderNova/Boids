using System;
using System.Numerics;

static class BoidHelper
{
	public static Vector3 GetNormalizedVector(Vector3 vector)
	{
		if (vector == Vector3.Zero)
			return Vector3.Zero;

		var length = vector.Length();

		return new Vector3(
			vector.X / length,
			vector.Y / length,
			vector.Z / length
			);
	}

	public static Vector3 CalculateRepelDirection(Vector3 otherBoid, Vector3 boid, BoidSettings settings)
	{
		var direction = otherBoid - boid;
		var distance = direction.Length();

		if (distance > settings.maxBoidAvoidDistance)
			return Vector3.Zero;

		var normalizedDirection = GetNormalizedVector(direction);

		// Make boids that are further away repel stronger than boids that are closer
		var invertedDistance = 1 / Math.Max(distance, 1); // 1 Can later be exchanged by boidSettings.minDistanceToBoids

		return -normalizedDirection * (invertedDistance * settings.repelForce);
	}
}