using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarController : MonoBehaviour
{
    public Material fogOfWarMaterial; // �� �κ��� �߰��մϴ�.
    public Vector2 fogOfWarSize;
    public Transform player;
    public float viewRadius;
    public LayerMask fogLayer;

    private Texture2D fogOfWarTexture; // �� �κ��� private���� �����մϴ�.
    private Color[] fogPixels;
    private int textureSize;

    void Start()
    {
        textureSize = fogOfWarTexture.width;
        fogOfWarTexture = new Texture2D(textureSize, textureSize); // �߰��� �ڵ�
        fogPixels = fogOfWarTexture.GetPixels();
        fogOfWarMaterial.SetTexture("_FogOfWarTex", fogOfWarTexture);
    }

    void Update()
    {
        UpdateFog();
    }

    void UpdateFog()
    {
        Vector2 playerPosition = new Vector2(player.position.x, player.position.y);
        Vector2 fogOfWarPosition = new Vector2(transform.position.x, transform.position.y);

        int viewRadiusInPixels = Mathf.RoundToInt(viewRadius / fogOfWarSize.x * textureSize);

        for (int x = 0; x < textureSize; x++)
        {
            for (int y = 0; y < textureSize; y++)
            {
                int pixelIndex = y * textureSize + x;
                Vector2 pixelWorldPos = fogOfWarPosition + new Vector2((float)x / textureSize * fogOfWarSize.x, (float)y / textureSize * fogOfWarSize.y);

                float distanceToPlayer = Vector2.Distance(pixelWorldPos, playerPosition);
                if (distanceToPlayer <= viewRadius)
                {
                    fogPixels[pixelIndex].a = 0;
                }
            }
        }

        fogOfWarTexture.SetPixels(fogPixels);
        fogOfWarTexture.Apply();
    }
}
