using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class upload : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
	    HttpFileCollection files = Request.Files;
        string msg = string.Empty;
        string error = string.Empty;
        string imgurl;
        if (files.Count > 0)
        {
            string fileName = Server.MapPath("/UploadFile/image/") + System.IO.Path.GetFileName(files[0].FileName);
            files[0].SaveAs(fileName);
            msg = " 成功! 文件大小为:" + files[0].ContentLength;
            imgurl = "/" + files[0].FileName;
            string res = "{ error:'" + error + "', msg:'" + msg + "',imgurl:'" + imgurl + "'}";
            Response.Write(res);
            Response.End();
        }
    }
}