using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using AnotherFileBrowser.Windows;

public class EditorController : MonoBehaviour
{
    public static EditorController Instance;

    public bool isPosition { get; set; }
    public GameObject RotateTool = null;
    public CharacterValues characterValues = null;
    [SerializeField] GameObject currentSelectedObject = null;

    private void Start()
    {
        Instance = this;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray mouse_pos = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouse_pos, out hit))
            {
                if (hit.collider.gameObject.GetComponent<Interactable>())
                {
                    hit.collider.gameObject.GetComponent<Interactable>().ToggleSelect();

                    if (currentSelectedObject != null)
                        currentSelectedObject.GetComponent<Interactable>().ToggleSelect();
                    currentSelectedObject = hit.collider.gameObject;
                }
            }
        }
    }

    public static void ClearTools()
    {
        foreach (var item in FindObjectsOfType<ToolInitializer>())
        {
            Destroy(item.gameObject);
        }
    }

    public void UpdateTools()
    {
        foreach (var item in FindObjectsOfType<ToolInitializer>())
        {
            item.UpdateTools();
        }
    }

    public Vector2[] GetModelPositions()
    {
        List<Vector2> positions = new List<Vector2>();
        positions.Add(new Vector2(characterValues.Head.localPosition.x, characterValues.Head.localPosition.y));
        positions.Add(new Vector2(characterValues.RightArm.localPosition.x, characterValues.RightArm.localPosition.y));
        positions.Add(new Vector2(characterValues.LeftArm.localPosition.x, characterValues.LeftArm.localPosition.y));
        positions.Add(new Vector2(characterValues.RightLeg.localPosition.x, characterValues.RightLeg.localPosition.y));
        positions.Add(new Vector2(characterValues.LeftLeg.localPosition.x, characterValues.LeftLeg.localPosition.y));
        positions.Add(new Vector2(characterValues.Body.localPosition.x, characterValues.Body.localPosition.y));

        return positions.ToArray();
    }

    public float[] GetModelRotations()
    {
        List<float> rotations = new List<float>();
        rotations.Add(characterValues.Head.localEulerAngles.z);
        rotations.Add(characterValues.LeftArm.localEulerAngles.z);
        rotations.Add(characterValues.RightArm.localEulerAngles.z);
        rotations.Add(characterValues.LeftLeg.localEulerAngles.z);
        rotations.Add(characterValues.RightLeg.localEulerAngles.z);
        rotations.Add(characterValues.Body.localEulerAngles.z);

        return rotations.ToArray();
    }

    public void SaveAnimation(string AnimationName)
    {
        string savepath = "";

        var bp = new BrowserProperties();
        bp.filter = "txt files (*.txt)|*.txt";
        bp.filterIndex = 0;

        new FileBrowser().SaveFileBrowser(bp, "animation", ".txt", path =>
        {
            savepath = path;
        });

        CachedJsonAnimation cachedJsonAnimation = new CachedJsonAnimation();
        cachedJsonAnimation.Keyframes = Animation.GetKeyFramesInJson(TimeLine.Instance.GetKeyFrames());

        File.WriteAllText(savepath, JsonUtility.ToJson(cachedJsonAnimation));
    }

    public void OpenAnimation()
    {
        string json = "";

        var bp = new BrowserProperties();
        bp.filter = "text files (*.txt)|*.txt";
        bp.filterIndex = 0;

        new FileBrowser().OpenFileBrowser(bp, path =>
        {
            json = File.ReadAllText(path);
        });

        if (json == string.Empty)
            return;

        CachedJsonAnimation cachedJsonAnimation = JsonUtility.FromJson<CachedJsonAnimation>(json);

        TimeLine.Instance.LoadNewTimeLine(Animation.GetKeyFramesFromJson(cachedJsonAnimation.Keyframes));
    }

}

[System.Serializable]
public class CachedJsonAnimation
{
    public string Keyframes;
}
