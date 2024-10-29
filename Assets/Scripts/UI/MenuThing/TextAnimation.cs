
using UnityEngine;
using TMPro;

public class TextAnimation : MonoBehaviour
{
    public TMP_Text tmp;
    public float YChange = 5.0f;
     void Update()
    {
        //强制更新网格数据
        tmp.ForceMeshUpdate();
        //获取文本的字符信息和网格信息
        var textInfo=tmp.textInfo;
        
        //遍历每个字符
        for(int i = 0; i < textInfo.characterCount; i++)
        {
            //声明
            var charInfo = textInfo.characterInfo[i];
           //跳过不可见的字符
            if(!charInfo.isVisible ) continue;

            var verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
            
            for(int j = 0; j < 4; j++) {
                var orig = verts[charInfo.vertexIndex + j];
                //动画
                verts[charInfo.vertexIndex + j] = orig+new Vector3(0,Mathf.Sin(Time.time*2f+orig.x*0.45f)*YChange,0);

            }
        }
        //更新网格
        for(int i = 0;i < textInfo.meshInfo.Length; i++)
        {
            var meshInfo = textInfo.meshInfo[i];
            //更新网格顶点
            meshInfo.mesh.vertices = meshInfo.vertices;
            //更新内部几何数据
            tmp.UpdateGeometry(meshInfo.mesh,i);
        }
    }
}
