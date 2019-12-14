using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//CREATED BY JEFFREY WONG

[CreateAssetMenu(fileName = "Dynamic Pool SO", menuName = "Dynamic Pool SO")] // Adds the PlayerData as an asset on your asset menu
public class DynamicPoolSO : ScriptableObject
{
	private bool isInit;
	private Dictionary<string, Queue<PoolObject>> idlePool;
	private Dictionary<string, HashSet<PoolObject>> inUsePool;
	private System.Random random;

	//Init to reset all values as scriptable object on editor will still keep the previous gameplay data
	public void ResetState() {
		isInit = true;
		idlePool = new Dictionary<string, Queue<PoolObject>>();
		inUsePool = new Dictionary<string, HashSet<PoolObject>>();
		random = new System.Random();
	}

	public T GetPoolObject<T>(PoolObject prefab, Transform parentTransform) where T : Component {
		string poolObjectId = prefab.PoolObjectId;
		
		if (!idlePool.ContainsKey(poolObjectId)) {
			idlePool.Add(poolObjectId, new Queue<PoolObject>());
		}

		if (!inUsePool.ContainsKey(poolObjectId)) {
			inUsePool.Add(poolObjectId, new HashSet<PoolObject>());
		}

		PoolObject poolObject;

		// exceed pool count? reuse from inUse pool
		if (inUsePool.Count >= prefab.CountLimit) {
			poolObject = inUsePool[poolObjectId].ElementAt(random.Next(inUsePool[poolObjectId].Count));
			inUsePool[poolObjectId].Remove(poolObject);
		} else if (idlePool[poolObjectId].Count > 0) {
			poolObject = idlePool[poolObjectId].Dequeue();
		} else {
			poolObject = Instantiate(prefab);
		}

		poolObject.transform.SetParent(parentTransform);
		poolObject.gameObject.SetActive(true);
		inUsePool[poolObjectId].Add(poolObject);

		return poolObject.GetComponent<T>();
	}

	public void ReturnAllPoolObjects(string poolObjectId) {
		if (!inUsePool.ContainsKey(poolObjectId)) {
			//Debug.LogError("Unable to return pool object, id not found: " + poolObjectId);
			return;
		}
		
		foreach (PoolObject poolObject in inUsePool[poolObjectId]) {
			poolObject.gameObject.SetActive(false);
			idlePool[poolObject.PoolObjectId].Enqueue(poolObject);
		}

		inUsePool[poolObjectId].Clear();
	}

	public void ReturnPoolObject(PoolObject poolObject) {
		string poolObjectId = poolObject.PoolObjectId;

		if (!inUsePool.ContainsKey(poolObjectId) || !idlePool.ContainsKey(poolObjectId)) {
			Debug.LogError("Unable to return pool object, id not found: " + poolObjectId);
			return;
		}

		poolObject.gameObject.SetActive(false);
		inUsePool[poolObjectId].Remove(poolObject);
		idlePool[poolObjectId].Enqueue(poolObject);
	}
}