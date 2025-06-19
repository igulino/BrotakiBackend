

namespace DBconnection
{
    using Google.Cloud.Firestore;
    using DotNetEnv;
    public class DB
    {
        public static FirestoreDb db;
        public DB()
        {
            Env.Load(); 

            string? path = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");

            if (string.IsNullOrEmpty(path))
            {
                throw new Exception("A variável de ambiente GOOGLE_APPLICATION_CREDENTIALS não foi definida.");
            }

            db = FirestoreDb.Create("brotaki-356ab");
            Console.WriteLine("Conectado ao Firestore com sucesso! " + db);

        }

        public static string Test()
        {

            System.Console.WriteLine("this is connection " + db);
            return "teste";
        }

    }


}