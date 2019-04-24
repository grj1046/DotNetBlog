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
    //public abstract class GuorjAccountDb : DbConnection
    //{
    //    public GuorjAccountDb(IConfiguration configuration)
    //    {
    //        Configuration = configuration;
    //    }
    //    public IConfiguration Configuration { get; }
    //}

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

        public IDbConnection AccountDb
        {
            get
            {
                if (this.accountDb == null)
                    this.accountDb = new MySqlConnection(Configuration.GetConnectionString("GuorjAccountConnection"));
                return this.accountDb;
            }
        }

        public IDbConnection BlogDb
        {
            get
            {
                if (this.blogDb == null)
                    this.blogDb = new MySqlConnection(Configuration.GetConnectionString("GuorjBlogConnection"));
                return this.blogDb;
            }
        }
    }
}
