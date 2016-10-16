using UnityEngine;
using System.Collections;

public class TrainController : MonoBehaviour
{

    public GameObject target;
    public static float Speed { get; set; }
    public float speed = 10;
    public static float CurentPosition=0;
  
  
  public void Update()
  {
   
    target.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, CurentPosition);
   // c();
  }

  void LateUpdate()
  {
    Camera.main.transform.position = Vector3.MoveTowards(
     Camera.main.transform.position, target.transform.position, Time.deltaTime * 100
    );
  }


  void c()
  {
    CurentPosition += 0.1f;
    ;
  }
}
