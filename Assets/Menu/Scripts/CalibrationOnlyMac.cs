using UnityEngine;

public class CalibrationOnlyMac : MonoBehaviour
{
  private void Awake()
  {
#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
#else
    gameObject.SetActive(false);
#endif
  }
}