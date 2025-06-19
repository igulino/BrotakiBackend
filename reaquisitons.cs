namespace Reaq
{
    using DBconnection;
    using Google.Cloud.Firestore;
    using Google.Type;

    public class Users
    {
        public static async Task<string> CadUser(skeleton.user user)
        {
            if (DBconnection.DB.db == null)
            {
                return "error";
            }

            DocumentReference userCreated = DBconnection.DB.db.Collection("usuário").Document();

            await userCreated.SetAsync(user);
            return "200";
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
        public static skeleton.Posts mes;
        public Posts(string desc, string Image, int PostID, skeleton.user User, int likes, string date)
        {
            mes = new skeleton.Posts
            {
                desc = desc,
                Image = Image,
                PostID = PostID,
                User = User,
                likes = likes,
                date = date
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

                DocumentReference doc = DBconnection.DB.db.Collection("message").Document();

                await doc.SetAsync(mes);

                return "message sent";

            }
            catch (System.Exception)
            {
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

                var doc = DBconnection.DB.db.Collection("message").WhereEqualTo("User.ID", mes.User.ID);

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
        public async Task<List<Dictionary<string, object>>> ConsultAll()
        {
            try
            {
                if (DBconnection.DB.db == null)
                {
                    throw new Exception("Firestore não foi inicializado corretamente.");
                }

                var doc = DBconnection.DB.db.Collection("message");
                var snapshot = await doc.GetSnapshotAsync();

                var lista = new List<Dictionary<string, object>>();

                foreach (var item in snapshot.Documents)
                {
                    if (item.Exists)
                    {
                        var data = item.ToDictionary();
                        data["id"] = item.Id;
                        lista.Add(data);
                    }
                }

                System.Console.WriteLine(snapshot);
                return lista;

            }
            catch (System.Exception)
            {
                throw;
            }

        }
        
        
    }
    
}