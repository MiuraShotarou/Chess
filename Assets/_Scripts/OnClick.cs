using Unity.VisualScripting;
using UnityEngine;

public class OnClick : MonoBehaviour
{
    public void OnPiecePressed(GameObject selectedPieceObj) //引数 SpriteRenderer に変更しても良いかも
    {
        SpriteRenderer spriteRenderer = selectedPieceObj.GetComponent<SpriteRenderer>(); //これも関数にしたい
        spriteRenderer.color = Color.white;
        spriteRenderer.material = Resources.Load<Material>("OutlineMaterial");
        GetComponent<OpenSelectableArea>().StartOpenArea(selectedPieceObj);//関数の呼び出し + キャッシュ化しておくこと
        this.GetComponent<SelectTileController>().enabled = true;
    }
}
