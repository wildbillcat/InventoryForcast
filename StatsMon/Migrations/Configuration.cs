namespace StatsMon.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using StatsMon.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<StatsMon.Models.StatusMonContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(StatsMon.Models.StatusMonContext context)
        {
                            
        }
    }
}
