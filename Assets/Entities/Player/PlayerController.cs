using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float speed = 15f;
	public float padding = 1;
	public float health = 500f;
	public GameObject projectile;
	public float projectileSpeed;
	public float firingRate = 0.2f;
	public AudioClip fireSounds;

	public GameObject lifeRemainingText;

	float xmin;
	float xmax;
	
	void Start(){
		float distance = transform.position.z - Camera.main.transform.position.z;
		//bottom left
		Vector3 leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));	
		//bottom right
		Vector3 rightmost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));	
		xmin = leftmost.x + padding;
		xmax = rightmost.x - padding;
	}

	void Fire(){
		Vector3 startPosition = this.transform.position + new Vector3(0,1, 0);
		GameObject beam = Instantiate(projectile,startPosition,Quaternion.identity) as GameObject;
		beam.GetComponent<Rigidbody2D>().velocity = new Vector3(0,projectileSpeed,0);
		AudioSource.PlayClipAtPoint(fireSounds, this.transform.position);
	}
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			InvokeRepeating("Fire", 0.000001f, firingRate);
		}
		if (Input.GetKeyUp(KeyCode.Space)) {
			CancelInvoke("Fire");
		}
		if(Input.GetKey(KeyCode.LeftArrow)){
			this.transform.position += new Vector3(-speed * Time.deltaTime, 0, 0);
		}
		if(Input.GetKey(KeyCode.RightArrow)){
			this.transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
		}
		
		float newX = Mathf.Clamp(transform.position.x, xmin, xmax); 
		this.transform.position = new Vector3(newX, transform.position.y, 0);

	}
	void OnTriggerEnter2D(Collider2D collider){
		Projectile missile = collider.gameObject.GetComponent<Projectile> ();
		if (missile) {
			missile.Hit();
			health -= missile.GetDamage();
			if (health <= 0){
				Destroy(this.gameObject);
				Application.LoadLevel("Win Screen");
			}
			if(health/100 != 0){
				lifeRemainingText.GetComponent<Text>().text = "X " + (health/100 - 1);
			}
			//print ("Hit by projectile");
		}
	}
}
