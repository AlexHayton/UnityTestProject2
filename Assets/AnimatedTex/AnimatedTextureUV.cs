using UnityEngine;

public class AnimatedTextureUV : MonoBehaviour
{
    public int FrameCount;
    public int FPS;
    public bool Reverse;
    public bool Repeat;
    private float startTime;
    private float widthOfEachImage;

    void Start()
    {
        transform.rotation = Quaternion.FromToRotation(transform.up, Vector3.up);
        gameObject.transform.Rotate(Vector3.up,Random.Range(0,360));
        print("Camera: " + Camera.main.transform.forward);
        print("Plane: " + transform.up);
        startTime = Time.time;
        widthOfEachImage = 1.0f / FrameCount;
        renderer.material.SetTextureScale("_MainTex", new Vector2(widthOfEachImage, 1));
    }
    //Update
    void Update()
    {
        var elapsedTime = Time.time - startTime;
        if (!Repeat && FrameCount*1f/(FPS*elapsedTime) < 1f)
        {
            Destroy(gameObject);
            return;
        }
        int index;
        if (Reverse)
            index = FrameCount - (int)(elapsedTime * FPS) % FrameCount;
        else
            index = (int)(elapsedTime * FPS) % FrameCount;

        // Size of every cell

        renderer.material.SetTextureOffset("_MainTex", new Vector2(index * widthOfEachImage, 0)); // Which face should be displayed
    }
}