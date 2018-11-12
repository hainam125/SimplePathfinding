using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public Transform target;
    public float speed;
    public Vector3 dir;
    public bool aStar;

    private List<Player> players;
    private bool isMoving;
    private int currentIdx;
    private List<Vector3> path;

	private IEnumerator Start () {
        players = FindObjectsOfType<Player>().Where(x => x != this).ToList();
        yield return new WaitForSeconds(1f);
        var sw = new System.Diagnostics.Stopwatch();
        sw.Start();
        if (aStar)
        {
            var aNodePath = APathfinding.FindPath(transform.position, target.position);
            sw.Stop();
            if (aNodePath != null)
            {
                path = aNodePath.Select(x => x.worldPosition).ToList();
            }
        }
        else
        {
            var dNodePath = DPathfinding.FindPath(transform.position, target.position);
            sw.Stop();
            if (dNodePath != null)
            {
                path = dNodePath.Select(x => x.worldPosition).ToList();
            }
        }
        Debug.Log(sw.Elapsed.Milliseconds);
        if(path != null)
        {
            currentIdx = 0;
            isMoving = true;
        }
	}
	
	private void Update () {
        if (isMoving)
        {
            dir = (path[currentIdx] - transform.position).normalized;
            //var dist = (dir + Avoid() * 10.0f).normalized;
            var dist = dir.normalized;
            transform.position += dist * Time.deltaTime * speed;
            if (Vector3.Distance(transform.position, path[currentIdx]) <= 1.2f * dist.magnitude)
            {
                if (currentIdx == path.Count - 1)
                {
                    isMoving = false;
                }
                currentIdx++;
            }
        }
    }

    private Vector3 Avoid()
    {
        Vector3 total = Vector3.zero;

        foreach (var p in players)
        {
            var sqrMag = Vector3.SqrMagnitude(transform.position - p.transform.position);
            var near = 2.0f;
            if (sqrMag < near)
            {
                total += new Vector3(-dir.z, 0f , dir.x) * sqrMag / near;
            }
        }

        return (total / players.Count).normalized;
    }

    private Vector3 MoveAway()
    {
        Vector3 total = Vector3.zero;
        foreach(var p in players)
        {
            var sqrMag = Vector3.SqrMagnitude(transform.position - p.transform.position);
            var near = 2f;
            if (sqrMag > near)
            {
                total += (transform.position - p.transform.position) / sqrMag;
            }
        }
        return total;
    }
    
	void OnDrawGizmos() {
		//Gizmos.DrawWireCube(transform.position,new Vector3(1,1,1));

        if (path != null)
        {
            foreach (var n in path)
            {
                Gizmos.color = aStar ? Color.blue : Color.red;
                Gizmos.DrawCube(n, Vector3.one * (1f - .1f));
            }
        }
    }
}
