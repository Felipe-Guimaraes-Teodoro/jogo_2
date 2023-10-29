using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
  public int grenadeCount = 2;
  bool grenadeEquipped = false;

  public GameObject grenadeOnStorage;
  public GameObject grenadeHandle;
  
  GameObject grenadeInstance;

  void Start()
  {

  }

  void Update()
  {
    if (
        Input.GetKeyDown(KeyCode.Alpha3) 
        && grenadeCount > 0 
        && !grenadeEquipped
      ) 
    {
      ExcludeOthers();
      AddGrenade();
      grenadeCount -= 1;
    }
    if (
        Input.GetKeyDown(KeyCode.Alpha2)
        // && !pistolEquipped,
      ) 
    {
      ExcludeOthers();  
    }
    if (Input.GetKeyUp(KeyCode.G)) { grenadeEquipped = false; }
  }

  void ExcludeOthers() {
    RemoveGrenade();
  }

  void AddGrenade() {
    grenadeEquipped = true;
    grenadeInstance = Object.Instantiate(grenadeOnStorage);
  
    // Script script = grenadeInstance.GetComponentInChildren<Script>();

    grenadeInstance.transform.SetParent(grenadeHandle.transform);
    grenadeInstance.SetActive(true);
    grenadeInstance.transform.position = new Vector3(-0.5f, -10f, -1.4f);
  }

  void RemoveGrenade() {
    if (grenadeInstance != null && grenadeEquipped) {
      Object.Destroy(grenadeInstance);
      grenadeEquipped = false;
      grenadeCount += 1;
    }
  }
}
