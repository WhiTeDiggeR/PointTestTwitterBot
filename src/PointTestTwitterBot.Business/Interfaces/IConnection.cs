using System.Collections.Generic;

namespace PointTestTwitterBot.Business.Interfaces
{
    public interface IConnection
    {
        List<string> GetUserLastPosts(string userName, int postsCount = 5);
        void Post(string text);
    }
}
