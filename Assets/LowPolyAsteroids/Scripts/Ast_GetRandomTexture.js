var textures: Texture[];
private var rndNr:float;

function Awake () {

rndNr=Mathf.Floor(Random.value*textures.length);
renderer.material.mainTexture=textures[rndNr];
}

function Update () {
}