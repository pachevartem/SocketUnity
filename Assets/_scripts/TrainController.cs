using UnityEngine;
using System.Collections;

public class TrainController : MonoBehaviour
{

  public static float Speed { get; set; }

  public void Update()
  {
    this.transform.Translate(Vector3.forward * Speed * Time.deltaTime,Space.World);
  }
}
