using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignTex : MonoBehaviour
{
    public ComputeShader shader;
    public int texResolution = 256;

    Renderer rend;
    RenderTexture outputTex;
    int kernelHandle;

    private void Start()
    {
        outputTex = new RenderTexture(texResolution, texResolution, 0);
        outputTex.enableRandomWrite = true; // gpu'n bunu üretir
        outputTex.Create();

        rend = GetComponent<Renderer>();
        rend.enabled = true;
        InitShader();
    }
    private void InitShader()
    {
        kernelHandle = shader.FindKernel("CSMain");
        shader.SetTexture(kernelHandle, "Result", outputTex);
        rend.material.SetTexture("_MainTex", outputTex);
        DispatchShader(texResolution / 16, texResolution / 16);
        /* 
         * Her texRes 8 thread'den oluşur.
         * TextRes/8= 1 thread'i verir. Bu bizim çözünürlüğümüzdür.
         * /16 yaparsak ekranın yarısına gelmiş oluruz x ve y ekseninde yani 
         * ekranın çeyreği tex ile kaplanmış olur
         */
    }

    private void DispatchShader(int x, int y)
    {
        shader.Dispatch(kernelHandle, x, y, 1);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DispatchShader(texResolution / 8, texResolution / 8);
        }
    }
}



















