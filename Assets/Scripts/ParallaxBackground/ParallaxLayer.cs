using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ParallaxLayer : MonoBehaviour
{
    public float parallaxFactor;
    public float imageWidth; // The width of one image in the layer

    private List<Transform> layerImages = new List<Transform>();

    void Start()
    {
        // Get all child images and store their transforms
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform image = transform.GetChild(i);
            layerImages.Add(image);
        }
    }

    public void Move(float delta)
    {
        Vector3 newPos = transform.localPosition;
        newPos.x -= delta * parallaxFactor;

        transform.localPosition = newPos;

        // Check and reposition images if they go out of view
        RepositionImages();
    }

    private void RepositionImages()
    {
        // Loop through each image in the layer
        for (int i = 0; i < layerImages.Count; i++)
        {
            Transform image = layerImages[i];

            // Check if the image is out of view
            if (IsOutOfView(image))
            {
                // Find the rightmost image
                Transform rightmostImage = GetRightmostImage();

                // Reposition the out-of-view image to the rightmost position
                image.localPosition = new Vector3(rightmostImage.localPosition.x + imageWidth, image.localPosition.y, image.localPosition.z);

                // Bring the repositioned image to the end of the list
                layerImages.Remove(image);
                layerImages.Add(image);
            }
        }
    }

    private bool IsOutOfView(Transform image)
    {
        float cameraLeftEdge = Camera.main.transform.position.x - Camera.main.orthographicSize * Camera.main.aspect;
        float imageRightEdge = image.position.x + imageWidth / 2;

        return imageRightEdge < cameraLeftEdge;
    }

    private Transform GetRightmostImage()
    {
        Transform rightmostImage = layerImages[0];

        foreach (Transform image in layerImages)
        {
            if (image.localPosition.x > rightmostImage.localPosition.x)
            {
                rightmostImage = image;
            }
        }

        return rightmostImage;
    }
}
