using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
//using NLog;
using System.Data.Common;
using UserInfoApi.UserList;

namespace UserInfoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserListController : ControllerBase
    {
        //private static Logger logger = LogManager.GetCurrentClassLogger();

        [HttpGet]
        public async Task GetUserList()
        {
            
            try
            {
               
                HttpClient httpClient = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://jsonplaceholder.typicode.com/todos");
            request.Method = HttpMethod.Get;
            
            HttpResponseMessage response = await httpClient.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();
            List<MyList> data = JsonConvert.DeserializeObject<List<MyList>>(responseString);
                //logger.Info("--------------------data---------------------");
                //logger.Info(data);
                
                using (var context = new UserListContext())
                {
                    data.ForEach(x =>
                    {
                        context.Add(new MyList{
                          UserId = x.UserId,
                          Title = x.Title,
                          Completed = x.Completed
                        });

                    });
                    context.SaveChanges();
                }               
            }
            catch(DbException dbEx)
            {
                //logger.Error(dbEx.Message);
            }
            catch(Exception ex)
            {
                //logger.Error(ex.ToString());
            }
        }
    }
}
