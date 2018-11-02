using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public Transform target;
    public float speed;
    public Vector3 dir;

    private List<Player> players;
    private bool isMoving;
    private int currentIdx;
    private List<Vector3> path;

	private IEnumerator Start () {
        players = GameObject.FindObjectsOfType<Player>().Where(x => x != this).ToList();
        yield return new WaitForSeconds(1f);
        var nodePath = Pathfinding.FindPath(transform.position, target.position);
        if(nodePath != null)
        {
            path = nodePath.Select(x => x.worldPosition).ToList();
            currentIdx = 0;
            isMoving = true;
        }
	}
	
	private void Update () {
        if (isMoving)
        {
            dir = (path[currentIdx] - transform.position).normalized;
            var dist = (dir + Avoid() * 10.0f).normalized;
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
}
