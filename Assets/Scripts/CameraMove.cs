using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private GameObject player;

    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    private void Update()
    {
        if (player != null)
            transform.position = (new Vector3(player.transform.position.x, player.transform.position.y, -10));
    }
}