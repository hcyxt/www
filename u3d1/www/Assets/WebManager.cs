using UnityEngine;
using System.Collections;

public class WebManager : MonoBehaviour {
    string m_info = "Nothing";
    public Texture2D m_uploadImage;  //上传图片
    protected Texture2D m_downloadTexture;//下载图片
    protected AudioClip m_downloadClip;

	// Use this for initialization
	void Start () {
        StartCoroutine(DownloadSound());
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        // 设置显示区域，Rect(左上角x,左上角y,宽，高）
        GUI.BeginGroup(new Rect(Screen.width * 0.5f - 100, Screen.height * 0.5f - 100, 500, 200), "");
        // 新建个Label显示m_info的内容,Group里的显示位置是相对位置
        GUI.Label(new Rect(10, 10, 400, 30), m_info);
        
        if (GUI.Button(new Rect(10, 50, 150, 30), "Get Data"))
        {
            //显示www实例发给服务器后返回的值www.text
            StartCoroutine(IGetData());
        }
        if (GUI.Button(new Rect(10, 100, 150, 30), "Post Data"))
        {
            StartCoroutine(IPostData());
        }
        //显示下载下来的图片
        if (m_downloadTexture != null)
            GUI.DrawTexture(new Rect(0, 0, m_downloadTexture.width, m_downloadTexture.height), m_downloadTexture);

        if (GUI.Button(new Rect(10, 150, 150, 30), "Request Image"))
        {
            StartCoroutine(IRequestPNG());
        }


        GUI.EndGroup();

    }
        //用WWW实例，GET方式发送请求，接收返回值
    IEnumerator IGetData()
    {
       //创建WWW实例，向指定IP发送GET请求，?后是附加数据
        WWW www = new WWW("http://7ROAD-20140625X.7road.com/test/test.php?username=get&password=12345");
            //yield return www 等待服务器响应
            yield return www;
            //只要返回的www.error不为空（即没报错），则m_info=www.text即返回值
            if(www.error != null)
            {
                m_info = www.error;
                yield return null ;
            }
            m_info=www.text ;
    }
    //用POST方式提交数据
    IEnumerator IPostData()
    {
        //HTTP报头 headers
        System.Collections.Hashtable headers = new System.Collections.Hashtable();
        headers.Add("Content-Type", "application/x-www-form-urlencoded");
        //post的内容，前面不用带'?'
        string data = "username=post&password=6789";
        //post方式需要将字符串转化为byte数组
        byte[] bs = System.Text.UTF8Encoding.UTF8.GetBytes(data);

        WWW www = new WWW("http://7ROAD-20140625X.7road.com/test/test.php", bs, headers);

        yield return www;

        if(www.error != null)
        {
            m_info = www.error;
            yield return null;
        }
        m_info = www.text;
        
    }
    //上传PNG图片，通过EncodeToPNG函数将PNG图片变为byte数组
    IEnumerator IRequestPNG()
    {
        byte[] bs = m_uploadImage.EncodeToPNG();

        WWWForm form = new WWWForm();
        form.AddBinaryData("picture", bs, "screenshot", "image/png");

        WWW www = new WWW("http://7ROAD-20140625X.7road.com/test/test.php", form);

        yield return www;

        if (www.error != null)
        {
            m_info = www.error;
            yield return null;
        }
        m_downloadTexture = www.texture;
    }

    IEnumerator DownloadSound()
    {
        WWW www = new WWW("http://7ROAD-20140625X.7road.com/music.wav");
        yield return www;

        if (www.error != null)
        {
            m_info = www.error;
            yield return null;
        }
        m_downloadClip = www.GetAudioClip(false);
        audio.PlayOneShot(m_downloadClip);
    }



}
