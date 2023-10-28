using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controlador : MonoBehaviour
{
  public GameObject camera;
  public float sensitivity = 1.0f;
  public float speed = 1.0f;
    
  void Start()
  {
    Cursor.lockState = CursorLockMode.Locked;
  }

  void Update()
  {
    float dt = Time.deltaTime;
    camera.transform.position += 
      camera.transform.forward * Input.GetAxis("Vertical") * speed * dt;

    camera.transform.position += 
      camera.transform.right * Input.GetAxis("Horizontal") * speed * dt;       

    float mouseX = Input.GetAxis("Mouse X");
    float mouseY = Input.GetAxis("Mouse Y");
    camera.transform.eulerAngles += 
      new Vector3(-mouseY * sensitivity, mouseX * sensitivity, 0);

  }
}
