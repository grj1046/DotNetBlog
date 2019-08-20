using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetBlog.Models
{
    public interface IDbConnectionFactory
    {
        MySqlConnection AccountDb { get; }
        MySqlConnection BlogDb { get; }
    }

    public class DbConnectionFactory : IDbConnectionFactory
    {
        public IConfiguration Configuration { get; }
        private MySqlConnection accountDb;
        private MySqlConnection blogDb;

        public DbConnectionFactory(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public MySqlConnection AccountDb => this.accountDb ?? (this.accountDb = new MySqlConnection(Configuration.GetConnectionString("GuorjAccountConnection")));

        public MySqlConnection BlogDb => this.blogDb ?? (this.blogDb = new MySqlConnection(Configuration.GetConnectionString("GuorjBlogConnection")));
    }
}
