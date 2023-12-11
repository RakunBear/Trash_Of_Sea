using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutGameMode : MonoBehaviour
{
    public Transform SpawnPoint;
    [SerializeField]
    private CutBook cutBook;

    private void Start() {
            StartCoroutine(LoadBook(DBManager.TargetBookName));
    }

    public IEnumerator LoadBook(string bookName) {
        GameObject book = Resources.Load<GameObject>(bookName);
        GameObject bookObj = Instantiate(book, SpawnPoint);
        bookObj.TryGetComponent(out cutBook);

        if (cutBook) {
            yield return cutBook.Playing();
            yield return 3;
            SoundBox.Box.ChangeBGM(null);
            LoadingScene.LoadScene(DBManager.NextSceneName);
        }
    }
}
