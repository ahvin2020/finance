using UnityEngine;

public class PoolObject : MonoBehaviour
{
	[Header("Pool Info")]
	[SerializeField]
	DynamicPoolSO dynamicPoolSO;

	[SerializeField]
	private string poolObjectId;

	[SerializeField]
	private int countLimit = 500;
	
	public string PoolObjectId {
		get {
			if (string.IsNullOrEmpty(poolObjectId)) {
				Debug.LogError("Pool object id does not have poolObjectId");
			}

			return poolObjectId;
		}
	}

	public int CountLimit {
		get {
			return countLimit;
		}
	}

	public void ReturnToPool() {
		dynamicPoolSO.ReturnPoolObject(this);
	}
}