using UnityEngine;

using System.Collections;

[ExecuteInEditMode]
public class UvControll : MonoBehaviour {
	private Mesh mesh;
	public int xIndex,yIndex;
    public int xMax { get { return (int)(renderer.sharedMaterial.mainTexture.width / uvRect.width); } }
    public int yMax { get { return (int)(renderer.sharedMaterial.mainTexture.height/ uvRect.height); } }

    private int oldxIndex = -1,oldyIndex = -1;
    private bool isOldFlip = false;
	private float alpha = 1;
	public bool isFlip = false;
	public Rect uvRect;
	public Color vertexColor = Color.white;
	private Color oldVertexColor = Color.clear;
	private Camera billboradCamera;

    private Renderer _renderer;
    public Renderer renderer
    {
        get { return _renderer == null ? (_renderer = GetComponent<Renderer>()) : _renderer; }
    }

	void Start () {
		//ビルボードターゲッt用のカメラを取得しておく//
		billboradCamera = Camera.main;
		if(billboradCamera == null)billboradCamera = Camera.current;
	}

	void Awake (){
		mesh = GetComponent<MeshFilter>().sharedMesh;
	}
	
	//外から色を変更できるよう//
	public void setColor(Color color){
		vertexColor = color;
	}
	
	public void setUvForce(){	//強制的にUVをセットするようにする//
		oldxIndex = -1;
	}
	
	public void setUV(){
		if(xIndex == oldxIndex && yIndex == oldyIndex && isFlip == isOldFlip)return;	//同じインデックスなので処理しない//
		Texture tex = renderer.sharedMaterial.mainTexture;
		if(tex == null)return;	//テクスチャがないので、そもそも処理しない//
		oldxIndex = xIndex;
		oldyIndex = yIndex;
	    isOldFlip = isFlip;
		
		Vector2[] uv = mesh.uv;
		
		//それぞれUV座標に変換
		float u = this.uvRect.x / tex.width;
		float v = this.uvRect.y / tex.height;
		float w = this.uvRect.width / tex.width;
		float h = this.uvRect.height / tex.height;
		
		Vector2 v0 = new Vector2(u + xIndex * w,1 - (v + yIndex * h));
		Vector2 v1 = new Vector2(u + xIndex * w + w,1 - (v + yIndex * h) );
		Vector2 v2 = new Vector2(u + xIndex * w,1 - (v + yIndex * h + h));
		Vector2 v3 = new Vector2(u + xIndex * w + w ,1 - (v + yIndex * h + h));

        //Quadに合わせたUV
	    if (isFlip)
	    {
	        uv[3] = v1;
	        uv[1] = v0;
	        uv[0] = v3;
	        uv[2] = v2;
        }
        else
	    {
	        uv[3] = v0;
	        uv[1] = v1;
	        uv[0] = v2;
	        uv[2] = v3;
	    }

        mesh.uv = uv;
	}
	
	void setColor(){
		if(this.vertexColor == this.oldVertexColor)return;	//すでに同じ色がセットされているぽいなら処理しない//
		oldVertexColor = vertexColor;
		Color[] color = mesh.colors;
		if(color.Length == 0)color = new Color[mesh.vertexCount];	//なければ作るのみ//
		for(int i = 0;i < color.Length;i++){
			color[i] = this.vertexColor;
		}
		mesh.colors = color;
	}
	
	public void setBillboard(){
		//カメラの向いている方向と逆方向を見る。　カメラ自体を見ると、横切る時がおかしいので、//
		if(billboradCamera != null){
			transform.LookAt(transform.position + billboradCamera.transform.forward,Vector3.right);	//縦画面用//
		}
	}
	
	void LateUpdate(){
		setUV();
		setColor();
	}
}
