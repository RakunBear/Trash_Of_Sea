using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteTimer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
            if (collision.CompareTag("Note"))
            {
                // Debug.Log(Time.time); // ť�� ���ƿ��� �ӵ� ���ϱ�� 
            }
    }
}
