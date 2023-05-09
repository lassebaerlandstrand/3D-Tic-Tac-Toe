using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaskUI : MaskableGraphic
{
    [SerializeField] private float initialWidth;
    [SerializeField] private float height;
    [SerializeField] private float extraTriangleLength = 25f;
    float extraSpace = 20f;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        Vector3 vec_00 = new Vector3(0, 0);
        Vector3 vec_01 = new Vector3(0, height);
        Vector3 vec_10 = new Vector3(initialWidth - extraSpace - extraTriangleLength, 0);
        Vector3 vec_11 = new Vector3(initialWidth - extraSpace + extraTriangleLength, height); // Add extraTriangleHeight since we have to account for the health ui

        vh.AddUIVertexQuad(new UIVertex[] {
            new UIVertex {position = vec_00, color = Color.green},
            new UIVertex {position = vec_01, color = Color.green},
            new UIVertex {position = vec_11, color = Color.green},
            new UIVertex {position = vec_10, color = Color.green},
        });

    }

    public void updateVertices()
    {
        SetVerticesDirty();
    }

    public float triangleWidth()
    {
        return extraSpace + extraTriangleLength;
    }

    public float getAngleExtraTriangle()
    {
        return Mathf.Atan(height / (extraTriangleLength * 2));
    }

    protected override void Start()
    {
        updateVertices();
    }
}
