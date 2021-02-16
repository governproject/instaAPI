using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InstaSharper;
using InstaSharper.API;
using InstaSharper.API.Builder;
using InstaSharper.Classes;
using InstaSharper.Classes.Models;
using InstaSharper.Logger;


namespace bot_deneme
{
    class Program
    {

        #region Hidden
        private const string username = "hasanqunes01";
        private const string password = "PoemMargin1411";
        #endregion

        private static UserSessionData user ;
        private static IInstaApi api;
        static void Main(string[] args)
        {
            user = new UserSessionData();
            user.UserName = username;
            user.Password = password;

            Login();
            Console.ReadKey();
        }
        private static async void Login(){

            api = InstaApiBuilder.CreateBuilder()
                .SetUser(user)
                .UseLogger(new DebugLogger(LogLevel.Exceptions))
                .SetRequestDelay(RequestDelay.FromSeconds(8,8))
                .Build();
            
            var loginRequest = await api.LoginAsync();
            if(loginRequest.Succeeded){

                Console.WriteLine("Logged is successfuly!");
                GetPosts("hasanqunes01");

            }
            else
                System.Console.WriteLine("Login Error!"+ loginRequest.Info.Message);
            
        }
        public static async void GetPosts(string userToScrape)
        {

            IResult<InstaUser> userSearch = await api.GetUserAsync(userToScrape);

            Console.WriteLine(string.Format("User: {0} \n\t Followers: {1} \n\t \n\t Verifeid: {2}",userSearch.Value.FullName, userSearch.Value.FollowersCount, userSearch.Value.IsVerified));
            IResult<InstaMediaList> media = await api.GetUserMediaAsync(userToScrape, PaginationParameters.MaxPagesToLoad(5));
            List<InstaMedia> mediaList = media.Value.ToList();

            for (int i = 0; i < mediaList.Count; i++)
            {
                InstaMedia m = mediaList[i];
                if(m != null && m.Caption != null)
                {
                    string captionText = m.Caption.Text;
                    if (captionText != null)
                    {
                        if (m.MediaType == InstaMediaType.Image)
                        {
                            if(m.Images[i]!= null && m.Images[i].URI != null)
                            {
                                string uri = m.Images[i].URI;
                                string info = mediaList[i].InstaIdentifier;
                                int hashDecode = mediaList[i].GetHashCode();
                                System.Console.WriteLine(Convert.ToString(i+1)+".nci Gönderi Resmi ->"+info+" "+ uri+"\n"+"\n\t hashcode: "+hashDecode);
                                System.Console.WriteLine(Convert.ToString(i+1)+".nci Gönderi Mesajı ->"+info+ " \t"+captionText+"\n");
                                
                            }
                           /* for (int x = 0; x < m.Images.Count; x++)
                            {
                                if (m.Images[x] != null && m.Images[x].URI != null)
                                { selam
                                    System.Console.WriteLine(Convert.ToString(x)+" "+ "\n\t"+captionText+"\n\t");
                                    string uri = m.Images[x].URI;
                                    System.Console.WriteLine(Convert.ToString(i)+" "+ uri+"\n\t");
                                }
                            }*/
                        }
                    }
                }
            }

        }
    }
}
