using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomController : MonoBehaviour {
    private List<Room> rooms = new List<Room>();

    void Awake()
    {
        foreach (Transform child in this.transform)
        {
            this.rooms.Add(child.GetComponent<Room>());
        }
    }

    public Room GetRandomRoom()
    {
        int randomInt = Random.Range(0, rooms.Count - 1);
        return rooms[randomInt];
    }
}
