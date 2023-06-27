using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [Header("Data")]
    [SerializeField] PlayerData playerData;
    
    PlayerMove playerMove;


    private void Awake()
    {
        Instance = this;
        playerMove = GetComponent<PlayerMove>();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerMove.MoveStart(this, playerData);
    }

    // Update is called once per frame
    void Update()
    {
        playerMove.MoveUpdate();
    }
}
