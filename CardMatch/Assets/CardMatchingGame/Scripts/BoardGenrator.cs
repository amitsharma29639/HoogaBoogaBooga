using UnityEngine;
using UnityEngine.UI;

public class BoardGenrator : MonoBehaviour
{
    [SerializeField]
    private GameObject tile;
    
    private float width, height;
    private float hGap, vGap;
    private float xOffset, yOffset;
    private GridLayoutGroup gridLayout;

    private void Awake()
    {
       int rows = 8;
       int cols = 8;
       RectTransform rectTransform = tile.GetComponent<RectTransform>();
       gridLayout = GetComponent<GridLayoutGroup>();
        width = rectTransform.rect.width;
        height = rectTransform.rect.height;
        hGap = 0;
        vGap = 0;
        float boardSizeX = width * cols + hGap * (cols - 1);
        float boardSizeY = height * rows + hGap * (rows - 1);
        xOffset = -(boardSizeX / 2) + width/2;
        yOffset = -(boardSizeY / 2) + height/2;
       // GenrateBoard(rows , cols);
         CreateAutoLayoutBoard();
    }

    private void GenrateBoard(int rows , int cols)
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                GameObject newTile = Instantiate(tile, transform , false);
                RectTransform rectTransform = (RectTransform)newTile.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(xOffset + (hGap + width) * j, yOffset + (vGap + height) * i);
                rectTransform.sizeDelta = new Vector2(width, height);
                Image image = newTile.GetComponent<Image>();
                Debug.Log(image);
                if ((i + j)%2 == 0)
                {
                    Debug.Log("SetColor white");
                    image.color = Color.white;
                }
                else
                {
                    Debug.Log("SetColor black");
                    image.color = Color.black;
                }

                
            }
        }
    }

    private void CreateAutoLayoutBoard()
    {
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = 8;
        gridLayout.cellSize = new Vector2(width, height);
        gridLayout.childAlignment = TextAnchor.MiddleCenter;

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                GameObject newTile = Instantiate(tile, transform , false);
                Image image = newTile.GetComponent<Image>();
                if ((i + j)%2 == 0)
                {
                    Debug.Log("SetColor white");
                    image.color = Color.white;
                }
                else
                {
                    Debug.Log("SetColor black");
                    image.color = Color.black;
                }
            }

            
        }
    }
}
