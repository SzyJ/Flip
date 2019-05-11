using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChar : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //if (!photonView.IsMine && GetComponent<Jump_Movement>() != null)
        //{
        //    Destroy(GetComponent<Jump_Movement>());
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void refreshInstance(ref PlayerChar player, PlayerChar prefab)
    {
        var position = Vector3.zero;
        var rotation = Quaternion.identity;

        if (player != null)
        {
            position = player.transform.position;
            rotation = player.transform.rotation;

            //PhotonNetwork.Destroy(player.gameObject);
        }

        //player = PhotonNetwork.Instantiate(prefab.gameObject.name, position, rotation).GetComponent<PlayerChar>();
    }
}
