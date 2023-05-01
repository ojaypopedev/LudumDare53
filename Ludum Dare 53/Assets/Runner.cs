using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner : MonoBehaviour
{
    public RunningTrack track;
     int Index = 5;

     float Speed = 5;
    public float rotationSpeed = 20;

    public CustomCharacter[] characters;

    private void Start()
    {
        int characterIndex = Random.Range(0, characters.Length);
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].gameObject.SetActive(i == characterIndex);
        }

        Index = Random.Range(0, 100);
        Speed = Random.Range(2f, 4.2f);
       transform.position = track.GetPoint(Index);
        transform.LookAt(track.GetPoint(Index + 1));

    }
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, track.GetPoint(Index), Speed * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(track.GetPoint(Index) - transform.position, Vector3.up), Time.deltaTime * rotationSpeed);

        if(Vector3.Distance(transform.position, track.GetPoint(Index)) < 0.5f)
        {
            Index++;
        }

    }
}
