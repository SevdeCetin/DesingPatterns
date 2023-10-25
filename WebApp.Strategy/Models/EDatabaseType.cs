using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Strategy.Models
{
    public enum EDatabaseType
    {
        //Kullanacağımız database isimlerini enum olarak tutuyoruz db'de claim olarak saklıycaz claimlere cookieden erişim sağlayabilirz daha performanslı
        SqlServer=1,
        MongoDb=2
    }
}
