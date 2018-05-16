 using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {
	public float health = 150f;
	public GameObject projectile;
	public float projectileSpeed;
	public float shotsPerSecond = 0.5f;
	public int scoreValue = 150;

	public AudioClip fireSoundEnemy;
	public AudioClip enemyDieSound;

	private ScoreKeeper scoreKeeper;

	void Start(){
		scoreKeeper = GameObject.Find("ScoreText").GetComponent<ScoreKeeper>();
	}
	void OnTriggerEnter2D(Collider2D collider){
		Projectile missile = collider.gameObject.GetComponent<Projectile> ();

		if (missile) {
			missile.Hit();
			health -= missile.GetDamage();
			if (health <= 0){
				Die();
			}
			//print ("Hit by projectile");
		}
	}
	void Die(){
		Destroy(this.gameObject);
		scoreKeeper.ScorePoints(scoreValue);
		AudioSource.PlayClipAtPoint(enemyDieSound, this.transform.position);
	}

	void Update(){
		float probability = Time.deltaTime * shotsPerSecond;
		//print (probability + " " + Time.deltaTime);
		if(Random.value < probability){
			Fire();
		}
	}
	void Fire(){
		Vector3 startPosition = this.transform.position + new Vector3(0,-1, 0);
		GameObject laser = Instantiate(projectile,startPosition,Quaternion.identity) as GameObject;
		laser.GetComponent<Rigidbody2D>().velocity = new Vector3(0, -projectileSpeed, 0);
		AudioSource.PlayClipAtPoint(fireSoundEnemy,this.transform.position);
	}
}