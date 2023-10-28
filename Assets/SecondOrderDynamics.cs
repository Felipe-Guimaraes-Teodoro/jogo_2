using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondOrderDynamics : MonoBehaviour
{
  private Vector3 xp; // Previous input
  private Vector3 y, yd; // State variables
  
  private float k1, k2, k3; // Dynamic constants

  private float PI = Mathf.PI;

  public float f, z, r;
  public Vector3 x0;
  public float v;

  public GameObject bola;
  public GameObject goal;

  // Start is called before the first frame update
  void Start()
  {
    x0 = goal.transform.position;

    k1 = z / (PI * f);
    k2 = 1.0f / ((2.0f * PI * f) * (2.0f * PI * f));
    k3 = r * z / (2.0f * PI * f);

    xp = x0;
    y = x0;
    yd = Vector3.zero;
  }

  // Update is called once per frame
  void Update()
  {
    // Assuming you have a valid reference to the `bola` object
    if (v <= 0.0f)
    {
      v = 1.0f;
    }

    RecalculateConstants();
    Vector3 x = goal.transform.position;
    Vector3 xd = CalculateVelocity(x);

    Vector3 coiso = UpdateSystem(x, xd);
    bola.transform.position = coiso;
    bola.transform.rotation = goal.transform.rotation;
  }

  Vector3 CalculateVelocity(Vector3 x)
  {
    return (x - xp) / Time.deltaTime;
  }

  Vector3 UpdateSystem(Vector3 x, Vector3 xd)
  {
    if (xp == null)
    {
      xp = (x - xp) / Time.deltaTime;
      xp = x;
    }
    y = y + Time.deltaTime * yd; // Integrate position by velocity
    yd = yd + Time.deltaTime * (x + k3 * xd - y - k1 * yd) / k2;
    xp = x;

    return y;
  }

  void RecalculateConstants()
  {
    k1 = z / (PI * f);
    k2 = 1.0f / ((2.0f * PI * f) * (2.0f * PI * f));
    k3 = r * z / (2.0f * PI * f);
  }
}
