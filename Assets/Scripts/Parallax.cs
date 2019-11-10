using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform background;
    public float speed;

    private Transform camera;
    private Vector3 previewCamPosition;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main.transform;
        previewCamPosition = camera.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        float parallaxX = previewCamPosition.x - camera.position.x;
        float bgTargetX = background.position.x + parallaxX;

        Vector3 bgPosition = new Vector3(bgTargetX, background.position.y, background.position.z);
        background.position = Vector3.Lerp(background.position, bgPosition, speed * Time.deltaTime);

        previewCamPosition = camera.position;
    }
}
