using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] CubePrefabArr;
    GameObject[] CubeArr = new GameObject[8];
    const float cubeSize = 6f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            MapGenerate();
        }
    }
    void MapGenerate()
    {
        //Debug.Log("MapGenerate");
        for (int i = 0; i < 8; i++)
        {
            Destroy(CubeArr[i]);
        }
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                Vector3 _rotation = Vector3.zero;
                if (j == 1)
                {
                    _rotation.y = -90;
                    if (i == 0)
                    {
                        _rotation.y *= Random.Range(1, 3);
                    }
                    else if (i == 3)
                    {
                        _rotation.y *= Random.Range(0, 2);
                    }
                }
                else
                {
                    _rotation.y = 90;
                    if (i == 0)
                    {
                        _rotation.y *= Random.Range(1, 3);
                    }
                    else if (i == 3)
                    {
                        _rotation.y *= Random.Range(0, 2);
                    }
                }

                int _cubeRandom = Random.Range(0, CubePrefabArr.Length);
                if (_cubeRandom == 0)
                {
                    _rotation.y = Random.Range(0, 3) * 90;
                }

                GameObject cube = Instantiate(CubePrefabArr[_cubeRandom], new Vector3(i * cubeSize, 3, j * cubeSize), Quaternion.identity);
                cube.transform.rotation = Quaternion.Euler(_rotation);
                cube.transform.parent = this.transform;
                CubeArr[i * 2 + j] = cube;
            }
        }
    }
}