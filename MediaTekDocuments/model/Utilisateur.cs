using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    public class Utilisateur
    {
        public string id { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public string idService { get; set; }

        public Utilisateur(string id, string login, string password, string idService)
        {
            this.id = id;
            this.login = login;
            this.password = password;
            this.idService = idService;
        }

    }
}