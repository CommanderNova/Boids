[System.Serializable]
public class BoidSettings
{
	public float speed = 9;
	public float targetAttractionForce = 1.5f;

	public float repelForce = 2;
	public float alignForce = 2;
	public float cohesionForce = 3;

	public float maxBoidAvoidDistance = 1.5f;
	public float maxBoidAlignDistance = 6;
	public float maxBoidCohesionDistance = 6;
}