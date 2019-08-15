using System.Data;
using FluentMigrator;
using NzbDrone.Core.Datastore.Migration.Framework;

namespace NzbDrone.Core.Datastore.Migration
{
    [Migration(132)]
    public class tv_to_books : NzbDroneMigrationBase
    {
        protected override void MainDbUpgrade()
        {
            Delete.Table("Series");
            Delete.Table("Seasons");
            Delete.Table("Episodes");

            Create.TableForModel("Books")
                .WithColumn("ISBN").AsInt64().Unique()
                .WithColumn("Title").AsString()
                .WithColumn("SubTitle").AsString()
                .WithColumn("Overview").AsString()
                .WithColumn("Publisher").AsString()
                .WithColumn("PublishDate").AsDateTime()
                .WithColumn("Monitored").AsBoolean()
                .WithColumn("TitleSlug").AsString()
                .WithColumn("Path").AsString()
                .WithColumn("Added").AsDateTime();
        }
    }
}
