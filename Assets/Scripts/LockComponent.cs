using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
public class LockComponent : MonoBehaviour
{
  [Tooltip("Speed of the lock in degrees per second.")]
  public float AngularSpeed = 5;

  public float Direction = -1;

  public IntEvent OnEnterNumber;

  private GameManager _gameManager;

  void Start()
  {
    this._gameManager = GameObject.FindObjectOfType<GameManager>();
  }

  void Update()
  {
    float angleDelta = this.Direction * this.AngularSpeed * Time.deltaTime;
    //  Rotation is determined by [deg/s * s == deg]

    this.transform.Rotate(0, 0, angleDelta);
    // The lock rotates around its own z axis.

    if (Input.GetButtonDown("Fire1"))
    // If the player just pressed down this button...
    {
      float angle = transform.localRotation.eulerAngles.z % 360f;
      // The math here is simplified if we modulo the rotation by 360 degrees
      // (in many engines, rotations outside of 0-360 are valid, but not very useful)

      float ratio = 1f - (angle / 360f);
      // Positive is counterclockwise, but rotary locks run clockwise, so this fixes that

      int number = Mathf.RoundToInt(ratio * _gameManager.Max);
      // Round to the nearest lock tick (the tick plus half the distance to the next)

      this.OnEnterNumber.Invoke(number);
      // So now the player has entered a lock number, let's tell anyone who cares.

      Debug.LogFormat(this, "Player entered number {0}", number);
    }
  }

  public void OnCorrectNumber(int current)
  {
    if (current == _gameManager.Combination.Length - 1)
    // If we just entered the final number...
    {
      Direction = 0;
    }
    else
    {
      Direction *= -1;
    }
  }

  public void OnWrongNumber()
  {
    this.Direction = -1;
  }
}
