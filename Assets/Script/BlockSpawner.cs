using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockSpawner : MonoBehaviour
{
    public GameObject[,] arrayOfGrids = new GameObject[larguraDaGrid, alturaDaGrid]; 
    public GameObject[,] previewBlock = new GameObject[2, 3];
    public Text scoreText;
    static int alturaDaGrid = 10;
    static int larguraDaGrid = 5;
    int coresDiposniveis = 5;
    int Score = 0;
    int tipoDeBloco = 0;
    float timePassed = 0;
    Vector2Int[] movingBlocks = new Vector2Int[(larguraDaGrid*alturaDaGrid)];
    Vector2Int[] occupiedBlocks = new Vector2Int[(larguraDaGrid * alturaDaGrid)];
    int totalMoving = 0;
    int linhaCheia = 0;
    int dificuldadeBloco = 2;
    int corDoBloco = 0;
    float tempoAtualizar = 0.8f;
    bool podeMover = true;
    bool podeMoverRight = true;
    bool podeMoverLeft = true;
    bool itsOver = false;


    // Start is called before the first frame update
    void Start()
    {
        int k = 0;
        for(int i = 0; i < alturaDaGrid; i++)
        {
            for(int j = 0; j< larguraDaGrid; j++)
            {
                string name = "Grid (" + k.ToString() + ")";
                arrayOfGrids[j, i] = GameObject.Find(name);
                k++;
            }
        }
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                string name = "Grid (" + k.ToString() + ")";
                previewBlock[j, i] = GameObject.Find(name);
                k++;
            }
        }
        tipoDeBloco = Random.Range(0, dificuldadeBloco);
        corDoBloco = Random.Range(0, coresDiposniveis);
        Spawn(tipoDeBloco, corDoBloco);
        PreviewBloco(tipoDeBloco, corDoBloco);
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + Score.ToString();
        timePassed += Time.deltaTime;
        if (timePassed > tempoAtualizar)
        {
            atualizaBlock();

            timePassed = 0;
        }
    }

    void atualizaBlock()
    {
        totalMoving = 0;
        podeMover = true;
        for (int i = 0; i < larguraDaGrid; i++)
        {
            for (int j = 0; j < alturaDaGrid; j++)
            {
                if (arrayOfGrids[i, j].GetComponent<GridBehaviour>().moving == true)
                {
                    movingBlocks[totalMoving] = new Vector2Int(i, j);
                    totalMoving += 1;
                }
            }
        }

        for(int i = 0; i < totalMoving; i++)
        {
            if (movingBlocks[i].y + 1 == alturaDaGrid)
            {
                podeMover = false;
                break;
            }
            else if (arrayOfGrids[movingBlocks[i].x , movingBlocks[i].y + 1].GetComponent<GridBehaviour>().occupied == true &&
                arrayOfGrids[movingBlocks[i].x , movingBlocks[i].y + 1].GetComponent<GridBehaviour>().moving == false )
            {
                podeMover = false;
                break;
            }
        }

         if (podeMover)
            moveBlock();
        else  cancelaMove();
    }

    void moveBlock()
    {
        for (int i = totalMoving - 1; i >= 0; i--)
        {
            arrayOfGrids[movingBlocks[i].x, movingBlocks[i].y].GetComponent<GridBehaviour>().moving = false;
            arrayOfGrids[movingBlocks[i].x, movingBlocks[i].y].GetComponent<GridBehaviour>().occupied = false;
            arrayOfGrids[movingBlocks[i].x, movingBlocks[i].y+1].GetComponent<GridBehaviour>().moving = true;
            arrayOfGrids[movingBlocks[i].x, movingBlocks[i].y+1].GetComponent<GridBehaviour>().occupied = true;
            arrayOfGrids[movingBlocks[i].x, movingBlocks[i].y + 1].GetComponent<GridBehaviour>().corDoBloco = arrayOfGrids[movingBlocks[i].x, movingBlocks[i].y].GetComponent<GridBehaviour>().corDoBloco;
        }
    }

    void moveBlockRight()
    {
        for (int i = totalMoving - 1; i >= 0; i--)
        {
            arrayOfGrids[movingBlocks[i].x, movingBlocks[i].y].GetComponent<GridBehaviour>().moving = false;
            arrayOfGrids[movingBlocks[i].x, movingBlocks[i].y].GetComponent<GridBehaviour>().occupied = false;
            arrayOfGrids[movingBlocks[i].x + 1, movingBlocks[i].y].GetComponent<GridBehaviour>().moving = true;
            arrayOfGrids[movingBlocks[i].x + 1, movingBlocks[i].y].GetComponent<GridBehaviour>().occupied = true;
            arrayOfGrids[movingBlocks[i].x + 1, movingBlocks[i].y].GetComponent<GridBehaviour>().corDoBloco = arrayOfGrids[movingBlocks[i].x, movingBlocks[i].y].GetComponent<GridBehaviour>().corDoBloco;
        }
    }
    void moveBlockLeft()
    {
        for (int i = 0; i < totalMoving; i++)
        {
            arrayOfGrids[movingBlocks[i].x, movingBlocks[i].y].GetComponent<GridBehaviour>().moving = false;
            arrayOfGrids[movingBlocks[i].x, movingBlocks[i].y].GetComponent<GridBehaviour>().occupied = false;
            arrayOfGrids[movingBlocks[i].x - 1, movingBlocks[i].y].GetComponent<GridBehaviour>().moving = true;
            arrayOfGrids[movingBlocks[i].x - 1, movingBlocks[i].y].GetComponent<GridBehaviour>().occupied = true;
            arrayOfGrids[movingBlocks[i].x - 1, movingBlocks[i].y].GetComponent<GridBehaviour>().corDoBloco = arrayOfGrids[movingBlocks[i].x, movingBlocks[i].y].GetComponent<GridBehaviour>().corDoBloco;
        }
    }

    void cancelaMove()
    {
        for (int j = 0; j < totalMoving; j++)
        {
            arrayOfGrids[movingBlocks[j].x, movingBlocks[j].y].GetComponent<GridBehaviour>().moving = false;
        }
        for (int j = alturaDaGrid - 1; j >= 0; j--)
        {
            linhaCheia = 0;
            for (int i = 0; i < larguraDaGrid; i++)
            {
                if (arrayOfGrids[i, j].GetComponent<GridBehaviour>().occupied == true)
                {
                    linhaCheia += 1;
                }
            }
            if (linhaCheia == larguraDaGrid)
            {
                for (int i = 0; i < larguraDaGrid; i++)
                {
                    arrayOfGrids[i, j].GetComponent<GridBehaviour>().occupied = false;
                }
                for(int linha = j; linha > 0; linha--)
                {
                    for(int i = 0; i < larguraDaGrid; i++)
                        arrayOfGrids[i, linha].GetComponent<GridBehaviour>().occupied = arrayOfGrids[i, linha - 1].GetComponent<GridBehaviour>().occupied;
                }
                Score += larguraDaGrid*10;
                j++;
            }
        }
        for (int i = 0; i < larguraDaGrid; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                if (arrayOfGrids[i, j].GetComponent<GridBehaviour>().occupied == true &&
                   arrayOfGrids[i, j].GetComponent<GridBehaviour>().moving == false)
                {
                    itsOver = true;
                }
            }
        }
        if(Score == larguraDaGrid*10)
        {
            tempoAtualizar = 0.7f;
        }
        if(Score == larguraDaGrid*20)
        {
            dificuldadeBloco = 3;
            tempoAtualizar = 0.6f;
        }
        if (Score == larguraDaGrid*40)
        {
            dificuldadeBloco = 4;
            tempoAtualizar = 0.5f;
        }
        if (Score == larguraDaGrid*50)
        {
            tempoAtualizar = 0.4f;
        }
        if (itsOver)
            GameOver();
        else Spawn(tipoDeBloco, corDoBloco);
    }
    void Spawn(int blockType, int blockColor)
    {
        int lugarDeSpawn = 0;
        if (blockType == 0)
        {
            lugarDeSpawn = Random.Range(0, larguraDaGrid);
            arrayOfGrids[lugarDeSpawn, 0].GetComponent<GridBehaviour>().occupied = true;
            arrayOfGrids[lugarDeSpawn, 1].GetComponent<GridBehaviour>().occupied = true;
            arrayOfGrids[lugarDeSpawn, 2].GetComponent<GridBehaviour>().occupied = true;
            arrayOfGrids[lugarDeSpawn, 0].GetComponent<GridBehaviour>().moving = true;
            arrayOfGrids[lugarDeSpawn, 1].GetComponent<GridBehaviour>().moving = true;
            arrayOfGrids[lugarDeSpawn, 2].GetComponent<GridBehaviour>().moving = true;
            arrayOfGrids[lugarDeSpawn, 0].GetComponent<GridBehaviour>().corDoBloco = blockColor;
            arrayOfGrids[lugarDeSpawn, 1].GetComponent<GridBehaviour>().corDoBloco = blockColor;
            arrayOfGrids[lugarDeSpawn, 2].GetComponent<GridBehaviour>().corDoBloco = blockColor;
        }
        else if (blockType == 1)
        {
            lugarDeSpawn = Random.Range(0, larguraDaGrid - 1);
            arrayOfGrids[lugarDeSpawn, 1].GetComponent<GridBehaviour>().occupied = true;
            arrayOfGrids[lugarDeSpawn, 2].GetComponent<GridBehaviour>().occupied = true;
            arrayOfGrids[lugarDeSpawn+1, 1].GetComponent<GridBehaviour>().occupied = true;
            arrayOfGrids[lugarDeSpawn+1, 2].GetComponent<GridBehaviour>().occupied = true;
            arrayOfGrids[lugarDeSpawn, 1].GetComponent<GridBehaviour>().moving = true;
            arrayOfGrids[lugarDeSpawn, 2].GetComponent<GridBehaviour>().moving = true;
            arrayOfGrids[lugarDeSpawn+1, 1].GetComponent<GridBehaviour>().moving = true;
            arrayOfGrids[lugarDeSpawn+1, 2].GetComponent<GridBehaviour>().moving = true;
            arrayOfGrids[lugarDeSpawn, 1].GetComponent<GridBehaviour>().corDoBloco = blockColor;
            arrayOfGrids[lugarDeSpawn, 2].GetComponent<GridBehaviour>().corDoBloco = blockColor;
            arrayOfGrids[lugarDeSpawn+1, 1].GetComponent<GridBehaviour>().corDoBloco = blockColor;
            arrayOfGrids[lugarDeSpawn + 1, 2].GetComponent<GridBehaviour>().corDoBloco = blockColor;
        }
        else if (blockType == 2)
        {
            lugarDeSpawn = Random.Range(0, larguraDaGrid - 1);
            arrayOfGrids[lugarDeSpawn, 0].GetComponent<GridBehaviour>().occupied = true;
            arrayOfGrids[lugarDeSpawn, 1].GetComponent<GridBehaviour>().occupied = true;
            arrayOfGrids[lugarDeSpawn, 2].GetComponent<GridBehaviour>().occupied = true;
            arrayOfGrids[lugarDeSpawn + 1, 2].GetComponent<GridBehaviour>().occupied = true;
            arrayOfGrids[lugarDeSpawn, 0].GetComponent<GridBehaviour>().moving = true;
            arrayOfGrids[lugarDeSpawn, 1].GetComponent<GridBehaviour>().moving = true;
            arrayOfGrids[lugarDeSpawn, 2].GetComponent<GridBehaviour>().moving = true;
            arrayOfGrids[lugarDeSpawn + 1, 2].GetComponent<GridBehaviour>().moving = true;
            arrayOfGrids[lugarDeSpawn, 0].GetComponent<GridBehaviour>().corDoBloco = blockColor;
            arrayOfGrids[lugarDeSpawn, 1].GetComponent<GridBehaviour>().corDoBloco = blockColor;
            arrayOfGrids[lugarDeSpawn, 2].GetComponent<GridBehaviour>().corDoBloco = blockColor;
            arrayOfGrids[lugarDeSpawn + 1, 2].GetComponent<GridBehaviour>().corDoBloco = blockColor;
        }
        else if (blockType == 3)
        {
            lugarDeSpawn = Random.Range(1, larguraDaGrid);
            arrayOfGrids[lugarDeSpawn - 1, 2].GetComponent<GridBehaviour>().occupied = true;
            arrayOfGrids[lugarDeSpawn, 0].GetComponent<GridBehaviour>().occupied = true;
            arrayOfGrids[lugarDeSpawn, 1].GetComponent<GridBehaviour>().occupied = true;
            arrayOfGrids[lugarDeSpawn, 2].GetComponent<GridBehaviour>().occupied = true;
            arrayOfGrids[lugarDeSpawn - 1, 2].GetComponent<GridBehaviour>().moving = true;
            arrayOfGrids[lugarDeSpawn, 0].GetComponent<GridBehaviour>().moving = true;
            arrayOfGrids[lugarDeSpawn, 1].GetComponent<GridBehaviour>().moving = true;
            arrayOfGrids[lugarDeSpawn, 2].GetComponent<GridBehaviour>().moving = true;
            arrayOfGrids[lugarDeSpawn, 0].GetComponent<GridBehaviour>().corDoBloco = blockColor;
            arrayOfGrids[lugarDeSpawn, 1].GetComponent<GridBehaviour>().corDoBloco = blockColor;
            arrayOfGrids[lugarDeSpawn, 2].GetComponent<GridBehaviour>().corDoBloco = blockColor;
            arrayOfGrids[lugarDeSpawn - 1, 2].GetComponent<GridBehaviour>().corDoBloco = blockColor;
        }
        tipoDeBloco = Random.Range(0, dificuldadeBloco);
        corDoBloco = Random.Range(0, coresDiposniveis);
        PreviewBloco(tipoDeBloco, corDoBloco);
    }

    void PreviewBloco(int blockType, int blockColor)
    {
        if (blockType == 0)
        {
            previewBlock[0, 0].GetComponent<GridBehaviour>().occupied = true;
            previewBlock[0, 0].GetComponent<GridBehaviour>().corDoBloco = blockColor;
            previewBlock[0, 1].GetComponent<GridBehaviour>().occupied = true;
            previewBlock[0, 1].GetComponent<GridBehaviour>().corDoBloco = blockColor;
            previewBlock[0, 2].GetComponent<GridBehaviour>().occupied = true;
            previewBlock[0, 2].GetComponent<GridBehaviour>().corDoBloco = blockColor;
            previewBlock[1, 0].GetComponent<GridBehaviour>().occupied = false;
            previewBlock[1, 1].GetComponent<GridBehaviour>().occupied = false;
            previewBlock[1, 2].GetComponent<GridBehaviour>().occupied = false;
        }
        else if (blockType == 1)
        {
            previewBlock[0, 0].GetComponent<GridBehaviour>().occupied = false;
            previewBlock[0, 1].GetComponent<GridBehaviour>().occupied = true;
            previewBlock[0, 1].GetComponent<GridBehaviour>().corDoBloco = blockColor;
            previewBlock[0, 2].GetComponent<GridBehaviour>().occupied = true;
            previewBlock[0, 2].GetComponent<GridBehaviour>().corDoBloco = blockColor;
            previewBlock[1, 0].GetComponent<GridBehaviour>().occupied = false;
            previewBlock[1, 1].GetComponent<GridBehaviour>().occupied = true;
            previewBlock[1, 1].GetComponent<GridBehaviour>().corDoBloco = blockColor;
            previewBlock[1, 2].GetComponent<GridBehaviour>().occupied = true;
            previewBlock[1, 2].GetComponent<GridBehaviour>().corDoBloco = blockColor;
        }
        else if (blockType == 2)
        {
            previewBlock[0, 0].GetComponent<GridBehaviour>().occupied = true;
            previewBlock[0, 0].GetComponent<GridBehaviour>().corDoBloco = blockColor;
            previewBlock[0, 1].GetComponent<GridBehaviour>().occupied = true;
            previewBlock[0, 1].GetComponent<GridBehaviour>().corDoBloco = blockColor;
            previewBlock[0, 2].GetComponent<GridBehaviour>().occupied = true;
            previewBlock[0, 2].GetComponent<GridBehaviour>().corDoBloco = blockColor;
            previewBlock[1, 0].GetComponent<GridBehaviour>().occupied = false;
            previewBlock[1, 1].GetComponent<GridBehaviour>().occupied = false;
            previewBlock[1, 2].GetComponent<GridBehaviour>().occupied = true;
            previewBlock[1, 2].GetComponent<GridBehaviour>().corDoBloco = blockColor;
        }
        else if (blockType == 3)
        {
            previewBlock[0, 0].GetComponent<GridBehaviour>().occupied = false;
            previewBlock[0, 1].GetComponent<GridBehaviour>().occupied = false;
            previewBlock[0, 2].GetComponent<GridBehaviour>().occupied = true;
            previewBlock[0, 2].GetComponent<GridBehaviour>().corDoBloco = blockColor;
            previewBlock[1, 0].GetComponent<GridBehaviour>().occupied = true;
            previewBlock[1, 0].GetComponent<GridBehaviour>().corDoBloco = blockColor;
            previewBlock[1, 1].GetComponent<GridBehaviour>().occupied = true;
            previewBlock[1, 1].GetComponent<GridBehaviour>().corDoBloco = blockColor;
            previewBlock[1, 2].GetComponent<GridBehaviour>().occupied = true;
            previewBlock[1, 2].GetComponent<GridBehaviour>().corDoBloco = blockColor;
        }
    }

    public void RightClick()
    {
        podeMoverRight = true;
        totalMoving = 0;
        for (int i = 0; i < larguraDaGrid; i++)
        {
            for (int j = 0; j < alturaDaGrid; j++)
            {
                if (arrayOfGrids[i, j].GetComponent<GridBehaviour>().moving == true)
                {
                    movingBlocks[totalMoving] = new Vector2Int(i, j);
                    totalMoving += 1;
                }
            }
        }
        for (int i = 0; i < totalMoving; i++)
        {
                if (movingBlocks[i].x + 1 == larguraDaGrid)
                {
                    podeMoverRight = false;
                    break;
                }
                else if (arrayOfGrids[movingBlocks[i].x + 1, movingBlocks[i].y].GetComponent<GridBehaviour>().occupied == true &&
                    arrayOfGrids[movingBlocks[i].x + 1, movingBlocks[i].y].GetComponent<GridBehaviour>().moving == false)
                {
                    podeMoverRight = false;
                    break;
                }
        }
        if (podeMoverRight)
            moveBlockRight();
    }

    public void LeftClick()
    {
        podeMoverLeft = true;
        totalMoving = 0;
        for (int i = 0; i < larguraDaGrid; i++)
        {
            for (int j = 0; j < alturaDaGrid; j++)
            {
                if (arrayOfGrids[i, j].GetComponent<GridBehaviour>().moving == true)
                {
                    movingBlocks[totalMoving] = new Vector2Int(i, j);
                    totalMoving += 1;
                }
            }
        }
        for (int i = 0; i < totalMoving; i++)
        {
                if (movingBlocks[i].x == 0)
                {
                    podeMoverLeft= false;
                    break;  
                }
                else if (arrayOfGrids[movingBlocks[i].x - 1, movingBlocks[i].y].GetComponent<GridBehaviour>().occupied == true &&
                    arrayOfGrids[movingBlocks[i].x - 1, movingBlocks[i].y].GetComponent<GridBehaviour>().moving == false)
                {
                    podeMoverLeft = false;
                    break;
                }
        }
        if (podeMoverLeft)
            moveBlockLeft();
    }

    void GameOver()
    {
        Debug.Log("GameOver");
    }
}