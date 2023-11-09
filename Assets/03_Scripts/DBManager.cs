using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBManager : MonoBehaviour
{
    public static DBManager Manager;

    private void Awake() {
        if (Manager != null) {
            Destroy(gameObject);
        } else {
            Manager = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    public string NextSceneName;
    public string TargetBookName;
}
