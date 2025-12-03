using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Centipede : MonoBehaviour
{
    private List<CentipedeSegment> segments = new List<CentipedeSegment>();

    private scoretest scoretest => FindAnyObjectByType<scoretest>();

    public CentipedeSegment segmentPrefab;
    public Mushroom mushroomPrefab;

    public LayerMask collisionMask;
    public BoxCollider homeArea;

    public int size = 12;

    public float speed = 5f;

    private int level = 0;

    [SerializeField]
    private AudioController AC;

    private void Start()
    {
        Respawn();
        StartCoroutine(DelayedWhileLoop());
    }

    public void Respawn()
    {
        foreach (CentipedeSegment segment in segments)
        {
            Instantiate(mushroomPrefab, GridPosition(segment.transform.position), Quaternion.Euler(90f, 0f, 0f));
            Destroy(segment.gameObject);
        }

        segments.Clear();

        for (int i = 0; i < size; i++)
        {
            Vector3 position = transform.position + (Vector3.left * i);
            CentipedeSegment newSegment = Instantiate(segmentPrefab, position, Quaternion.identity);
            newSegment.centipede = this;
            segments.Add(newSegment);
        }

        for (int i = 0; i < segments.Count; i++)
        {
            CentipedeSegment segment = segments[i];
            segment.ahead = GetSegmetAt(i-1);
            segment.behind = GetSegmetAt(i+1);
        }
    }

    public void Remove(CentipedeSegment segment)
    {
        //instatiate mushroom here
        Instantiate(mushroomPrefab, GridPosition(segment.transform.position), Quaternion.Euler(90f, 0f, 0f));
        scoretest.scoreUpdate(10);

        if (segment.ahead != null)
            segment.ahead.behind = null;

        if (segment.behind != null)
        {
            segment.behind.ahead = null;
            segment.behind.UpdateHeadSegement();
        }

        //removes segment
        if (AC != null)
            AC.PlayVol(AC.centipedeAudioClips, AC.centipedeAudioClips.Count - 1, 1);

        segments.Remove(segment);
        Destroy(segment.gameObject);
    }

    private CentipedeSegment GetSegmetAt(int index)
    {
        if (index >= 0 && index < segments.Count)
            return segments[index];
        else return null;
    }

    private Vector3 GridPosition(Vector3 position)
    {
        position = new Vector3(Mathf.Round(position.x), 0.5f, Mathf.Round(position.z));
        return position;
    }

    private void Update()
    {
        if (segments.Count == 0)
        {
            scoretest.scoreUpdate(50);
            level++;
            speed += 5f;
            size += level;
            Respawn();
        }
    }
    private IEnumerator DelayedWhileLoop()
    {
        while (segments.Count > 0)
        {
            Debug.Log("Play sound");
            AC.PlayVol(AC.centipedeAudioClips, Random.Range(0, AC.centipedeAudioClips.Count - 1), 1);
            yield return new WaitForSeconds(0.1f);
            Debug.Log("Sound is over");
        }
    }
}
