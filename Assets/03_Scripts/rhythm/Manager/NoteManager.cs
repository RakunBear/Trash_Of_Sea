using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoteData
{
    public float time;
    public string type;
}

[System.Serializable]
public class NoteList
{
    public List<NoteData> notes;
}

public class NoteManager : MonoBehaviour
{
    public int bpm = 0;
    /*double currentTime = 0d;*/

    [SerializeField] Transform LeftNoteAppearLocation = null;
    [SerializeField] Transform RightNoteAppearLocation = null;
    [SerializeField] GameObject leftNotePrefab = null; // ������ ����
    [SerializeField] GameObject rightNotePrefab = null; // ������ ����

    TimingManager timingManager;
    CubeGenerator cubeGenerator;

    public string notesJsonPath = "Assets/Notes/notes.json"; // JSON ������ ���

    void Start()
    {
        timingManager = GetComponent<TimingManager>();
        cubeGenerator = FindObjectOfType<CubeGenerator>();

        LoadNotes();
    }

    void LoadNotes()
    {
        string json = System.IO.File.ReadAllText(notesJsonPath);
        NoteList noteList = JsonUtility.FromJson<NoteList>(json);

        foreach (NoteData noteData in noteList.notes)
        {
            StartCoroutine(CreateNoteDelayed(noteData.time, noteData.type));
        }
    }

    IEnumerator CreateNoteDelayed(float time, string type)
    {
        yield return new WaitForSeconds(time);

        CreateNote(type);
    }

    void CreateNote(string type)
    {
        GameObject t_note = null;

        // Ÿ�Կ� ���� �ٸ� ������ ���
        if (type == "lt")
        {
            t_note = Instantiate(leftNotePrefab, LeftNoteAppearLocation.position, Quaternion.identity);
            cubeGenerator.Create_Cube_Lt(new Vector3(-5, -50, 500));
        }
        else if (type == "rt")
        {
            t_note = Instantiate(rightNotePrefab, RightNoteAppearLocation.position, Quaternion.identity);
            cubeGenerator.Create_Cube_Rt(new Vector3(50, -50, 500));
        }

        if (t_note != null)
        {
            t_note.transform.SetParent(this.transform);
            timingManager.boxNoteList.Add(t_note);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Note"))
        {
            timingManager.boxNoteList.Remove(collision.gameObject);
            Destroy(collision.gameObject);
        }
    }

}
