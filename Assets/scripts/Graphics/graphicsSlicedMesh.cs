using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[ExecuteInEditMode]
public class graphicsSlicedMesh : MonoBehaviour
{

#if UNITY_EDITOR
    private float oldBorder;
    private float oldWidth;
    private float oldHeight;
    private float oldMargin;
    private bool oldCentered;
    private float oldFillAmount;
#endif

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

    public float _fillAmount = 1;
    public float FillAmount
    {
        get { return _fillAmount; }
        set
        {
            _fillAmount = value;
            CreateSlicedMesh();
        }
    }

    void Awake()
    {
        if (!mesh)
            mesh = new Mesh();
    }

    void Start()
    {

#if UNITY_EDITOR
        oldBorder = Border;
        oldHeight = Height;
        oldMargin = Margin;
        oldWidth = Width;
        oldCentered = Centered;
        oldFillAmount = FillAmount;
#endif

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
        if (FillAmount != oldFillAmount)
        {
            FillAmount = _fillAmount;
            oldFillAmount = FillAmount;
        }
    }
#endif

    private void CreateSlicedMesh()
    {
        if (!mesh)
            mesh = new Mesh();

        GetComponent<MeshFilter>().sharedMesh = mesh;

        var y = _fillAmount * _h;
        if (_b <= 0)
            CreateQuad();
        else if (y <= _b)
            CreateTopOnly();
        else if (y <= (_h - _b))
            CreateTopAndMid();
        else
            CreateAll();
    }

    private void CreateQuad()
    {
        mesh.Clear();
        var vertices = new[] { new Vector3(0, 0, 0), new Vector3(_w, 0, 0), new Vector3(0, _h, 0), new Vector3(_w, _h, 0) };
        if (Centered)
            Center(ref vertices);

        mesh.vertices = vertices;
        mesh.uv = new[] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1), new Vector2(1, 1) };
        mesh.triangles = new[] { 0, 2, 3, 0, 3, 1 };
    }

    private void CreateTopOnly()
    {
        mesh.Clear();

        var y = _fillAmount * _h;
        var vertices = new []
        {
            new Vector3(0, 0, 0), new Vector3(_b, 0, 0), new Vector3(_w - _b, 0, 0), new Vector3(_w, 0, 0),
            new Vector3(0, y, 0), new Vector3(_b, y, 0), new Vector3(_w - _b, y, 0), new Vector3(_w, y, 0)
        };

        if (Centered)
            Center(ref vertices);

        mesh.vertices = vertices;

        var v = (_b > 0) ? (y / _b) * _m : 0;
        mesh.uv = new[]
        {
            new Vector2(0, 0), new Vector2(_m, 0), new Vector2(1-_m, 0), new Vector2(1, 0),
            new Vector2(0, v), new Vector2(_m, v), new Vector2(1-_m, v), new Vector2(1, v)
        };

        mesh.triangles = new [] 
        {
            0, 4, 5,
            0, 5, 1,
            1, 5, 6,
            1, 6, 2,
            2, 6, 7,
            2, 7, 3,
        };
    }

    private void CreateTopAndMid()
    {
        mesh.Clear();

        var y = _fillAmount * _h;

        var vertices = new[]
        {
            new Vector3(0, 0, 0), new Vector3(_b, 0, 0), new Vector3(_w - _b, 0, 0), new Vector3(_w, 0, 0),
            new Vector3(0, _b, 0), new Vector3(_b, _b, 0), new Vector3(_w - _b, _b, 0), new Vector3(_w, _b, 0),
            new Vector3(0, y, 0), new Vector3(_b, y, 0), new Vector3(_w - _b, y, 0), new Vector3(_w, y, 0)
        };

        if (Centered)
            Center(ref vertices);

        mesh.vertices = vertices;

        var s = y / _h;
        if (_b > 0)
            s = ((y - _b) / (_h - 2 * _b)) * _m;

        var v = _m + s * (1 - 2 * _m);
        mesh.uv = new[]
        {
            new Vector2(0, 0), new Vector2(_m, 0), new Vector2(1-_m, 0), new Vector2(1, 0),
            new Vector2(0, _m), new Vector2(_m, _m), new Vector2(1-_m, _m), new Vector2(1, _m),
            new Vector2(0, v), new Vector2(_m, v), new Vector2(1-_m, v), new Vector2(1, v),
        };

        mesh.triangles = new[]
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
        };
    }

    private void CreateAll()
    {
        mesh.Clear();

        var y = _fillAmount * _h;
        var vertices = new []
        {
            new Vector3(0, 0, 0), new Vector3(_b, 0, 0), new Vector3(_w - _b, 0, 0), new Vector3(_w, 0, 0),
            new Vector3(0, _b, 0), new Vector3(_b, _b, 0), new Vector3(_w - _b, _b, 0), new Vector3(_w, _b, 0),
            new Vector3(0, _h - _b, 0), new Vector3(_b, _h - _b, 0), new Vector3(_w - _b, _h - _b, 0), new Vector3(_w, _h - _b, 0),
            new Vector3(0, y, 0), new Vector3(_b, y, 0), new Vector3(_w - _b, y, 0), new Vector3(_w, y, 0)
        };

        if (Centered)
            Center(ref vertices);

        mesh.vertices = vertices;

        var v = 1.0f;
        if (_b > 0)
        {
            var f = (y - (_h - _b)) / _b;
            v = (1 - _m) + _m * f;
        }

        mesh.uv = new[]
        {
            new Vector2(0, 0), new Vector2(_m, 0), new Vector2(1-_m, 0), new Vector2(1, 0),
            new Vector2(0, _m), new Vector2(_m, _m), new Vector2(1-_m, _m), new Vector2(1, _m),
            new Vector2(0, 1-_m), new Vector2(_m, 1-_m), new Vector2(1-_m, 1-_m), new Vector2(1, 1-_m),
            new Vector2(0, v), new Vector2(_m, v), new Vector2(1-_m, v), new Vector2(1, v)
        };

        mesh.triangles = new [] 
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

    private void Center(ref Vector3[] vertices)
    {
        var dx = -_w * 0.5f;
        var dy = -_h * 0.5f;
        for (var i = 0; i < vertices.Length; i++)
        {
            vertices[i].x += dx;
            vertices[i].y += dy;
        }
    }
}