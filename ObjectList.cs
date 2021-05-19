using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectList : MonoBehaviour
{
    public List<GameObject> ToEnable;
    public List<GameObject> ToDisable;

    private void Awake()
    {
        ToEnable.ForEach(obj => obj.AddComponent<Disabler>());
    }

    public void SwitchObjectsStates()
    {
        ToEnable.ForEach(obj => obj.SetActive(true));
        ToDisable.ForEach(obj => obj.SetActive(false));
    }
}

public class Disabler : MonoBehaviour
{
    private void Start() => gameObject.SetActive(false);
}