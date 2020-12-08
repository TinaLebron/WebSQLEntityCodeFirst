namespace WebSQLEntityCodeFirst.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WebSQLEntityCodeFirst.EntityFramework.EntityFramework.SeedData;

    internal sealed class Configuration : DbMigrationsConfiguration<WebSQLEntityCodeFirst.EntityFramework.EntityFramework.SchoolContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WebSQLEntityCodeFirst.EntityFramework.EntityFramework.SchoolContext context)
        {

            //防止重複執行
            if (context.Student.Count() == 0) { new InitialWebSQLEntityCodeFirstDbBuilder(context).Create(); }
        }
    }
}
