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

    private bool GameOver = false;

    private void Start()
    {
        GameOver = false;

        Respawn();

        if (AC != null)
        StartCoroutine(DelayedWhileLoop());

        Player.mOnGameOver += OnGameOver;
    }

    private void OnDisable()
    {
        Player.mOnGameOver -= OnGameOver;
    }

    public void Respawn()
    {
        foreach (CentipedeSegment segment in segments)
        {
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
        Instantiate(mushroomPrefab, GridPosition(segment.transform.position), Quaternion.Euler(0f, 0f, 0f));

        if (segment.ahead != null)
            segment.ahead.behind = null;

        if (segment.behind != null)
        {
            segment.behind.ahead = null;
            segment.behind.UpdateHeadSegement();
        }

        segments.Remove(segment);
        Destroy(segment.gameObject);

        //removes segment
        if (AC != null)
            AC.PlayNormal(AC.centipedeHit, 2);

        scoretest.scoreUpdate(10);
        if (segments.Count == 0)
        {
            Restart();
        }
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

    private void Restart()
    {
            scoretest.scoreUpdate(50);
            level++;
            speed += 2f;
            size += level;
            Respawn();
    }

    private void OnGameOver()
    {
        GameOver = true;
        speed = 0;
        StopCoroutine(DelayedWhileLoop());
    }

    private IEnumerator DelayedWhileLoop()
    {
        while (segments.Count > 0 && GameOver != true)
        {
            //Debug.Log("Play sound");
            AC.PlayRandom(AC.centipedeSounds, Random.Range(0, AC.centipedeSounds.Count), 2);
            yield return new WaitForSeconds(0.3f);
            //Debug.Log("Sound is over");
        }
    }
}
