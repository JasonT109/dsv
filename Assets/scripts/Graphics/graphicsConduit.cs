using UnityEngine;
[ExecuteInEditMode]
public class graphicsConduit : MonoBehaviour
{
#if UNITY_EDITOR
    private float oldWidth;
    private float oldHeight;
    private float oldInAngle;
    private float oldOutAngle;
    private Vector2 oldUVScale;
    private Vector2 oldUVOffset;
#endif

    private Mesh mesh;

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
            CreateMesh();
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
            CreateMesh();
        }
    }

    public float _inAngle = 0;
    public float InAngle
    {
        get
        {
            return _inAngle;
        }
        set
        {
            _inAngle = value;
            CreateMesh();
        }
    }

    public float _outAngle = 0;
    public float OutAngle
    {
        get
        {
            return _outAngle;
        }
        set
        {
            _outAngle = value;
            CreateMesh();
        }
    }

    public Vector2 _uvScale = Vector2.one;
    public Vector2 UVScale
    {
        get { return _uvScale; }
        set
        {
            _uvScale = value;
            CreateMesh();
        }
    }

    public Vector2 _uvOffset = Vector2.zero;
    public Vector2 UVOffset
    {
        get { return _uvOffset; }
        set
        {
            _uvOffset = value;
            CreateMesh();
        }
    }

    void Awake()
    {
        mesh = new Mesh();
    }

    void Start()
    {
#if UNITY_EDITOR
        oldWidth = Width;
        oldHeight = Height;
        oldInAngle = InAngle;
        oldOutAngle = OutAngle;
        oldUVScale = UVScale;
        oldUVOffset = UVOffset;
#endif

        CreateMesh();
    }

#if UNITY_EDITOR
    void Update()
    {
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
        if (InAngle != oldInAngle)
        {
            InAngle = _inAngle;
            oldInAngle = InAngle;
        }
        if (OutAngle != oldOutAngle)
        {
            OutAngle = _outAngle;
            oldOutAngle = OutAngle;
        }
        if (UVScale != oldUVScale)
        {
            UVScale = _uvScale;
            oldUVScale = UVScale;
        }
        if (UVOffset != oldUVOffset)
        {
            UVOffset = _uvOffset;
            oldUVOffset = UVOffset;
        }
    }
#endif

    void CreateMesh()
    {
        GetComponent<MeshFilter>().sharedMesh = mesh;

        var dy = _h * 0.5f;

        var dxIn = dy * Mathf.Tan(InAngle * Mathf.Deg2Rad);
        var dxOut = dy * Mathf.Tan(OutAngle * Mathf.Deg2Rad);

        var vertices = new []
        {
            new Vector3(dxIn, -dy, 0),
            new Vector3(_w + dxOut, -dy, 0),
            new Vector3(_w - dxOut, dy, 0),
            new Vector3(-dxIn, dy, 0),
        };

        mesh.vertices = vertices;

        mesh.uv = new []
        {
            _uvOffset + new Vector2(UVScale.x * dxIn, 0),
            _uvOffset + new Vector2(UVScale.x * (_w + dxOut), 0),
            _uvOffset + new Vector2(UVScale.x * (_w - dxOut), UVScale.y),
            _uvOffset + new Vector2(UVScale.x * -dxIn, UVScale.y),
        };

        mesh.triangles = new []
        {
            0, 1, 2,
            0, 2, 3
        };
    }

}