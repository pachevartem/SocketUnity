using UnityEngine;
using System.Collections;

public class TrainController : MonoBehaviour
{
	/// <summary>
	/// Объект пустышка которые перемещается в координату
	/// </summary>
	public GameObject target;
	/// <summary>
	/// Текущая позиция
	/// </summary>
	public static float CurentPosition = 0;

	/// <summary>
	/// Метод выполняется каждый кадр отрисовка. Частота выполнения зависит от показателя FPS
	/// </summary>
	public void Update ()
	{
		target.transform.position = new Vector3 (target.transform.position.x, target.transform.position.y, CurentPosition);
	//	FakeServerRequest (); //off
	}
	/// <summary>
	/// Вызывается в конце кадра
	/// </summary>
	void LateUpdate ()
	{
		Camera.main.transform.position = Vector3.Lerp (Camera.main.transform.position, target.transform.position, Time.deltaTime * 50);
	}

	/// <summary>
	/// Fakes the server request.
	/// </summary>
	void FakeServerRequest ()
	{
		CurentPosition += 2;
	}
}
