using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
	public GameObject enemyPrefab;
	[Space(1)]
	[Header("Properties")]
	public float width = 10f;
	public float height = 5f;
	public float spawnDelay = 0.5f;
	[Range(.1f,5f)]
	public int speed = 5;

	float xmin;
	float xmax;

	// Use this for initialization
	void Start () {
		SpawnUntilFull();
		float distance = transform.position.z - Camera.main.transform.position.z;
		//bottom left
		Vector3 leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));	
		//bottom right
		Vector3 rightmost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));	
		xmin = leftmost.x + width / 2;
		xmax = rightmost.x - width / 2;
	}

	void SpawnEnemies (){
		foreach (Transform child in this.transform){
			GameObject enemy = Instantiate(enemyPrefab, child.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = child;
		}
	}

	void SpawnUntilFull() {
		//print ("inside SpawnUntilFull ");
		Transform freePosition = NextFreePosition();
		if (freePosition == null){
			CancelInvoke("SpawnUntilFull");
			return;
		}
		GameObject enemy = Instantiate(enemyPrefab, freePosition.position, Quaternion.identity) as GameObject;
		enemy.transform.parent = freePosition;
		Invoke("SpawnUntilFull", spawnDelay);
	}

	void OnDrawGizmos(){
		Gizmos.DrawWireCube (this.transform.position, new Vector3(width, height, 0));
	}

	// Update is called once per frame
	void Update () {
		if(this.transform.position.x + speed * Time.deltaTime > xmax || 
		   this.transform.position.x + speed * Time.deltaTime < xmin){
			speed *= -1;
		}
		this.transform.position += new Vector3 (speed * Time.deltaTime, 0, 0);

		if(AllMembersDead()){
			Debug.Log("Empty Formation");
			SpawnUntilFull();
		}
	}

	Transform NextFreePosition (){
		foreach (Transform childPositionGameObject in this.transform){
			if (childPositionGameObject.childCount == 0){
				return childPositionGameObject;
			}
		}
		return null;
	}

	bool AllMembersDead(){
		foreach (Transform childPositionGameObject in this.transform){
			if (childPositionGameObject.childCount != 0){
				return false;
			}
		}
		return true;
	}
}
