using UnityEngine;
using System.Collections;

public class ItemDropLogic : MonoBehaviour {
	
	public GameObject[] items;
	public bool randomDrop = true;
	public int dropPercentage = 30;
	
	private float realDropPercantage;
		

	// Use this for initialization
	void Start () {
		realDropPercantage = (float)dropPercentage / 100f;
	}
	
	
	public void DropItems() {
		if (randomDrop) {

			if (Random.value <= realDropPercantage) {
				// drop one item from the list
				Instantiate(items[Random.Range (0, items.Length)], transform.position, Quaternion.identity);	
			}
			
		} else {
			foreach (GameObject item in items) {
				Instantiate(item, transform.position, Quaternion.identity);
			}			
		}
	}
	
	
}
