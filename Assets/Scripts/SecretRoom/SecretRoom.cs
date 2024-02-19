using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SecterRoom : MonoBehaviour
{
    BoundsInt area;
    Tilemap tilemap;
    BoxCollider2D coll;
    [SerializeField] private GameObject hiddenRoomCover;
    //public AudioSource destroyWallSound;

    void Start()
    {
        tilemap = GameObject.FindGameObjectWithTag("HiddenRoom").GetComponent<Tilemap>();
        coll = GetComponent<BoxCollider2D>();

        Vector3Int position = Vector3Int.FloorToInt(coll.bounds.min);
        Vector3Int size = Vector3Int.FloorToInt(coll.bounds.size + new Vector3Int(0, 0, 1));
        area = new BoundsInt(position, size);

        foreach(Vector3Int point in area.allPositionsWithin)
        {
            tilemap.SetTileFlags(point, TileFlags.None);
            //tilemap.SetColor(point, new Color(255f, 255f, 255f, 0f));
            //tilemap.SetTileFlags(point, TileFlags.LockColor);
        }
    }

    void RevealRoom()
    {
        foreach(Vector3Int point in area.allPositionsWithin)
        {
            tilemap.SetTileFlags(point, TileFlags.None);
            //tilemap.SetColor(point, new Color(255f, 255f, 255f, 255f));
        }
        //destroyWallSound.Play();
        Destroy(hiddenRoomCover);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            RevealRoom();
        }
    }
}
