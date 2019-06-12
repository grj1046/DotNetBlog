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
        IDbConnection AccountDb { get; }
        IDbConnection BlogDb { get; }
    }

    public class DbConnectionFactory : IDbConnectionFactory
    {
        public IConfiguration Configuration { get; }
        private IDbConnection accountDb;
        private IDbConnection blogDb;

        public DbConnectionFactory(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IDbConnection AccountDb => this.accountDb ?? new MySqlConnection(Configuration.GetConnectionString("GuorjAccountConnection"));

        public IDbConnection BlogDb => this.blogDb ?? new MySqlConnection(Configuration.GetConnectionString("GuorjBlogConnection"));
    }
}
