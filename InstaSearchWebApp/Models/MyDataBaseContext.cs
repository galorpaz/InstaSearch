namespace InstaSearchWebApp.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations;

    public class MyDataBaseContext : DbContext
    {
        // Your context has been configured to use a 'MyDataBaseContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'InstaSearchWebApp.Models.Search' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'Search' 
        // connection string in the application configuration file.
        public MyDataBaseContext() : base("name=MyDbConnection")
        {   
        }
        public System.Data.Entity.DbSet<Search> Searches { get; set; }
        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
    }

    public class Search
    {
        [Required]
        public string Term{ get; set; }

        [Key]
        [Required]
        [DataType(DataType.Date)]
        public DateTime SearchTime { get; set; }
    }    
}