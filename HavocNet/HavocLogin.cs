using System.Web;
using Core;
using Core.Repositories;
using Phoenix.Core.Servers;

namespace HavocNet
{
    public class HavocLogin
    {
        public User FetchUser(HavocServer aServer)
        {
            User aUser;
            var aUserRepository = new UserRepository(aServer);

            if (HttpContext.Current.Session["HavocNetUser"] != null)
            {
                aUser = (User)HttpContext.Current.Session["HavocNetUser"];
            }
            else
            {
                if (HttpContext.Current.Request.QueryString["UserID"] != null)
                {
                    //Readonly User
                    aUser = aUserRepository.GetPublic();
                }
                else
                {
                    if (HttpContext.Current.Request.Cookies.Get("Username") != null)
                    {
                        if (aUserRepository.TryGet(HttpContext.Current.Request.Cookies.Get("Username").Value, HttpContext.Current.Request.Cookies.Get("Password").Value, out aUser))
                        {
                            HttpContext.Current.Session.Add("HavocNetUser", aUser);
                        }
                        else
                        {
                            aUser = aUserRepository.GetPublic();
                        }
                    }
                    else
                    {
                        aUser = aUserRepository.GetPublic();
                    }
                }
            }

            aUserRepository.TryGet("havoc", "Laser197302", out aUser);

            return aUser;
        }
    }
}
