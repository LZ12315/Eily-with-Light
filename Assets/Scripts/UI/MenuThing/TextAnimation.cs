
using UnityEngine;
using TMPro;

public class TextAnimation : MonoBehaviour
{
    public TMP_Text tmp;
    public float YChange = 5.0f;
     void Update()
    {
        //ǿ�Ƹ�����������
        tmp.ForceMeshUpdate();
        //��ȡ�ı����ַ���Ϣ��������Ϣ
        var textInfo=tmp.textInfo;
        
        //����ÿ���ַ�
        for(int i = 0; i < textInfo.characterCount; i++)
        {
            //����
            var charInfo = textInfo.characterInfo[i];
           //�������ɼ����ַ�
            if(!charInfo.isVisible ) continue;

            var verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
            
            for(int j = 0; j < 4; j++) {
                var orig = verts[charInfo.vertexIndex + j];
                //����
                verts[charInfo.vertexIndex + j] = orig+new Vector3(0,Mathf.Sin(Time.time*2f+orig.x*0.45f)*YChange,0);

            }
        }
        //��������
        for(int i = 0;i < textInfo.meshInfo.Length; i++)
        {
            var meshInfo = textInfo.meshInfo[i];
            //�������񶥵�
            meshInfo.mesh.vertices = meshInfo.vertices;
            //�����ڲ���������
            tmp.UpdateGeometry(meshInfo.mesh,i);
        }
    }
}
