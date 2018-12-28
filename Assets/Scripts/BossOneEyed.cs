using UnityEngine;

public class BossOneEyed : Enemy {

    public float[] fireballSpeed = { 2f, -2f };
    public float distance = 0.25f;
    public Transform[] fireBalls;

    protected override void Start () {
        base.Start();
	}
	
	private void Update () {
        for(int i = 0; i < fireBalls.Length; i++) {
            fireBalls[i].transform.position = transform.position + new Vector3(-Mathf.Cos(Time.time * fireballSpeed[i]) * distance, -Mathf.Sin(Time.time * fireballSpeed[i]) * distance, 0f);
        }
        
	}
}
