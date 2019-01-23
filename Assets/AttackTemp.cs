using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTemp : MonoBehaviour {
    public Animator[] ranged;
    public Animator[] melee;
    public UnityEngine.UI.Text stateText;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0)) {
            stateText.text = "Attack State";
            for (int i = 0; i < melee.Length; i++) {
                melee[i].SetBool("IsRunning", true);
                if (i >= ranged.Length)
                    continue;
                ranged[i].SetTrigger("Attack");
            }
        }
	}
}
