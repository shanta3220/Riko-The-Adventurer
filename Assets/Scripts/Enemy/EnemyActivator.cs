using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// this scripts activates enemy batches  and open next barriers if enemy clears the area
/// </summary>
public class EnemyActivator : MonoBehaviour {
    public GameObject firstEnemyBatch;
    public GameObject[] LockedBarriers;

    public void ActivateFirstEnemyBatch() {
        if(firstEnemyBatch != null)
            firstEnemyBatch.SetActive(true);
    }
    public void OpenBarrier(int lockerID) {
        AudioController.instance.PlaySound(SoundClip.gateOpen);
        Destroy(LockedBarriers[lockerID]);
    }

}
