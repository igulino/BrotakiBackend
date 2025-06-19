namespace Reaq
{
    using DBconnection;
    using Google.Cloud.Firestore;
    using Google.Type;
    using skeleton;

    public class Users
    {
        public static async Task<string> CadUser(skeleton.user user)
        {
            if (DBconnection.DB.db == null)
            {
                return "error";
            }

            DocumentReference userCreated = DBconnection.DB.db.Collection("usuário").Document();
            Random rnd = new Random();
            int idPost = rnd.Next(100000, 999999);

            string senha = user.Password;
            string hash = BCrypt.Net.BCrypt.HashPassword(senha);
            user.Password = hash;
            user.ID = idPost;
            
            await userCreated.SetAsync(user);
            return "200";
        }

        public static async Task<skeleton.user> LoginUser(string pass, string name)
        {
            try
            {
                Query userLogin = DBconnection.DB.db.Collection("usuário").WhereEqualTo("name", name);
                QuerySnapshot snapshot = await userLogin.GetSnapshotAsync();
                if (snapshot.Documents.Count > 0)
                {
                    var user = snapshot.Documents[0].ConvertTo<user>();
                    bool senhaCorreta = BCrypt.Net.BCrypt.Verify(pass, user.Password);
                    if (senhaCorreta)
                    {
                        return user;
                    }
                    else
                    {
                        throw new Exception("Senha incorreta.");
                    }

                }
                throw new Exception("Usúario não encontrado.");
                
            }
            catch (System.Exception err)
            {
                throw new Exception("erro: ", err);
                throw;
            }
            
        }

    }
    public class Follows
    {
        public skeleton.Followers Task;
        public Follows(int followedID, int followerID)
        {
            Task = new skeleton.Followers
            {
                followedID = followedID,
                followerID = followerID
            };
        }
        public async Task Follow()
        {
            try
            {
                if (DBconnection.DB.db == null)
                {
                    throw new Exception("Firestore não foi inicializado corretamente.");
                }

                DocumentReference doc = DBconnection.DB.db.Collection("followers").Document();

                await doc.SetAsync(Task);
                System.Console.WriteLine("feito!");
            }
            catch (System.Exception)
            {

                throw;
            }
        }
        public async Task unfollow()
        {
            try
            {
                if (DBconnection.DB.db == null)
                {
                    throw new Exception("Firestore não foi inicializado corretamente.");
                }

                Query doc = DBconnection.DB.db.Collection("followers").WhereEqualTo("followedID", Task.followedID).WhereEqualTo("followerID", Task.followerID);
                QuerySnapshot snapshot = await doc.GetSnapshotAsync();
                if (snapshot.Documents.Count > 0)
                {
                    await snapshot.Documents[0].Reference.DeleteAsync();
                    System.Console.WriteLine("feito!");
                }
                else
                {
                    Console.WriteLine("Documento não encontrado.");
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }

    }


    public class Posts
    {
        public static skeleton.PostsUser mes;
        public Posts(skeleton.Posts posts, int ID)
        {
            mes = new skeleton.PostsUser
            {
                Posts = posts,
                ID = ID
            };
        }

        
        public async static Task<string> PostMessage()
        {
            try
            {
                if (DBconnection.DB.db == null)
                {
                    throw new Exception("Firestore não foi inicializado corretamente.");
                }
                
                Random rnd = new Random();
                int idPost = rnd.Next(100000, 999999);
                mes.Posts.PostID = idPost;

                DocumentReference doc = DBconnection.DB.db.Collection("message").Document();
                Query doc2 = DBconnection.DB.db.Collection("usuário").WhereEqualTo("ID", mes.ID);
                QuerySnapshot snapshot = await doc2.GetSnapshotAsync();
                
                if (snapshot.Documents.Count > 0)
                {
                    QuerySnapshot userSnapshot = await doc2.GetSnapshotAsync();
                    var user = userSnapshot.Documents[0].ConvertTo<user>();

                    System.Console.WriteLine("feito!");
                    FeedItem a = new FeedItem
                    {
                        Post = mes.Posts,
                        User = user
                    };
                    await doc.SetAsync(a);
                    return "message sent";
                }


                return "something went wrong...";
                

            }
            catch (System.Exception error)
            {
                System.Console.WriteLine(error);
                return "deu ruim";
                throw;
            }

        }
        public async Task<Dictionary<string, object?>> ConsultMessage()
        {
            try
            {
                if (DBconnection.DB.db == null)
                {
                    throw new Exception("Firestore não foi inicializado corretamente.");
                }

                var doc = DBconnection.DB.db.Collection("message").WhereEqualTo("ID", mes.ID);

                var snapshot = await doc.GetSnapshotAsync();

                if (snapshot != null)
                {
                    var data = snapshot[0].ToDictionary();

                    return data;

                }
                return null;

            }
            catch (System.Exception)
            {
                throw;
            }

        }


        public async Task<List<FeedItem>> GetFeedAsync()
        {

            var postSnapshots = await DBconnection.DB.db.Collection("message").GetSnapshotAsync();
            List<FeedItem> list = new List<FeedItem>();
            foreach (var item in postSnapshots)
            {
                var i = item.ConvertTo<FeedItem>();
                list.Add(i);
            }

            return list;
        }

        
        
    }
    
}