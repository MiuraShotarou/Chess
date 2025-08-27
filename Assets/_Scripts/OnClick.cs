using System;
using Unity.VisualScripting;
using UnityEngine;

public class OnClick : MonoBehaviour
{
    OpenSelectableArea _openSelectableArea;
    void Start()
    {
        _openSelectableArea = GetComponent<OpenSelectableArea>();
    }
    public void OnPiecePressed(GameObject selectedPieceObj) //引数 SpriteRenderer に変更しても良いかも
    {
        GetComponent<OpenSelectableArea>().StartOpenArea(selectedPieceObj);//関数の呼び出し + キャッシュ化しておくこと
        // this.GetComponent<SelectTileController>().enabled = true;//このタイミングにする必要はない
    }
}
