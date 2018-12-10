using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///<summary>
///responsible to attack the player
///</summary>
public class EnemyHitBox : Collidable {

	//damage
	public int damage = 1;
	public int pushForce = 2;

	protected override void OnCollide(Collider2D col){
		if(col.tag == "Player"){
			/// create new dmg
			Damage dmg = new Damage{
				damageAmount = damage,
				pushForce = pushForce,
				origin = transform.position

			};
			col.SendMessage("ReceiveDamage",dmg);
		}
	}

}
