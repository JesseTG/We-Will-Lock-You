using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

[DisallowMultipleComponent]
public class GameManager : MonoBehaviour
{
  [Tooltip("Maximum value for the lock, not inclusive")]
  public int Max = 32;

  public int Current = 0;

  public int[] Combination = new int[3];

  public Text[] ComboLabels = new Text[3];

  public IntEvent OnCorrectNumber;
  public UnityEvent OnWrongNumber;
  public UnityEvent OnWin;

  // Use this for initialization
  void Start()
  {
    for (int i = 0; i < Combination.Length; ++i)
    {
      Combination[i] = Random.Range(0, this.Max);

      if (ComboLabels[i])
      // If we've actually assigned a Text object in this array...
      {
        ComboLabels[i].text = Combination[i].ToString();
      }
    }
  }

  public void OnNumberEntered(int number)
  {
    if (Current < Combination.Length)
    // If we haven't yet finished the game...
    {
      if (number == Combination[Current])
      // If this is the correct number for this part of the combination...
      {
        OnCorrectNumber.Invoke(Current);
        // Tell anyone who's interested.

        ComboLabels[Current].color = Color.green;
        // Set the current number to green to show that we've entered it.

        Current++;
        // Now we're entering the next number.

        if (Current == Combination.Length)
        {
          OnWin.Invoke();
        }
      }
      else
      {
        OnWrongNumber.Invoke();
        // Tell anyone who's listening.

        Current = 0;
        // Start over.

        foreach (Text t in ComboLabels)
        {
          t.color = Color.white;
          // Set each of the combo labels to white to show that we have to start again.
        }
      }
    }
  }

  public void RestartGame()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    // Reloads the current scene.  Not the best way to restart a game, but it's the easiest.
    // Doing it this way will (I believe) reload all the relevant assets.  In reality, you
    // might just recreate/delete/reset objects yourself to reflect the initial state of the
    // level or game.
  }
}
