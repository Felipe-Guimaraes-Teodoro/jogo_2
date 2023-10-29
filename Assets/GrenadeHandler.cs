using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeHandler : MonoBehaviour
{
  public GameObject pino;
  public GameObject self;
  public GameObject grenadeObject;
  public Transform camera;
  public StateController StateController;

  bool canExplode = false;
  bool pinOut = false;
  bool beenThrown = false;

  Rigidbody grenade;

  void Start()
  {
    grenade = grenadeObject.GetComponent<Rigidbody>();
    canExplode = true;
  }

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.G) && canExplode) {
      List<Rigidbody> rigidBodies = new List<Rigidbody>();

        pino.GetComponents(rigidBodies);
      
      foreach (Rigidbody rigidBody in rigidBodies)
      {
        rigidBody.isKinematic = false;

        Vector3 localDir = -camera.transform.right;
        rigidBody.AddForce(localDir * 3.0f, ForceMode.Impulse);
      }
      pinOut = true;
      canExplode = false;
      // pino.transform.SetParent(null); 
    }

    if (Input.GetKeyUp(KeyCode.G) && pinOut && !beenThrown) {
      // throw the grenade in the direction which the camera is facing
      grenade.isKinematic = false;

      Vector3 localDir = camera.transform.forward;
      grenade.AddForce(localDir * 20.0f, ForceMode.Impulse);

      transform.SetParent(null);

      StartCoroutine(Bait(5f));

      beenThrown = true;
    }
  }

  IEnumerator Bait(float t) {
    yield return new WaitForSeconds(t);
    Blow();
  }

  void Blow() 
  {
    Vector3 explosionPos = grenadeObject.transform.position;
    Collider[] colliders = Physics.OverlapSphere(explosionPos, 10f);

    foreach (Collider hit in colliders)
    {
      if (hit.GetComponent<Rigidbody>())
      {
        hit.GetComponent<Rigidbody>()
          .AddExplosionForce(1000f, explosionPos, 5f, 5000f);
      }
    }

    Object.Destroy(self, 2f);
  }
}
