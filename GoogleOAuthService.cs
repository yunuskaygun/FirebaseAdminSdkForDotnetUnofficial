using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Piri.Vehicle.Services.Api.Service
{
    public class GoogleOAuthService
    {
        public GoogleOAuthService()
        {
        }

        public async Task<string> GetToken()
        {
            GoogleCredential credential;
            using (var stream = new System.IO.FileStream("serviceAccountKey.json", System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(
                    new string[] {
                        "https://www.googleapis.com/auth/firebase.database",
                        "https://www.googleapis.com/auth/firebase.messaging",
                        "https://www.googleapis.com/auth/identitytoolkit",
                        "https://www.googleapis.com/auth/userinfo.email"}
                    );
            }

            ITokenAccess c = credential as ITokenAccess;
            return await c.GetAccessTokenForRequestAsync();
        }

        public bool SetUserClaims(string accessToken, string userId, int roleId)
        {
            try
            {
                WebRequest tRequest = WebRequest.Create("https://www.googleapis.com/identitytoolkit/v3/relyingparty/setAccountInfo");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";

                JObject data = new JObject();
                data.Add("localId", userId);
                data.Add("customAttributes", "{'roleId':"+ roleId.ToString()+"}");

                Byte[] byteArray = Encoding.UTF8.GetBytes(data.ToString());
                tRequest.Headers.Add($"X-Client-Version","Node/Admin/5.5.0");
                tRequest.Headers.Add($"Authorization"," Bearer " + accessToken);
                tRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            if (dataStreamResponse != null)
                                using (StreamReader tReader = new StreamReader(dataStreamResponse))
                                {
                                    String sResponseFromServer = tReader.ReadToEnd();
                                    string str = sResponseFromServer;
                                }
                            else return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                string str = ex.Message;
                return false;
            }
        }


        public string CreateUser(string accessToken, string email, string password)
        {
            try
            {
                WebRequest tRequest = WebRequest.Create("https://www.googleapis.com/identitytoolkit/v3/relyingparty/signupNewUser");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";

                var data = new
                {
                    email = email,
                    emailVerified = false,
                    password = password,
                    displayName = "",
                    //phoneNumber = "",
                    //photoURL = "",
                    disabled = false
                };

                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(data);
                
                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add($"X-Client-Version", "Node/Admin/5.5.0");
                tRequest.Headers.Add($"Authorization", " Bearer " + accessToken);
                tRequest.ContentLength = byteArray.Length;

                string localId = "";
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            if (dataStreamResponse != null) { 
                                using (StreamReader tReader = new StreamReader(dataStreamResponse))
                                {
                                    String sResponseFromServer = tReader.ReadToEnd();
                                    string str = sResponseFromServer;
                                    JObject resJson = JObject.Parse(str);
                                    localId = resJson["localId"].ToString();
                                }
                                return localId;
                            }
                        }
                    }
                }
                return localId;
            }
            catch (Exception ex)
            {
                string str = ex.Message;
                return "";
            }
        }

    }
}
