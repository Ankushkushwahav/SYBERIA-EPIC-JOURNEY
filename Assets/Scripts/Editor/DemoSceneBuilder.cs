#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SyberiaEpic;

public static class DemoSceneBuilder
{
    [MenuItem("SYBERIA/Create Demo Scene")]
    public static void Create()
    {
        foreach (var go in Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None)) Object.DestroyImmediate(go);
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        var state = new GameObject("GameState"); state.AddComponent<GameState>();
        new GameObject("PlayerInput").AddComponent<PlayerInput>();
        new GameObject("MissionManager").AddComponent<MissionManager>();

        Material groundMat = Mat(new Color(.28f,.27f,.24f));
        Material wallMat = Mat(new Color(.43f,.34f,.27f));
        Material roofMat = Mat(new Color(.18f,.22f,.25f));
        Material goldMat = Mat(new Color(.72f,.48f,.12f));
        Material lampMat = Mat(new Color(1f,.72f,.25f,1f));

        Cube("Market Plaza", new Vector3(0,-.5f,0), new Vector3(38,1,38), groundMat);
        for (int i=0;i<8;i++)
        {
            float x = (i%2==0 ? -1 : 1) * (8 + (i/2)*2);
            float z = -13 + (i/2)*8;
            Cube("Old Bazaar Building "+i, new Vector3(x,2.5f,z), new Vector3(6,5,6), wallMat);
            Cube("Slate Roof "+i, new Vector3(x,5.4f,z), new Vector3(6.6f,.5f,6.6f), roofMat);
            Cube("Shop Sign "+i, new Vector3(x,3.4f,z + (x<0?3.1f:-3.1f)), new Vector3(2,.7f,.15f), goldMat);
        }
        for(int i=-2;i<=2;i++)
        {
            var lamp=GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            lamp.name="Bazaar Lamp"; lamp.transform.position=new Vector3(i*5,1.8f,4); lamp.transform.localScale=new Vector3(.12f,1.8f,.12f);
            lamp.GetComponent<Renderer>().material=roofMat;
            var bulb=GameObject.CreatePrimitive(PrimitiveType.Sphere); bulb.transform.position=new Vector3(i*5,3.8f,4); bulb.transform.localScale=Vector3.one*.35f; bulb.GetComponent<Renderer>().material=lampMat;
            var light=bulb.AddComponent<Light>(); light.type=LightType.Point; light.range=7; light.intensity=1.4f; light.color=new Color(1,.68f,.35f);
        }
        var fountain=GameObject.CreatePrimitive(PrimitiveType.Cylinder); fountain.name="Old Stone Fountain"; fountain.transform.position=new Vector3(0,.2f,9); fountain.transform.localScale=new Vector3(3,.5f,3); fountain.GetComponent<Renderer>().material=roofMat;

        var train=GameObject.CreatePrimitive(PrimitiveType.Cube); train.name="Decorative Steam Tram"; train.transform.position=new Vector3(0,1.2f,-15); train.transform.localScale=new Vector3(4,2.2f,7); train.GetComponent<Renderer>().material=roofMat;
        var chimney=GameObject.CreatePrimitive(PrimitiveType.Cylinder); chimney.transform.position=new Vector3(0,3,-16.5f); chimney.transform.localScale=new Vector3(.65f,1.2f,.65f); chimney.GetComponent<Renderer>().material=goldMat;

        var player=GameObject.CreatePrimitive(PrimitiveType.Capsule); player.name="Player"; player.tag="Player"; player.transform.position=new Vector3(0,1,0);
        Object.DestroyImmediate(player.GetComponent<CapsuleCollider>());
        var cc=player.AddComponent<CharacterController>(); cc.height=2; cc.radius=.45f; cc.center=new Vector3(0,1,0);
        player.AddComponent<PlayerController>();

        var key=GameObject.CreatePrimitive(PrimitiveType.Cylinder); key.name="Lost Brass Clockwork Key"; key.transform.position=new Vector3(2,.45f,2); key.transform.localScale=new Vector3(.18f,.06f,.7f); key.transform.rotation=Quaternion.Euler(0,0,90); key.GetComponent<Renderer>().material=goldMat; key.AddComponent<LostMechanicalKey>();

        var marker=GameObject.CreatePrimitive(PrimitiveType.Cube); marker.name="Mission Return Marker"; marker.transform.position=new Vector3(0,.2f,9); marker.transform.localScale=new Vector3(1,.4f,1); marker.GetComponent<Renderer>().material=goldMat;
        var finish=marker.AddComponent<ChapterFinishMarker>(); finish.prompt="Return here after finding the key";

        var cameraGO=new GameObject("Main Camera"); cameraGO.tag="MainCamera"; var cam=cameraGO.AddComponent<Camera>(); cam.fieldOfView=65; cameraGO.AddComponent<AudioListener>();
        var follow=cameraGO.AddComponent<CameraFollow>(); follow.target=player.transform;

        var sun=new GameObject("Sun"); var dl=sun.AddComponent<Light>(); dl.type=LightType.Directional; dl.intensity=1.15f; sun.transform.rotation=Quaternion.Euler(45,-35,0);
        RenderSettings.ambientLight=new Color(.48f,.48f,.52f);
        RenderSettings.fog=true; RenderSettings.fogColor=new Color(.55f,.61f,.68f); RenderSettings.fogDensity=.008f;

        var eventSystem=new GameObject("EventSystem"); eventSystem.AddComponent<EventSystem>(); eventSystem.AddComponent<StandaloneInputModule>();
        var canvasGO=new GameObject("Game UI",typeof(Canvas),typeof(CanvasScaler),typeof(GraphicRaycaster));
        var canvas=canvasGO.GetComponent<Canvas>(); canvas.renderMode=RenderMode.ScreenSpaceOverlay;
        var scaler=canvasGO.GetComponent<CanvasScaler>(); scaler.uiScaleMode=CanvasScaler.ScaleMode.ScaleWithScreenSize; scaler.referenceResolution=new Vector2(1920,1080);
        var hud=canvasGO.AddComponent<UIHud>(); hud.Build(canvas);
        MobileControls.Build(canvas);

        EditorSceneManager.SaveScene(scene,"Assets/Scenes/VaghenBazaar_Demo.unity");
        AssetDatabase.SaveAssets();
        Selection.activeGameObject=player;
        Debug.Log("SYBERIA demo scene created. Open Assets/Scenes/VaghenBazaar_Demo.unity and press Play.");
    }

    static Material Mat(Color c) { var m=new Material(Shader.Find("Universal Render Pipeline/Lit")); if(m.shader==null)m=new Material(Shader.Find("Standard")); m.color=c; return m; }
    static GameObject Cube(string n,Vector3 p,Vector3 s,Material m){var g=GameObject.CreatePrimitive(PrimitiveType.Cube);g.name=n;g.transform.position=p;g.transform.localScale=s;g.GetComponent<Renderer>().material=m;return g;}
}

public class ChapterFinishMarker : Interactable
{
    public override void Interact() { MissionManager.Instance?.FinishChapter(); }
}
#endif
