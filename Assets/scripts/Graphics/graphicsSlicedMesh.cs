using UnityEngine;
[ExecuteInEditMode]
public class graphicsSlicedMesh : MonoBehaviour
{
    private float oldBorder;
    private float oldWidth;
    private float oldHeight;
    private float oldMargin;
    private bool oldCentered;
    private Mesh mesh;

    public float _b = 0.1f;
    public float Border
    {
        get
        {
            return _b;
        }
        set
        {
            _b = value;
            CreateSlicedMesh();
        }
    }

    public float _w = 1.0f;
    public float Width
    {
        get
        {
            return _w;
        }
        set
        {
            _w = value;
            CreateSlicedMesh();
        }
    }

    public float _h = 1.0f;
    public float Height
    {
        get
        {
            return _h;
        }
        set
        {
            _h = value;
            CreateSlicedMesh();
        }
    }

    public float _m = 0.4f;
    public float Margin
    {
        get
        {
            return _m;
        }
        set
        {
            _m = value;
            CreateSlicedMesh();
        }
    }

    public bool _centered;
    public bool Centered
    {
        get { return _centered; }
        set
        {
            _centered = value;
            CreateSlicedMesh();
        }
    }

    void Awake()
    {
        mesh = new Mesh();
    }

    void Start()
    {
        
        oldBorder = Border;
        oldHeight = Height;
        oldMargin = Margin;
        oldWidth = Width;
        oldCentered = Centered;

        //_w = 1.0f / gameObject.transform.localScale.x;
        //_h = 1.0f / gameObject.transform.localScale.y;
        CreateSlicedMesh();
    }
#if UNITY_EDITOR
    void Update()
    {
        if (Border != oldBorder)
        {
            Border = _b;
            oldBorder = Border;
        }
        if (Height != oldHeight)
        {
            Height = _h;
            oldHeight = Height;
        }
        if (Width != oldWidth)
        {
            Width = _w;
            oldWidth = Width;
        }
        if (Margin != oldMargin)
        {
            Margin = _m;
            oldMargin = Margin;
        }
        if (Centered != oldCentered)
        {
            Centered = _centered;
            oldCentered = Centered;
        }
    }
#endif
    void CreateSlicedMesh()
    {
        GetComponent<MeshFilter>().sharedMesh = mesh;

        var vertices = new Vector3[]
        {
            new Vector3(0, 0, 0), new Vector3(_b, 0, 0), new Vector3(_w - _b, 0, 0), new Vector3(_w, 0, 0),
            new Vector3(0, _b, 0), new Vector3(_b, _b, 0), new Vector3(_w - _b, _b, 0), new Vector3(_w, _b, 0),
            new Vector3(0, _h - _b, 0), new Vector3(_b, _h - _b, 0), new Vector3(_w - _b, _h - _b, 0),
            new Vector3(_w, _h - _b, 0),
            new Vector3(0, _h, 0), new Vector3(_b, _h, 0), new Vector3(_w - _b, _h, 0), new Vector3(_w, _h, 0)
        };

        if (Centered)
        {
            var dx = -_w * 0.5f;
            var dy = -_h * 0.5f;
            for (var i = 0; i < vertices.Length; i++)
            {
                vertices[i].x += dx;
                vertices[i].y += dy;
            }
        }

        mesh.vertices = vertices;

        mesh.uv = new Vector2[] 
        {
            new Vector2(0, 0), new Vector2(_m, 0), new Vector2(1-_m, 0), new Vector2(1, 0),
            new Vector2(0, _m), new Vector2(_m, _m), new Vector2(1-_m, _m), new Vector2(1, _m),
            new Vector2(0, 1-_m), new Vector2(_m, 1-_m), new Vector2(1-_m, 1-_m), new Vector2(1, 1-_m),
            new Vector2(0, 1), new Vector2(_m, 1), new Vector2(1-_m, 1), new Vector2(1, 1)
        };

        mesh.triangles = new int[] 
        {
        0, 4, 5,
        0, 5, 1,
        1, 5, 6,
        1, 6, 2,
        2, 6, 7,
        2, 7, 3,
        4, 8, 9,
        4, 9, 5,
        5, 9, 10,
        5, 10, 6,
        6, 10, 11,
        6, 11, 7,
        8, 12, 13,
        8, 13, 9,
        9, 13, 14,
        9, 14, 10,
        10, 14, 15,
        10, 15, 11
        };
    }
}