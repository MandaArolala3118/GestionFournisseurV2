namespace Evaluation_Fournisseur.Models
{
    public class LDAPSettings
    {
        public string ServerAddress { get; set; }
        public int Port { get; set; }
        public bool UseSSL { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string BaseDN { get; set; }
    }
}
