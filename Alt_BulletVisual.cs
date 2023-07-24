using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Alt_BulletVisual : MonoBehaviour
{
    public Alt_Bullet bulletData;
    private Vector3 currentPosition;


//Later: Maybe optimize this with a On Tick Listener
    void Update()
    {
        if(GameManager.Instance.GameState == GameState.MunitionFactory)
        {
            gameObject.SetActive(true);
            checkUpperAndLowerType();
            if (bulletData.currentTile.myRepresentation.transform.position != currentPosition)
            {
                currentPosition = bulletData.currentTile.myRepresentation.transform.position;
                MoveToPosition(currentPosition, .5f);
            }
            transform.GetChild(2).transform.GetChild(0).GetComponent<TMP_Text>().text = "" + bulletData.upperDamage;
            transform.GetChild(2).transform.GetChild(1).GetComponent<TMP_Text>().text = "" + bulletData.lowerDamage;

            if (bulletData.currentTile.Content != bulletData)
            {
                Debug.Log("currentTile is Null");
                Destroy(this.gameObject);
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
        

    private void checkUpperAndLowerType()
    {
        if (bulletData.upperType == BulletType.AIR)
        {
            transform.GetChild(0).GetComponent<Renderer>().material.color = Color.yellow;
        }
        if (bulletData.lowerType == BulletType.AIR)
        {
            transform.GetChild(1).GetComponent<Renderer>().material.color = Color.yellow;
        }
        if (bulletData.upperType == BulletType.FIRE)
        {
            transform.GetChild(0).GetComponent<Renderer>().material.color = Color.red;
        }
        if (bulletData.lowerType == BulletType.FIRE)
        {
            transform.GetChild(1).GetComponent<Renderer>().material.color = Color.red;
        }
        if (bulletData.upperType == BulletType.STONE)
        {
            transform.GetChild(0).GetComponent<Renderer>().material.color = Color.black;
        }
        if (bulletData.lowerType == BulletType.STONE)
        {
            transform.GetChild(1).GetComponent<Renderer>().material.color = Color.black;
        }
        if (bulletData.upperType == BulletType.WATER)
        {
            transform.GetChild(0).GetComponent<Renderer>().material.color = Color.blue;
        }
        if (bulletData.lowerType == BulletType.WATER)
        {
            transform.GetChild(1).GetComponent<Renderer>().material.color = Color.blue;
        }
    }

    public void MoveToPosition(Vector3 position, float timeToMove)
    {
        StartCoroutine(MoveTo(position, timeToMove));
    }
    public IEnumerator MoveTo(Vector3 position, float timeToMove)
    {
        var currentPos = transform.position;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp(currentPos, position, t);
            yield return null;
        }
    }
}
